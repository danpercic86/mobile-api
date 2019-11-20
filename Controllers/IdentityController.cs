﻿using System.Linq;
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

namespace itec_mobile_api_final.Controllers
{
    [Route("api/Auth")]
    [ApiController]
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
        /// Register a new account.
        /// </summary>
        /// <param name="request">User data</param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
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

        /// <summary>
        /// Login using e-mail and password.
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
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

        /// <summary>
        /// Get the user logged in by the current Authorization token.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Update current user's profile.
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Update")]
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