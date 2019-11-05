using System;
using System.Linq;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Cars
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly IRepository<CarEntity> _carRepository;
        private readonly IRepository<SocketsEntity> _socketRepository;
        
        public CarsController(ApplicationDbContext context)
        {
            _carRepository = context.GetRepository<CarEntity>();
            _socketRepository = context.GetRepository<SocketsEntity>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _carRepository.Queryable.Include(s => s.Socket);
        }

        [HttpGet]
        public IQueryable<CarEntity> GetAll()
        {
            var cars = _carRepository.GetAll();
            return cars;
        }

        [HttpGet("{id}")]
        public ActionResult<CarEntity> GetOne(string id)
        {
            var car = _carRepository.Get(id);
            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpPost]
        public ActionResult<CarEntity> Post([FromBody]CarEntity car)
        {
            _carRepository.Add(car);

            return CreatedAtAction(nameof(GetOne), new {id = car.Id}, car);
        }

        [HttpPatch("{id}")]
        public ActionResult<CarEntity> Patch(CarEntity car, [FromRoute]string id)
        {
            var existing = _carRepository.Get(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Model = car.Model;
            existing.Autonomy = car.Autonomy;
            existing.Company = car.Company;
            existing.Year = car.Year;
            existing.BatteryLeft = car.BatteryLeft;
            existing.Socket.State = car.Socket.State;
            existing.Socket.Type = car.Socket.Type;
            existing.LastTechRevision = car.LastTechRevision;
            
            _carRepository.Update(existing);

            return existing;
        }

        [HttpDelete("{id}")]
        public ActionResult<CarEntity> Delete(string id)
        {
            var car = _carRepository.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            _carRepository.Delete(car);

            return Ok("Car deleted!");
        }
    }
}