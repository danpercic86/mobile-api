using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            if (userId is null) return Unauthorized();

            var categories = await _categoryRepository.Queryable.Where(c => c.ParentId == null).ToListAsync();
            if (!categories.Any()) return NotFound();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var category = await _categoryRepository.GetAsync(id);
            if (category is null) return NotFound();

            return Ok(category);
        }
        
        [HttpGet("{id}/Categories")]
        public async Task<IActionResult> GetChildCategories([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();
            
            var categories = await _categoryRepository.Queryable.Where(a => a.ParentId == id).ToListAsync();
            if (!categories.Any()) return NotFound();

            return Ok(categories);
        }
        
        [HttpGet("{id}/Topics")]
        public async Task<IActionResult> GetChildTopics([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var topics = await _topicRepository.Queryable.Where(t => t.CategoryId == id).ToListAsync();
            if (!topics.Any()) return NotFound();

            return Ok(topics);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryEntity category)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            category.Id = Guid.NewGuid().ToString();
            category.ParentId = null;
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }

        [HttpPost("{id}/Category")]
        public async Task<IActionResult> AddChildCategory([FromBody] CategoryEntity category, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            category.Id = Guid.NewGuid().ToString();
            category.ParentId = id;
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }

        [HttpPost("{id}/Topic")]
        public async Task<IActionResult> AddChildTopic([FromBody] TopicEntity topic, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            topic.Id = Guid.NewGuid().ToString();
            topic.CategoryId = id;
            topic.UserId = userId;
            topic.Created = DateTime.Now;
            topic.LastEdited = DateTime.Now;

            await _topicRepository.AddAsync(topic);
            return Ok(topic);
        }
    }
}