using System;
using System.Threading.Tasks;
using itec_mobile_api_final.Models.Requests;
using itec_mobile_api_final.Models.Responses;
using itec_mobile_api_final.Services;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
    }
}