using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Forum
{
    [Route("/api/Forum/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : Controller
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<TopicEntity> _topicRepository;

        public CategoryController(ApplicationDbContext context)
        {
            _categoryRepository = context.GetRepository<CategoryEntity>();
            _topicRepository = context.GetRepository<TopicEntity>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var allAsync = await _categoryRepository.GetAllAsync();
            var categories = allAsync.Where(c => c.ParentId == null);
            if (!categories.Any())
            {
                return NotFound();
            }
            
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var category = await _categoryRepository.GetAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        
        [HttpGet("{id}/Categories")]
        public async Task<IActionResult> GetChildCategories(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var categories = await _categoryRepository.Queryable.Where(a => a.ParentId == id).ToListAsync();
            if (!categories.Any())
            {
                return NotFound();
            }

            return Ok(categories);
        }
        
        [HttpGet("{id}/Topics")]
        public async Task<IActionResult> GetChildTopics(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var topics = await new Task<IEnumerable<TopicEntity>>(() => _topicRepository.Queryable.Where(t => t.CategoryId == id));
            if (!topics.Any())
            {
                return NotFound();
            }

            return Ok(topics);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryEntity category)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            category.ParentId = null;
            category.Id = Guid.NewGuid().ToString();
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }

        [HttpPost("{id}/Category")]
        public async Task<IActionResult> AddChildCategory([FromBody] CategoryEntity category, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            category.ParentId = id;
            category.Id = Guid.NewGuid().ToString();
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }

        [HttpPost("{id}/Topic")]
        public async Task<IActionResult> AddChildTopic([FromBody] TopicEntity topic, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            topic.CategoryId = id;
            topic.Id = Guid.NewGuid().ToString();
            topic.UserId = userId;
            topic.Created = DateTime.Now;
            topic.LastEdited = DateTime.Now;

            await _topicRepository.AddAsync(topic);
            return Ok(topic);
        }
    }
}