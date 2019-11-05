using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Cars
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly IRepository<CarEntity> _carRepository;
        private readonly UserManager<IdentityUser> _userManager;
        
        public CarsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _carRepository = context.GetRepository<CarEntity>();
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var all = await _carRepository.GetAllAsync();
            var cars = all.Where(c => c.User == currentUser);
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var car = await _carRepository.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CarEntity car)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            car.User = currentUser;
            await _carRepository.AddAsync(car);

            return CreatedAtAction(nameof(GetOne), new {id = car.Id}, car);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(CarEntity car, [FromRoute]string id)
        {
            var existing = await _carRepository.GetAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Model = car.Model;
            existing.Autonomy = car.Autonomy;
            existing.Company = car.Company;
            existing.Year = car.Year;
            existing.BatteryLeft = car.BatteryLeft;
            existing.LastTechRevision = car.LastTechRevision;
            
            await _carRepository.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var car = await _carRepository.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            await _carRepository.DeleteAsync(car);

            return Ok("Car deleted!");
        }
    }
}