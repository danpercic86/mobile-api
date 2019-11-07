using System.Threading.Tasks;
using System.Linq;
using itec_mobile_api_final.Data;
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
    public class MessageController: Controller
    {
       
        private readonly IRepository<MessageForumEntity> __messageRepository;
        private readonly UserManager<User> _userManager;
        
        public MessageController(ApplicationDbContext context, UserManager<User> userManager)
        {
            __messageRepository = context.GetRepository<MessageForumEntity>();
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
            var all = await __messageRepository.GetAllAsync();
            return Ok(all);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var message = await __messageRepository.GetAsync(id);
            if (message is null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            return Ok(message);
        }
    }
}