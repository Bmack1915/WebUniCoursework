using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCoursework.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoursework.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public DataController(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/DataGeneration
        [HttpPost]
        public IActionResult PostDataGeneration()
        {
            //Create a league
            League league = new League()
            {
                LeagueId = 1,
                Name = "Division 1"
            };

            _context.League.Add(league);

            Venue venue1 = new Venue()
            {
                VenueId = 1,
                Name = "Bernabau",
                Capacity = 80000
            };
            Venue venue2 = new Venue()
            {
                VenueId = 2,
                Name = "OldTrafford",
                Capacity = 90000
            };

           
            _context.Venue.Add(venue1);
            _context.Venue.Add(venue2);

            string[] starting11 = {
            "De Gea", "Shaw", "Bailly", "Maguire", "Wan-Bissaka",
            "Fred", "Pogba", "Sancho", "Fernandes", "Rashford", "Ronaldo"
            };

            int n = 0;

            foreach (var player in starting11)
            {
                n++;
                _context.Player.Add(new Player
                {
                    Name = player,
                    PlayerId = n,
                    TeamId = 1
                }) ;
            }

            _context.Team.Add(new Team
            {
                Name = "UTD",
                LeagueId = 1,
                TeamId = 1
            });

            _context.SaveChangesAsync();

            return CreatedAtAction("GetLeague", new { id = league.LeagueId }, league);
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

