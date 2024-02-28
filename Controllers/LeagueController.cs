using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;

namespace WebCoursework.Controllers
{
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
            return await _context.League.ToListAsync();
        }

        // GET: api/League/5
        [HttpGet("{id}")]
        public async Task<ActionResult<League>> GetLeague(int id)
        {
            //var league = await _context.League.FindAsync(id);

            //Include list of teams
            var league = await _context.League.Include(League => League.Teams).FirstOrDefaultAsync(Team => Team.LeagueId == id);
            if (league == null)
            {
                return NotFound();
            }

            return league;
        }

        // PUT: api/League/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(int id, League league)
        {
            if (id != league.LeagueId)
            {
                return BadRequest();
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

            return NoContent();
        }

        // POST: api/League
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<League>> PostLeague(League league)
        {
            _context.League.Add(league);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeague", new { id = league.LeagueId }, league);
        }

        // DELETE: api/League/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            var league = await _context.League.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            _context.League.Remove(league);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeagueExists(int id)
        {
            return _context.League.Any(e => e.LeagueId == id);
        }
    }
}
