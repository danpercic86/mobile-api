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
    [Produces("application/json")]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    public class CategoryController : Controller
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<TopicEntity> _topicRepository;

        public CategoryController(ApplicationDbContext context)
        {
            _categoryRepository = context.GetRepository<CategoryEntity>();
            _topicRepository = context.GetRepository<TopicEntity>();
        }

        /// <summary>
        /// List all root categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryEntity>), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var categories = await _categoryRepository.Queryable.Where(c => c.ParentId == null).ToListAsync();
            if (!categories.Any()) return NotFound();

            return Ok(categories);
        }

        /// <summary>
        /// Get the category details.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var category = await _categoryRepository.GetAsync(id);
            if (category is null) return NotFound();

            return Ok(category);
        }
        
        /// <summary>
        /// List the category's child categories.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        [HttpGet("{id}/Categories")]
        [ProducesResponseType(typeof(IEnumerable<CategoryEntity>), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetChildCategories([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();
            
            var categories = await _categoryRepository.Queryable.Where(a => a.ParentId == id).ToListAsync();
            if (!categories.Any()) return NotFound();

            return Ok(categories);
        }
        
        /// <summary>
        /// List the category's child topics.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        [HttpGet("{id}/Topics")]
        [ProducesResponseType(typeof(IEnumerable<TopicEntity>), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> GetChildTopics([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var topics = await _topicRepository.Queryable.Where(t => t.CategoryId == id).ToListAsync();
            if (!topics.Any()) return NotFound();

            return Ok(topics);
        }

        /// <summary>
        /// Create a root category.
        /// </summary>
        /// <param name="category">Category properties</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CategoryEntity>), 200)]
        public async Task<IActionResult> Add([FromBody] CategoryEntity category)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            category.Id = Guid.NewGuid().ToString();
            category.ParentId = null;
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            var created = await _categoryRepository.GetAsync(category.Id);
            
            return Ok(created);
        }

        /// <summary>
        /// Create a child category.
        /// </summary>
        /// <param name="category">Category properties</param>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        [HttpPost("{id}/Category")]
        [ProducesResponseType(typeof(CategoryEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> AddChildCategory([FromBody] CategoryEntity category, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _categoryRepository.GetAsync(id);
            if (existing is null) return NotFound();
            
            category.Id = Guid.NewGuid().ToString();
            category.ParentId = id;
            category.UserId = userId;
            category.LastEdited = DateTime.Now;
            
            await _categoryRepository.AddAsync(category);
            var created = await _categoryRepository.GetAsync(category.Id);
            
            return Ok(created);
        }

        /// <summary>
        /// Create a child topic.
        /// </summary>
        /// <param name="topic">Topic properties</param>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        [HttpPost("{id}/Topic")]
        [ProducesResponseType(typeof(TopicEntity), 200)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<IActionResult> AddChildTopic([FromBody] TopicEntity topic, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();
            
            var existing = await _categoryRepository.GetAsync(id);
            if (existing is null) return NotFound();

            topic.Id = Guid.NewGuid().ToString();
            topic.CategoryId = id;
            topic.UserId = userId;
            topic.Created = DateTime.Now;
            topic.LastEdited = DateTime.Now;

            await _topicRepository.AddAsync(topic);
            var created = await _topicRepository.GetAsync(topic.Id);
            
            return Ok(created);
        }
    }
}