using System;
using itec_mobile_api_final.Data;
using System.Linq;
using System.Threading.Tasks;
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
    public class TopicsController: Controller
    {
       
        private readonly IRepository<TopicEntity> _topicRepository;
        private readonly IRepository<MessageEntity> _messageRepository;
        
        public TopicsController(ApplicationDbContext context)
        {
            _topicRepository = context.GetRepository<TopicEntity>();
            _messageRepository = context.GetRepository<MessageEntity>();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var allAsync = await _topicRepository.GetAllAsync();
            if (!allAsync.Any())
            {
                return NotFound();
            }
            
            return Ok(allAsync);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var topic = await _topicRepository.GetAsync(id);
            if (topic is null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromBody] dynamic topic, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var existing = await _topicRepository.GetAsync(id);
            if (existing is null || existing.UserId != userId)
            {
                return NotFound();
            }
            
            existing = ReflectionHelper.PatchObject(existing, topic);
            existing.LastEdited = DateTime.Now;
            
            await _topicRepository.UpdateAsync(existing);
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
            
            var existing = await _topicRepository.GetAsync(id);
            if (existing is null || existing.UserId != userId)
            {
                return NotFound();
            }

            await _topicRepository.DeleteAsync(existing);
            return Ok("Topic deleted!");
        }

        [HttpGet("{id}/Messages")]
        public async Task<IActionResult> GetMessage([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var allAsync = await _messageRepository.GetAllAsync();
            if (!allAsync.Any())
            {
                return NotFound();
            }
            
            var messages = allAsync.Where(m => m.TopicId == id);
            if (!messages.Any())
            {
                return NotFound();
            }

            return Ok(messages);
        }
        
        [HttpPost("{id}/Messages")]
        public async Task<IActionResult> AddMessage([FromBody] MessageEntity message, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            message.TopicId = id;
            message.UserId = userId;
            message.Created = DateTime.Now;
            message.LastEdited = DateTime.Now;    

            await _messageRepository.AddAsync(message);

            return CreatedAtAction(nameof(GetOne), new {id = message.Id}, message);
        }
    }
}