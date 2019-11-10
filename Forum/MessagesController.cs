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
    public class MessagesController: Controller
    {
       
        private readonly IRepository<MessageEntity> _messagesRepository;
        
        public MessagesController(ApplicationDbContext context)
        {
            _messagesRepository = context.GetRepository<MessageEntity>();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromBody] dynamic message , string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var existing = await _messagesRepository.GetAsync(id);
            if (existing is null || existing.UserId != userId)
            {
                return NotFound();
            }

            existing = ReflectionHelper.PatchObject(existing, message);
            existing.LastEdited = DateTime.Now;

            await _messagesRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var existing = await _messagesRepository.GetAsync(id);
            if (existing is null || existing.UserId != userId)
            {
                return NotFound();
            }

            await _messagesRepository.DeleteAsync(existing);
            return Ok("Message deleted!");
        }
    }
}