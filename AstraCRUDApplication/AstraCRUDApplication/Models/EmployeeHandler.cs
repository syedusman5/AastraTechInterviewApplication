using AstraCRUDApplication.Data;
using AstraCRUDApplication.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AstraCRUDApplication.Models
{
    public class EmployeeHandler
    {
        public static async Task<object> GetEmployeeList(GlobalDbContext globalDbContext, object json)
        {
            var inputRequest = JObject.Parse(json.ToString());

            string searchValue = Convert.ToString(inputRequest?["searchValue"]) ?? "";
            int startIndex = Convert.ToInt32(inputRequest?["startIndex"]);
            int limit = Convert.ToInt32(inputRequest?["limit"]);
            string columnName = Convert.ToString(inputRequest?["columnName"]) ?? "";
            string sorting = Convert.ToString(inputRequest?["sorting"]) ?? "";


            var resultListQuery = globalDbContext.Employees.Where(a => a.IsDeleted == 0).Select(a => new
            {
                a.EmployeeUniqueId,
                a.FirstName,
                a.LastName,
                a.DepartmentId,
                departmentName = globalDbContext.Department
                                            .Where(d => d.Id == a.DepartmentId)
                                            .Select(d => d.Name)
                                            .FirstOrDefault(),
                a.Location,
                a.IsActive,
                a.CreatedAt
            });

            var resultList = await resultListQuery.ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                resultList = resultList.Where(w =>
                    (w.FirstName != null && w.FirstName.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                    (w.LastName != null && w.LastName.Contains(searchValue, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(sorting) && !string.IsNullOrEmpty(columnName))
            {
                bool isAscending = sorting == "1";

                resultList = columnName switch
                {
                    "FirstName" => isAscending ? resultList.OrderBy(a => a.FirstName).ToList() :
                                                resultList.OrderByDescending(a => a.FirstName).ToList(),

                    "LastName" => isAscending ? resultList.OrderBy(a => a.LastName).ToList() :
                                                resultList.OrderByDescending(a => a.LastName).ToList(),

                    "Location" => isAscending ? resultList.OrderBy(a => a.Location).ToList() :
                                                resultList.OrderByDescending(a => a.Location).ToList(),

                    _ => resultList.OrderByDescending(d => d.CreatedAt).ToList()
                };
            }

            if (startIndex >= 0 && limit > 0)
            {
                resultList = resultList.Skip(startIndex).Take(limit).ToList();
            }

            return ResponseHandler.SuccessResponseHelperWithObject(resultList);
        }

        public static async Task<object> EditEmployeeListFetch(GlobalDbContext globalDbContext, object json)
        {
            var inputRequest = JObject.Parse(json.ToString());

            string uniqueId = Convert.ToString(inputRequest?["uniqueId"]) ?? "";


            if (!string.IsNullOrEmpty(uniqueId))
            {
                var result = await globalDbContext.Employees
                                    .Where(e => e.EmployeeUniqueId == uniqueId)
                                    .Select(e => new
                                    {
                                        e.EmployeeUniqueId,
                                        e.FirstName,
                                        e.LastName,
                                        DepartmentName = globalDbContext.Department
                                            .Where(d => d.Id == e.DepartmentId)
                                            .Select(d => d.Name)
                                            .FirstOrDefault(),
                                        e.Location,
                                        e.IsActive
                                    })
                                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    return ResponseHandler.SuccessResponseHelperWithObject(result);
                }
                else
                {
                    return ResponseHandler.FailureResponseHelper("Invalid Employee ID");
                }
            }
            else
            {
                return ResponseHandler.FailureResponseHelper("Employee ID is manditory");
            }



        }

        public static async Task<object> EmployeeAction(GlobalDbContext globalDbContext, object json)
        {
            var inputRequest = JObject.Parse(json.ToString());

            string uniqueId = Convert.ToString(inputRequest?["uniqueId"]) ?? "";
            string firstName = Convert.ToString(inputRequest?["firstName"] ?? "");
            string lastName = Convert.ToString(inputRequest?["lastName"] ?? "");
            int departmentId = Convert.ToInt32(inputRequest?["departmentId"] ?? 0);
            string location = Convert.ToString(inputRequest?["location"] ?? "");
            byte isActive = Convert.ToByte(inputRequest?["isActive"] ?? 0);

            string actionUserUniqueId = Convert.ToString(inputRequest?["userUniqueId"] ?? "");
            int type = Convert.ToInt32(inputRequest?["type"] ?? 0); // 1 -> Create, 2 -> Edit, 3 -> Delete



            if (type == 3)
            {
                var employee = await globalDbContext.Employees
                    .FirstOrDefaultAsync(a => a.EmployeeUniqueId == uniqueId && a.IsDeleted == 0);

                if (employee != null)
                {
                    employee.IsDeleted = 1;
                    employee.DeletedBy = actionUserUniqueId;
                    employee.DeletedAt = DateTime.UtcNow;

                    await globalDbContext.SaveChangesAsync();

                    return ResponseHandler.SuccessResponseWithoutResultHelper();
                }
                else
                {
                    return ResponseHandler.FailureResponseHelper("Employee not found");
                }
            }
            else if (type == 2)
            {
                var departmentIdCheck = await globalDbContext.Department
                    .AnyAsync(a => a.Id == departmentId);

                if (departmentIdCheck)
                {
                    var existingEmployee = await globalDbContext.Employees
                        .FirstOrDefaultAsync(a => a.EmployeeUniqueId == uniqueId && a.IsDeleted == 0);

                    if (existingEmployee != null)
                    {
                        existingEmployee.FirstName = firstName;
                        existingEmployee.LastName = lastName;
                        existingEmployee.DepartmentId = departmentId;
                        existingEmployee.Location = location;
                        existingEmployee.IsActive = isActive;
                        existingEmployee.UpdatedAt = DateTime.UtcNow;
                        existingEmployee.UpdatedBy = actionUserUniqueId;

                        await globalDbContext.SaveChangesAsync();

                        return ResponseHandler.SuccessResponseWithoutResultHelper();
                    }
                    else
                    {
                        return ResponseHandler.FailureResponseHelper("Employee not found");
                    }
                }
                else
                {
                    return ResponseHandler.FailureResponseHelper("Invalid Department Id");
                }
            }
            else if(type == 1)
            {
                var result = new Employees
                {
                    EmployeeUniqueId = Guid.NewGuid().ToString(),
                    FirstName = firstName,
                    LastName = lastName,
                    DepartmentId = departmentId,
                    Location = location,
                    IsActive = isActive,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = actionUserUniqueId

                };

                 globalDbContext.Employees.Add(result);

                await globalDbContext.SaveChangesAsync();

                return ResponseHandler.SuccessResponseWithoutResultHelper();
            }
            else
            {
                return ResponseHandler.FailureResponseHelper("Action type is missing");
            }
        }

       
    }
}
