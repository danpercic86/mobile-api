using System;
using itec_mobile_api_final.Data;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Forum
{
    [Route("/api/Forum/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TopicController: Controller
    {
       
        private readonly IRepository<TopicForumEntity> __topicRepository;
        
        private readonly UserManager<User> _userManager;
        
        public TopicController(ApplicationDbContext context, UserManager<User> userManager)
        {
           
            __topicRepository = context.GetRepository<TopicForumEntity>();
        
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var all = await __topicRepository.GetAllAsync();
            return Ok(all);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var topic = await __topicRepository.GetAsync(id);
            if (topic is null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            return Ok(topic);
        }
    
    }
}