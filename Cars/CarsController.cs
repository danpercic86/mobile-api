using System;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Helpers;
using itec_mobile_api_final.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Cars
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarsController : Controller
    {
        private readonly IRepository<CarEntity> _carRepository;
        
        public CarsController(ApplicationDbContext context)
        {
            _carRepository = context.GetRepository<CarEntity>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var cars = await _carRepository.Queryable.Where(c => c.UserId == userId).ToListAsync();
            if (cars is null) return NotFound();

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var car = await _carRepository.GetAsync(id);
            if (car is null || car.UserId != userId) return NotFound();

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarEntity car)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            car.UserId = userId;
            car.Id = Guid.NewGuid().ToString();
            
            await _carRepository.AddAsync(car);
            return CreatedAtAction(nameof(GetOne), new {id = car.Id}, car);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromBody] dynamic car1, [FromRoute] string id)
        {
            var car = _carRepository.GetAsync(id).Result;
            if (car is null) return NotFound();

            car = ReflectionHelper.PatchObject(car, car1);

            await _carRepository.UpdateAsync(car);
            return Ok(car);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();
            
            var car = await _carRepository.GetAsync(id);
            if (car is null || car.UserId != userId) return NotFound();

            await _carRepository.DeleteAsync(car);
            return Ok("Car deleted!");
        }
    }
}