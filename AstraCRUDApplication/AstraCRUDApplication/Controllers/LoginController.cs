using AstraCRUDApplication.Data;
using AstraCRUDApplication.Models;
using AstraCRUDApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstraCRUDApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly GlobalDbContext globalDbContext;
        private readonly JwtService _jwtService;

        public LoginController(GlobalDbContext context, JwtService jwtService)
        {
            globalDbContext = context;
            _jwtService = jwtService;
        }


        [HttpPost]
        [Route("LoginValidation")]
        public async Task<object> Login([FromBody] object json)
        {
            try
            {
                string apiKey = Request.Headers["X-API-Key"];

                if (!CommonValidations.ValidateAccess(apiKey))
                {
                    return ResponseHandler.FailureResponseHelper("InValid API Key");
                }

                var data = await LoginHandler.LoginValidation(globalDbContext, _jwtService, json);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseHandler.FailureResponseHelper(ex.Message);
            }
        }

        [HttpPost]
        [Route("signupUser")]
        public async Task<object> signupUser([FromBody] object json)
        {
            try
            {
                string apiKey = Request.Headers["X-API-Key"];

                if (!CommonValidations.ValidateAccess(apiKey))
                {
                    return ResponseHandler.FailureResponseHelper("InValid API Key");
                }

                var data = await LoginHandler.RegisterUser(globalDbContext, json);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseHandler.FailureResponseHelper(ex.Message);
            }
        }
    }
}
