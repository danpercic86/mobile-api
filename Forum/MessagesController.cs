using System;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Forum
{
    [Route("/api/Forum/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    public class MessagesController: Controller
    {
        private readonly IRepository<MessageEntity> _messagesRepository;
        
        public MessagesController(ApplicationDbContext context)
        {
            _messagesRepository = context.GetRepository<MessageEntity>();
        }

        /// <summary>
        /// Get a message details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(ForbidResult), 403)]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _messagesRepository.GetAsync(id);
            if (existing is null) return NotFound();
            if (existing.UserId != userId) return Forbid();
            
            return Ok(existing);
        }
        
        /// <summary>
        /// Update a message. User must be owner.
        /// </summary>
        /// <param name="message">Message properties</param>
        /// <param name="id">Message id</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(MessageEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(ForbidResult), 403)]
        public async Task<IActionResult> Update([FromBody] dynamic message ,[FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _messagesRepository.GetAsync(id);
            if (existing is null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            existing = ReflectionHelper.PatchObject(existing, message);
            existing.LastEdited = DateTime.Now;

            await _messagesRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        /// <summary>
        /// Delete a message. User must be owner.
        /// </summary>
        /// <param name="id">Message id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(ForbidResult), 403)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _messagesRepository.GetAsync(id);
            if (existing is null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            await _messagesRepository.DeleteAsync(existing);
            return Ok("Message deleted!");
        }
    }
}