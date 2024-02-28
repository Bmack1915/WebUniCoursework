using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;

namespace WebCoursework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ApplicationDbContext context, ILogger<TeamController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Team
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeam()
        {
            return await _context.Team.ToListAsync();
        }

        // GET: api/Team/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Team.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            //Player doesn't exist
            if (!TeamExists(id))
            {
                return TeamNotExistMessage(id);
            }

            if (id != team.TeamId)
            {
                _logger.LogInformation($"URL ID {id} doesn't match request team ID {team.TeamId}");
                return BadRequest($"The player ID in the URL ({id}) does not match the player ID ({team.TeamId}) in the request body");
            }

            if (team.LeagueId == 0)
            {
                _logger.LogInformation("User attempted to edit a team without assigning them to a League");
                return BadRequest("You must assign a League to a team. Please pass a team ID.");
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Team
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            if (team.LeagueId == 0)
            {
                _logger.LogInformation("User attempted to create a Team without assigning them to a League");
                return BadRequest("A team must be assigned to a League. Please pass a League ID.");
            }

            if (!_context.League.Any(c => c.LeagueId == team.LeagueId))
            {
                _logger.LogInformation($"Failed to find a League with Id ({team.LeagueId}) that the user passed");
                return BadRequest($"A League with the ID {team.LeagueId} doesn't exist");
            }
            _context.Team.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.TeamId }, team);
        }

        // DELETE: api/Team/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            if (!TeamExists(id))
            {
                return TeamNotExistMessage(id);
            }
            var team = await _context.Team.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Team.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.TeamId == id);
        }
        private IActionResult TeamNotExistMessage(int id)
        {
            _logger.LogInformation($"Failed to find a team with Id ({id}) passed by the user");
            return BadRequest($"A team with ID {id} does not exist");
        }
    }
}
