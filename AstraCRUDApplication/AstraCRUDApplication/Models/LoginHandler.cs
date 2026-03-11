using AstraCRUDApplication.Data;
using AstraCRUDApplication.Models.TableModels;
using AstraCRUDApplication.Services;
using Newtonsoft.Json.Linq;

namespace AstraCRUDApplication.Models
{
    public class LoginHandler
    {
        public static async Task<object> LoginValidation(GlobalDbContext globalContext, JwtService jwtService, object json)
        {
            var inputRequest = JObject.Parse(json.ToString());

            var email = Convert.ToString(inputRequest["emailId"]);
            var password = Convert.ToString(inputRequest["password"]);

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var userData = globalContext.Users
                    .Where(a => a.EmailId == email && a.IsDeleted == 0)
                    .FirstOrDefault();

                if (userData != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(password, userData.Password))
                    {
                        return ResponseHandler.FailureResponseHelper("Invalid user details");
                    }

                    var jwtToken = jwtService.GenerateToken(userData.Name, userData.RoleId);

                    var result = new
                    {
                        uniqueId = userData.UniqueId,
                        jwtToken = jwtToken,
                    };

                    return ResponseHandler.SuccessResponseHelperWithObject(result);
                }
                else
                {
                    return ResponseHandler.FailureResponseHelper("Invalid email");
                }
            }
            else
            {
                return ResponseHandler.FailureResponseHelper("Email or Password is Mandatory");
            }
        }

        public static async Task<object> RegisterUser(GlobalDbContext globalContext, object json)
        {
            var inputRequest = JObject.Parse(json.ToString());

            var email = Convert.ToString(inputRequest["emailId"]);
            var password = Convert.ToString(inputRequest["password"]);
            var name = Convert.ToString(inputRequest["name"]);
            var gender = Convert.ToString(inputRequest["gender"]);


            var existingUser = globalContext.Users
            .Where(a => a.EmailId == email && a.IsDeleted == 0)
            .FirstOrDefault();

            if (existingUser == null)
            {

                var userData = new Users
                {
                    UniqueId = Guid.NewGuid().ToString(),
                    Name = name,
                    EmailId = email,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    Gender = gender,
                    RoleId = 3,//default user
                    IsDeleted = 0,
                    CreatedAt = DateTime.Now
                };

                await globalContext.Users.AddAsync(userData);
                await globalContext.SaveChangesAsync();

                return ResponseHandler.SuccessResponseWithoutResultHelper();
            }
            else
            {
                return ResponseHandler.FailureResponseHelper("Email already registered");
            }
        }
    }
}
