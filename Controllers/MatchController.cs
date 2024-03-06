using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class MatchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MatchController> _logger;
        
        public MatchController(ApplicationDbContext context, ILogger<MatchController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: api/Match
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatch()
        {
            _logger.LogInformation($"Matches successfully retrieved");
            return await _context.Match.ToListAsync();
        }

        // GET: api/Match/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Match.FindAsync(id);

            if (match == null)
            {
                return NotFound("Match not found");
            }
            _logger.LogInformation($"Match (ID: {match.MatchId}) successfully retrieved");
            return match;
        }

        // PUT: api/Match/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match match)
        {
            //Match doesn't exist
            if (!MatchExists(id))
            {
                return MatchNotExistMessage(id);
            }

            if (id != match.MatchId)
            {
                _logger.LogInformation($"URL ID {id} doesn't match request Match ID {match.MatchId}");
                return BadRequest($"The Match ID in the URL, ({id}), does not match the Match ID, ({match.MatchId}), in the request body");
            }

            if (!AreTeamIdsValid(match.HomeTeamId, match.AwayTeamId))
            {
                _logger.LogInformation("Admin attempted to edit a match but did not provide a TeamId for both teams");
                return BadRequest("A match must involve two teams, please pass a valid team ID for both teams.");
            }

            if (!AreScoresValid(match.HomeTeamScore, match.AwayTeamScore))
            {
                _logger.LogInformation("Admin attempted to edit a match without passing two valid scorelines");
                return BadRequest("A match score must involve a score from each team, please pass a valid scoreline for each team.");
            }

            if (match.VenueId == null || !_context.Venue.Any(v => v.VenueId == match.VenueId))
            {
                _logger.LogInformation($"An invalid Venue ID, {match.VenueId}, was passed when editing a match");
                return BadRequest($"An invalid Venue ID, {match.VenueId}, was passed when editing a match, please pass a valid venue ID for this match");
            }

            if (id != match.MatchId)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation($"Match (ID: {match.MatchId}) successfully edited");
            return NoContent();
        }

        // POST: api/Match
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {

            if (!AreTeamIdsValid(match.HomeTeamId, match.AwayTeamId))
            {
                _logger.LogInformation("Admin attempted to edit a match but did not provide a TeamId for both teams");
                return BadRequest("A match must involve two teams, please pass a valid team ID for both teams.");
            }

            if (!AreScoresValid(match.HomeTeamScore, match.AwayTeamScore))
            {
                _logger.LogInformation("Admin attempted to edit a match without passing two valid scorelines");
                return BadRequest("A match score must involve a score from each team, please pass a valid scoreline for each team.");
            }

            if (match.VenueId == null || !_context.Venue.Any(v => v.VenueId == match.VenueId))
            {
                _logger.LogInformation($"An invalid Venue ID, {match.VenueId}, was passed when editing a match");
                return BadRequest($"An invalid Venue ID, {match.VenueId}, was passed when editing a match, please pass a valid venue ID for this match");
            }

            _context.Match.Add(match);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Match (ID: {match.MatchId}) successfully created");
            return CreatedAtAction("GetMatch", new { id = match.MatchId }, match);
        }

        // DELETE: api/Match/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            if (!MatchExists(id))
            {
                return MatchNotExistMessage(id);
            }
            var match = await _context.Match.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Match (ID: {match.MatchId}) successfully deleted");
            return NoContent();
        }

        private bool MatchExists(int id)
        {
            return _context.Match.Any(e => e.MatchId == id);
        }

        private bool AreTeamIdsValid(int? homeTeamId, int? awayTeamId)
        {
            return homeTeamId != null && awayTeamId != null;
        }

        private bool AreScoresValid(int? homeTeamScore, int? awayTeamScore)
        {
            return homeTeamScore != null && awayTeamScore != null;
        }

        private IActionResult MatchNotExistMessage(int id)
        {
            _logger.LogInformation($"Failed to find a Match with Id ({id})");
            return BadRequest($"A Match with ID {id} does not exist");
        }


    }
}
