using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            _logger.LogInformation("List of players successfully retrieved");
            return await _context.Player.ToListAsync();
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound($"A Player with Id {id} does not exist");
            }
           
            _logger.LogInformation($"Player with ID: {id} successfully retrieved");
            return player;
        }

        // PUT: api/Player/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            //Player doesn't exist
            if (!PlayerExists(id))
            {
                return PlayerNotExistMessage(id);
            }

            if (id != player.PlayerId)
            {
                _logger.LogInformation($"URL ID {id} doesn't match request player ID {player.PlayerId}");
                return BadRequest($"The player ID in the URL ({id}) does not match the player ID ({player.PlayerId}) in the request body");
            }

            if (player.TeamId == 0)
            {
                _logger.LogInformation("Admin attempted to edit a player without assigning them to a team");
                return BadRequest("You must assign a player to a team. Please pass a team ID.");
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            if (player.TeamId == 0)
            {
                _logger.LogInformation("Admin attempted to create a player without assigning them to a team");
                return BadRequest("A new player must be assigned to a team. Please pass a team ID.");
            }

            if (!_context.Team.Any(c => c.TeamId == player.TeamId))
            {
                _logger.LogInformation($"Failed to find a team with Id ({player.TeamId}) that the user passed");
                return BadRequest($"A team with the ID {player.TeamId} doesn't exist");
            }

            if (player.Name != null)
            {
                _context.Player.Add(player);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Player (ID: {player.PlayerId}) added");
                return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);
            }

            return BadRequest();
          
        }

        // DELETE: api/Player/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            //Player doesn't exist
            if (!PlayerExists(id))
            {
                return PlayerNotExistMessage(id);
            }

            var player = await _context.Player.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Player (ID: {player.PlayerId}) deleted");

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.PlayerId == id);
        }

        private IActionResult PlayerNotExistMessage(int id)
        {
            _logger.LogInformation($"Failed to find a player with Id ({id}) passed by the user");
            return BadRequest($"A player with ID {id} does not exist");
        }
    }
}
