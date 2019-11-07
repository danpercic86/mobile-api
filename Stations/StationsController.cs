using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Stations
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StationsController : Controller
    {
        private readonly IRepository<StationEntity> _stationRepo;

        public StationsController(ApplicationDbContext context)
        {
            _stationRepo = context.GetRepository<StationEntity>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allAsync = await _stationRepo.GetAllAsync();
            var stations = allAsync.Where(s => s.Deleted == false);

            return Ok(stations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var station = await _stationRepo.GetAsync(id);
            if (station == null || station.Deleted)
            {
                return NotFound();
            }

            return Ok(station);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StationEntity station)
        {
            await _stationRepo.AddAsync(station);

            return CreatedAtAction(nameof(GetOne), new {id = station.Id}, station);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(StationEntity stationEntity, [FromRoute]string id)
        {
            var existing = await _stationRepo.GetAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _stationRepo.UpdateAsync(existing);
            
            return Ok(existing);
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var station = await _stationRepo.GetAsync(id);
            
            if (station == null || station.Deleted)
            {
                return NotFound();
            }

            station.Deleted = true;
            await _stationRepo.UpdateAsync(station);

            return Ok(station);
        }
    }
}