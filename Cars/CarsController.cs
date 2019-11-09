using System;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Helpers;
using itec_mobile_api_final.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Cars
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarsController : Controller
    {
        private readonly IRepository<CarEntity> _carRepository;
        private readonly UserManager<User> _userManager;
        
        public CarsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _carRepository = context.GetRepository<CarEntity>();
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
            
            var all = await _carRepository.GetAllAsync();
            var cars = all.Where(c => c.UserId == userId);

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var car = await _carRepository.GetAsync(id);
            if (car is null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetCurrentUserId();
            if (car.UserId != userId)
            {
                return Unauthorized();
            }
            
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCarRequest car)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            
            var newCar = new CarEntity
            {
                Autonomy = car.Autonomy,
                Company = car.Company,
                Model = car.Model,
                Year = car.Year,
                BatteryLeft = car.BatteryLeft,
                LastTechRevision = car.LastTechRevision,
                UserId = HttpContext.GetCurrentUserId(),
            };

            await _carRepository.AddAsync(newCar);
            return CreatedAtAction(nameof(GetOne), new {id = newCar.Id}, newCar);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromBody] dynamic car1, [FromRoute] string id)
        {
            var car = _carRepository.GetAsync(id).Result;
            if (car == null) return NotFound();

            try
            {
               car = ReflectionHelper.PatchObject(car, car1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);    
                throw;
            }

            await _carRepository.UpdateAsync(car);
            return Ok(car);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var car = await _carRepository.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            
            var userId = HttpContext.GetCurrentUserId();
            if (car.UserId != userId)
            {
                return Unauthorized();
            }
            
            await _carRepository.DeleteAsync(car);
            return Ok("Car deleted!");
        }
    }
}