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
    public class VenueContext : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public VenueContext(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/VenueContext
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venue>>> GetVenue()
        {
            return await _context.Venue.ToListAsync();
        }

        // GET: api/VenueContext/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venue>> GetVenue(int id)
        {
            var venue = await _context.Venue.FindAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            return venue;
        }

        // PUT: api/VenueContext/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenue(int id, Venue venue)
        {
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

            return NoContent();
        }

        // POST: api/VenueContext
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Venue>> PostVenue(Venue venue)
        {
            _context.Venue.Add(venue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenue", new { id = venue.VenueId }, venue);
        }

        // DELETE: api/VenueContext/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
    }
}
