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
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ApplicationDbContext context, ILogger<PlayerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer()
        {
            return await _context.Player.ToListAsync();
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Player.FindAsync(id);

            if (player == null)
            {
                return NotFound("This player does not exist");
            }

            return player;
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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

        // POST: api/Player
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            if (player.TeamId == 0)
            {
                return BadRequest("A new player must be assigned to a team. Please pass a team ID.");
            }

            if (!_context.Team.Any(c => c.TeamId == player.TeamId))
            {
                _logger.LogInformation($"Failed to find a team with Id ({player.TeamId})the user passed");
                return BadRequest($"A team with the ID {player.TeamId} doesn't exist");
            }

            if (player.Name != null)
            {
                _context.Player.Add(player);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);
            }

            return BadRequest();
          
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.PlayerId == id);
        }
    }
}
