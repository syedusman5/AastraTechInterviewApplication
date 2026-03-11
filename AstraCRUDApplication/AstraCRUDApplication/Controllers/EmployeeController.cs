using AstraCRUDApplication.Data;
using AstraCRUDApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstraCRUDApplication.Controllers
{
    [Route("api/v1/")]
    public class EmployeeController : Controller
    {
        private readonly GlobalDbContext globalDbContext;

        public EmployeeController(GlobalDbContext context)
        {
            globalDbContext = context;
        }

        [Authorize]
        [HttpPost]
        [Route("employeeFetchList")]
        public async Task<object> EmployeeFetch([FromBody] object json)
        {
            try
            {
                string apiKey = Request.Headers["X-API-Key"];

                if (!CommonValidations.ValidateAccess(apiKey))
                {
                    return ResponseHandler.FailureResponseHelper("InValid API Key");
                }

                var data = await EmployeeHandler.GetEmployeeList(globalDbContext, json);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseHandler.FailureResponseHelper(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("editEmployeeListFetch")]
        public async Task<object> EditEmployeeListFetch([FromBody] object json)
        {
            try
            {
                string apiKey = Request.Headers["X-API-Key"];

                if (!CommonValidations.ValidateAccess(apiKey))
                {
                    return ResponseHandler.FailureResponseHelper("InValid API Key");
                }

                var data = await EmployeeHandler.EditEmployeeListFetch(globalDbContext, json);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseHandler.FailureResponseHelper(ex.Message);
            }
        }

        [Authorize(Roles = "1,2")]
        [HttpPost]
        [Route("employeeAction")]
        public async Task<object> EmployeeAction([FromBody] object json)
        {
            try
            {
                string apiKey = Request.Headers["X-API-Key"];

                if (!CommonValidations.ValidateAccess(apiKey))
                {
                    return ResponseHandler.FailureResponseHelper("InValid API Key");
                }

                var data = await EmployeeHandler.EmployeeAction(globalDbContext, json);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseHandler.FailureResponseHelper(ex.Message);
            }
        }
        
    }
}
