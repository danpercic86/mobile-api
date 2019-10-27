using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace itec_mobile_api_final.Stations
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : Controller
    {
        private readonly IRepository<StationEntity> _stationRepo;
        private readonly IRepository<SocketsEntity> _socketRepo;

        public StationsController(ApplicationDbContext context)
        {
            _stationRepo = context.GetRepository<StationEntity>();
            _socketRepo = context.GetRepository<SocketsEntity>();
        }

        [HttpGet]
        public IQueryable<StationEntity> GetAll()
        {
            var stations = _stationRepo.GetAll();

            return stations;
        }

        [HttpGet("{id}")]
        public ActionResult<StationEntity> GetOne(string id)
        {
            var station = _stationRepo.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            return station;
        }

        [HttpPost]
        public ActionResult<StationEntity> Post(StationEntity station)
        {
            _stationRepo.Add(station);

            return CreatedAtAction(nameof(GetOne), new {id = station.Id}, station);
        }

        [HttpDelete]
        public ActionResult<StationEntity> Delete(string id)
        {
            var station = _stationRepo.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            _stationRepo.Delete(station);
            return Ok("Station deleted!");
        }

        [HttpGet("{id}/GetSockets")]
        public IQueryable<SocketsEntity> GetAll(string id)
        {
            var sockets = _socketRepo.GetAll().Where(s => s.Station.Id == id);

            return sockets;
        }

        [HttpGet("GetSocket/{id}")]
        public ActionResult<SocketsEntity> GetOneSocket(string id)
        {
            var socket = _socketRepo.Get(id);

            if (socket == null)
            {
                return NotFound();
            }

            return socket;
        }

        [HttpPost("{id}/AddSocket")]
        public ActionResult<SocketsEntity> Post([FromBody] SocketsEntity socket, [FromRoute] string id)
        {
            _socketRepo.Queryable.Include(s => s.Station);
            Console.WriteLine("is = " + id);
            socket.Station = _stationRepo.Get(id);
            Console.WriteLine("isnull" + (socket.Station == null));

            _socketRepo.Add(socket);

            return CreatedAtAction(nameof(GetOneSocket), new {id = socket.Id}, socket);
        }
    }
}