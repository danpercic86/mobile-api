using System;
using System.Collections.Generic;
using itec_mobile_api_final.Data;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Forum
{
    [Route("/api/Forum/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    public class TopicsController: Controller
    {
       
        private readonly IRepository<TopicEntity> _topicRepository;
        private readonly IRepository<MessageEntity> _messageRepository;
        
        public TopicsController(ApplicationDbContext context)
        {
            _topicRepository = context.GetRepository<TopicEntity>();
            _messageRepository = context.GetRepository<MessageEntity>();
        }
        
        /// <summary>
        /// Get all topics details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TopicEntity>), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var allAsync = await _topicRepository.GetAllAsync();
            if (!allAsync.Any()) return NotFound();

            return Ok(allAsync);
        }
        
        /// <summary>
        /// Get a topic's details.
        /// </summary>
        /// <param name="id">Topic id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TopicEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var topic = await _topicRepository.GetAsync(id);
            if (topic is null) return NotFound();

            return Ok(topic);
        }

        /// <summary>
        /// Update a topic. User must be owner.
        /// </summary>
        /// <param name="topic">Topic properties</param>
        /// <param name="id">Topic id</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(TopicEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(ForbidResult), 403)]
        public async Task<IActionResult> Update([FromBody] dynamic topic, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _topicRepository.GetAsync(id);
            if (existing is null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            existing = ReflectionHelper.PatchObject(existing, topic);
            existing.LastEdited = DateTime.Now;
            
            await _topicRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        /// <summary>
        /// Delete a topic. User must be owner.
        /// </summary>
        /// <param name="id">Topic id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(ForbidResult), 403)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _topicRepository.GetAsync(id);
            if (existing is null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            await _topicRepository.DeleteAsync(existing);
            return Ok("Topic deleted!");
        }

        /// <summary>
        /// List the topic's messages.
        /// </summary>
        /// <param name="id">Topic id</param>
        /// <returns></returns>
        [HttpGet("{id}/GetMessages")]
        [ProducesResponseType(typeof(IEnumerable<MessageEntity>), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetMessage([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var messages = await _messageRepository.Queryable.Where(m => m.TopicId == id).ToListAsync();
            if (!messages.Any()) return NotFound();

            return Ok(messages);
        }
        
        /// <summary>
        /// Add a new message to the topic.
        /// </summary>
        /// <param name="message">Message properties</param>
        /// <param name="id">Topic id</param>
        /// <returns></returns>
        [HttpPost("{id}/AddMessage")]
        [ProducesResponseType(typeof(MessageEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> AddMessage([FromBody] MessageEntity message, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var existing = await _topicRepository.GetAsync(id);
            if (existing is null) return NotFound();
            
            message.Id = Guid.NewGuid().ToString();
            message.TopicId = id;
            message.UserId = userId;
            message.Created = DateTime.Now;
            message.LastEdited = DateTime.Now;    

            await _messageRepository.AddAsync(message);
            var created = await _messageRepository.GetAsync(message.Id);

            return Ok(created);
        }
    }
}