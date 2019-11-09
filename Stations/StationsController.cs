using System;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Votes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Stations
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : Controller
    {
        private readonly IRepository<StationEntity> _stationRepo;
        private readonly IRepository<VoteEntity> _voteRepo;

        public StationsController(ApplicationDbContext context)
        {
            _stationRepo = context.GetRepository<StationEntity>();
            _voteRepo = context.GetRepository<VoteEntity>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var allAsync = await _stationRepo.GetAllAsync();
            if (allAsync == null)
            {
                return NotFound();
            }

            var stations = allAsync.Where(s => s.Deleted == false && s.Old == false);

            if (!stations.Any())
            {
                return NotFound();
            }
            

            return Ok(stations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var station = await _stationRepo.GetAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            if (station.Deleted)
            {
                return Ok(new
                {
                    Deleted = true,
                    Id = station.OldStationId,
                });
            }

            return Ok(station);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StationEntity station)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            station.UserId = userId;

            await _stationRepo.AddAsync(station);

            return CreatedAtAction(nameof(GetOne), new {id = station.Id}, station);
        }

        [HttpPost("{id}/Edit")]
        public async Task<IActionResult> Edit(StationEntity stationEntity, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var existing = await _stationRepo.GetAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            stationEntity.OldStationId = existing.Id;
            stationEntity.UserId = userId;
            existing.Old = true;

            await _stationRepo.AddAsync(stationEntity);

            return Ok(stationEntity);
        }

        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var existing = await _stationRepo.GetAsync(id);

            if (existing == null || existing.Deleted || existing.Old)
            {
                return NotFound();
            }

            var newStation = new StationEntity
            {
                Name = existing.Name,
                TotalSockets = existing.TotalSockets,
                FreeSockets = existing.FreeSockets,
                Location = existing.Location,
                OldStationId = existing.Id,
                OldStation = existing.OldStation,
                UserId = userId,
                Deleted = true,
            };
            existing.Old = true;
            
            await _stationRepo.AddAsync(newStation);

            return Ok(newStation);
        }

        [HttpPut("{id}/FreeSockets")]
        public async Task<IActionResult> UpdateSockets(string id, int freeSocketsValue)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var existing = await _stationRepo.GetAsync(id);

            if (existing == null || existing.Deleted || existing.Old)
            {
                return NotFound();
            }

            existing.FreeSockets = freeSocketsValue;

            await _stationRepo.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpGet("{id}/Votes")]
        public async Task<IActionResult> GetAllVotes(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var allAsync = await _voteRepo.GetAllAsync();
            if (!allAsync.Any())
            {
                return NotFound();
            }
            
            var votes = allAsync.Where(a => a.StationId == id);
            if (!votes.Any())
            {
                return NotFound();
            }

            return Ok(votes);
        }

        [HttpGet("{id}/Vote")]
        public async Task<IActionResult> GetUserVote(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var allAsync = await _voteRepo.GetAllAsync();
            if (!allAsync.Any())
            {
                return NotFound();
            }
            
            var vote = await allAsync.FirstOrDefaultAsync(a => a.StationId == id && a.UserId == userId);
            if (vote is null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        [HttpPut("{id}/Vote")]
        public async Task<IActionResult> UpdateVote(string id, bool? newVote)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var vote = default(VoteEntity);
            
            var allAsync = await _voteRepo.GetAllAsync();
            if (!allAsync.Any())
            {
                vote = new VoteEntity {StationId = id, UserId = userId, Vote = newVote};

                await _voteRepo.AddAsync(vote);

                return Ok(vote);
            }
            
            vote = await allAsync.FirstOrDefaultAsync(a => a.StationId == id && a.UserId == userId);
            if (vote is null)
            {
                vote = new VoteEntity {StationId = id, UserId = userId, Vote = newVote};

                await _voteRepo.AddAsync(vote);

                return Ok(vote);
            }

            vote.Vote = newVote;
            await  _voteRepo.UpdateAsync(vote);

            return Ok(vote);
        }
    }
}