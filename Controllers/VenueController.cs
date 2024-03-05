using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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
    public class VenueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VenueController> _logger;

        public VenueController(ApplicationDbContext context, ILogger<VenueController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/VenueContext
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venue>>> GetVenue()
        {
            _logger.LogInformation("Venues successfully retrieved");
            return await _context.Venue.ToListAsync();
        }

        // GET: api/VenueContext/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venue>> GetVenue(int id)
        {
            var venue = await _context.Venue.FindAsync(id);

            if (venue == null)
            {
                return NotFound("Venue not found");
            }
            _logger.LogInformation($"Venue (ID: {id}) successfully retrieved");
            return venue;
        }

        // PUT: api/VenueContext/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenue(int id, Venue venue)
        {
            if (!VenueExists(id))
            {
                return VenueNotExistMessage(id);
            }

            if (id != venue.VenueId)
            {
                _logger.LogInformation($"URL ID {id} doesn't match request for Venue ID {venue.VenueId}");
                return BadRequest($"The Venue ID in the URL ({id}) does not match the Venue ID ({venue.VenueId}) in the request body");
            }

            if (venue.Capacity > 100000 || venue.Capacity < 10000)
            {
                _logger.LogInformation("An attempt to change a venue's capacity beyond league rules was made.");
                return BadRequest("League rules state you cannot increase a venue's capacity to over 100,000 or below 10,000.");
            }

            if (id != venue.VenueId)
            {
                return BadRequest();
            }

            _context.Entry(venue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation($"Venue (ID: {venue.VenueId}) successfully edited");
            return NoContent();
        }

        // POST: api/VenueContext
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Venue>> PostVenue(Venue venue)
        {
            if (venue.Capacity > 100000 || venue.Capacity < 10000)
            {
                _logger.LogInformation("An attempt to create a venue with capacity beyond league rules was made.");
                return BadRequest("League rules state you cannot have a venue's capacity over 100,000 or below 10,000.");
            }


            _context.Venue.Add(venue);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Venue (ID: {venue.VenueId}) successfully created");
            return CreatedAtAction("GetVenue", new { id = venue.VenueId }, venue);
        }

        // DELETE: api/VenueContext/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            if (!VenueExists(id))
            {
                return VenueNotExistMessage(id);
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            if (_context.Match.Any(m => m.VenueId == id))
            {
                _logger.LogInformation($"Failed to delete a venue, ID: ({id}) as holds a record of a match played here");
                return BadRequest($"Venue ID: {id} has hosted matches so can't be deleted.");
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Venue (ID: {id}) successfully deleted");
            return NoContent();
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }

        private IActionResult VenueNotExistMessage(int id)
        {
            _logger.LogInformation($"Failed to find a Venue with Id ({id}) passed by the user");
            return BadRequest($"A Venue with ID {id} does not exist");
        }
    }
}
