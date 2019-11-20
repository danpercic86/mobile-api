using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Models.Requests;
using itec_mobile_api_final.Models.Responses;
using itec_mobile_api_final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace itec_mobile_api_final.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [SwaggerTag("JWT-based authentication. After logging in, a token is received from the API. " +
                "You will need to provide this token for every subsequent request, in the Authorization header " +
                "(as 'bearer {token}' - don't forget the prefix!).")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<User> _userManager;

        public IdentityController(IIdentityService identityService, UserManager<User> userManager)
        {
            _identityService = identityService;
            _userManager = userManager;
        }

        /// <summary>
        /// The user will receive a link by email, which will need to be opened and handled in-app.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(a => a.ErrorMessage))
                });
            }
            
            var authResponse = await _identityService.RegisterAsync(request);

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
        
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await HttpContext.GetCurrentUserAsync(_userManager);
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
            });
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Update")]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var user = await HttpContext.GetCurrentUserAsync(_userManager);
            var updateResponse = await _identityService.UpdateAsync(user, request);
            
            if (!updateResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = updateResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = updateResponse.Token
            });
        }
    }
}