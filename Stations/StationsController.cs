using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Data;
using itec_mobile_api_final.Extensions;
using itec_mobile_api_final.Votes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace itec_mobile_api_final.Stations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [SwaggerTag(
        "To prevent incorrect information, each station entity has a score determined by user upvotes and downvotes. " +
        "Each user can vote once on a station, saying whether the information is valid or not. " +
        "When editing or deleting a station, a new entity will be created, with a reference to the old entity. This new entity will have no votes and will need to be validated again." +
        "A station's edit history can be reconstructed by following the previous version references.")]
    public class StationsController : Controller
    {
        private readonly IRepository<StationEntity> _stationRepo;
        private readonly IRepository<VoteEntity> _voteRepo;

        public StationsController(ApplicationDbContext context)
        {
            _stationRepo = context.GetRepository<StationEntity>();
            _voteRepo = context.GetRepository<VoteEntity>();
        }

        /// <summary>
        /// List all stations.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var stations = await _stationRepo.Queryable.Where(s => s.Deleted == false && s.Old == false).ToListAsync();
            if (!stations.Any()) return NotFound();

            return Ok(stations);
        }

        /// <summary>
        /// Get station details.
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var station = await _stationRepo.GetAsync(id);
            if (station is null) return NotFound();

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

        /// <summary>
        /// Add a station.
        /// </summary>
        /// <param name="station">Station properties</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StationEntity station)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            station.Id = Guid.NewGuid().ToString();
            station.UserId = userId;

            await _stationRepo.AddAsync(station);

            return CreatedAtAction(nameof(GetOne), new {id = station.Id}, station);
        }

        /// <summary>
        /// A new station entity will be created with the updated field values. The new entity will be returned.
        /// </summary>
        /// <param name="stationEntity">Station properties</param>
        /// <param name="id">Station id</param>
        /// <returns></returns>
        [HttpPost("{id}/Edit")]
        public async Task<IActionResult> Edit([FromBody] StationEntity stationEntity, [FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _stationRepo.GetAsync(id);
            if (existing is null) return NotFound();

            stationEntity.Id = Guid.NewGuid().ToString();
            stationEntity.OldStationId = existing.Id;
            stationEntity.UserId = userId;
            existing.Old = true;

            await _stationRepo.AddAsync(stationEntity);

            return Ok(stationEntity);
        }

        /// <summary>
        /// A new station entity will be created with the deleted flag set, and all other fields set to the old station's values. The new entity will be returned.
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns></returns>
        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId is null) return Unauthorized();

            var existing = await _stationRepo.GetAsync(id);
            if (existing is null || existing.Deleted || existing.Old) return NotFound();

            var newStation = new StationEntity
            {
                Id = Guid.NewGuid().ToString(),
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

        /// <summary>
        /// Update the station's free sockets count. 0 <= value <= capacity.
        /// This update doesn't require validation from other users.
        /// </summary>
        /// <param name="id">Station id</param>
        /// <param name="freeSocketsValue">Number of free sockets</param>
        /// <returns></returns>
        [HttpPut("{id}/FreeSockets")]
        public async Task<IActionResult> UpdateSockets([FromRoute] string id, int freeSocketsValue)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var existing = await _stationRepo.GetAsync(id);
            if (existing == null || existing.Deleted || existing.Old) return NotFound();

            existing.FreeSockets = freeSocketsValue;

            await _stationRepo.UpdateAsync(existing);

            return Ok(existing);
        }

        /// <summary>
        /// Get the station's upvotes and downvotes.
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns></returns>
        [HttpGet("{id}/Votes")]
        public async Task<IActionResult> GetAllVotes([FromRoute] string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var votes = await _voteRepo.Queryable.Where(a => a.StationId == id).ToListAsync();
            if (!votes.Any()) return NotFound();

            return Ok(votes);
        }

        /// <summary>
        /// Get the current user's vote.
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns></returns>
        [HttpGet("{id}/Vote")]
        public async Task<IActionResult> GetUserVote(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var vote = await _voteRepo.Queryable.FirstOrDefaultAsync(a => a.StationId == id && a.UserId == userId);
            if (vote is null) return NotFound();

            return Ok(vote);
        }

        /// <summary>
        /// Update the current user's vote. The value can be 0 (downvote), null (no vote) or 1 (upvote).
        /// </summary>
        /// <param name="id">Station id</param>
        /// <param name="newVote">Vote value</param>
        /// <returns></returns>
        [HttpPut("{id}/Vote")]
        public async Task<IActionResult> UpdateVote([FromRoute] string id, bool? newVote)
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId == null) return Unauthorized();

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
            await _voteRepo.UpdateAsync(vote);

            return Ok(vote);
        }
    }
}