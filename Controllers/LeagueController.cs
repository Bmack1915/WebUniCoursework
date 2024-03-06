using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;

namespace WebCoursework.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeagueController> _logger;

        public LeagueController(ApplicationDbContext context, ILogger<LeagueController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/League
        [HttpGet]
        public async Task<ActionResult<IEnumerable<League>>> GetLeague()
        {
            _logger.LogInformation("Leagues successfully retrieved");
            return await _context.League.ToListAsync();
        }

        // GET: api/League/5
        [HttpGet("{id}")]
        public async Task<ActionResult<League>> GetLeague(int id)
        {
            //Include list of teams
            var league = await _context.League.Include(League => League.Teams).FirstOrDefaultAsync(Team => Team.LeagueId == id);

            if (league == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"League (ID: {id}) successfully retrieved");
            return league;
        }

        // PUT: api/League/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutLeague(int id, League league)
        {
            if (!LeagueExists(id))
            {
                return LeagueDoesntExistMessage(id);
            }

            if (id != league.LeagueId)
            {
                _logger.LogInformation($"URL ID {id} doesn't match request League ID {league.LeagueId}");
                return BadRequest($"The League ID in the URL ({id}) does not match the League ID ({league.LeagueId}) in the request body");
            }

            _context.Entry(league).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation($"League (ID: {league.LeagueId}) successfully edited");
            return NoContent();
        }

       

        // POST: api/League
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<League>> PostLeague(League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.League.Add(league);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"League (ID: {league.LeagueId}) successfully created");
            return CreatedAtAction("GetLeague", new { id = league.LeagueId }, league);
        }

        // DELETE: api/League/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            var league = await _context.League.FindAsync(id);

            if (league == null)
            {
                return NotFound();
            }

            if (_context.Team.Any(t => t.LeagueId == id))
            {
                _logger.LogInformation($"Failed to delete a League, ID: ({id}) as it still contains teams");
                return BadRequest($"League ID: {id} has teams on it so can't be deleted.");
            }

            _context.League.Remove(league);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"League (ID: {id}) successfully edited");
            return NoContent();
        }

        private bool LeagueExists(int id)
        {
            return _context.League.Any(e => e.LeagueId == id);
        }

        private IActionResult LeagueDoesntExistMessage(int id)
        {
            _logger.LogInformation($"Failed to find a League with Id ({id}) passed by the user");
            return BadRequest($"A League with ID {id} does not exist");
        }
    }
}
