using System;
using System.Threading.Tasks;
using System.Linq;
using itec_mobile_api_final.Cars;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Stations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Forum
{
    [Route("/api/Forum/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : Controller
    {
        private readonly IRepository<CategoryForumEntity> _categoryRepository;
        private readonly UserManager<User> _userManager;

        public CategoryController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _categoryRepository = context.GetRepository<CategoryForumEntity>();
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

            var all = await _categoryRepository.GetAllAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(category);
        }
        
        [HttpGet("{id}/categories")]
        public async Task<IActionResult> Get( string id)
        {
            var category = _categoryRepository.Queryable.Where(a=>a.CategoryId == id).ToArray();
            
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryForumEntity category)
        {
            var userId = HttpContext.GetCurrentUserId();
            
            category.Id = Guid.NewGuid().ToString();
            if (userId is null)
            {
                return Unauthorized();
            }

            if (!string.IsNullOrWhiteSpace(category.CategoryId))
            {
                if (!Guid.TryParse(category.CategoryId, out _))
                {
                    return BadRequest("Category id invalid");
                }
            }
            else
            {
                category.CategoryId = null;
            }

            category.Id = Guid.NewGuid().ToString();
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }
    }
}