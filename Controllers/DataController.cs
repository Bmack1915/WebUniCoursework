using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCoursework.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoursework.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataController> _logger;

        public DataController(ApplicationDbContext context, ILogger<DataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/Data
        //Auto generate some data for the DB if required
        [HttpPost]
        public async Task Post()
        {
            //Create a league
            League league = new League()
            {
                LeagueId = 1,
                Name = "Division 1"
            };

            League league2 = new League()
            {
                LeagueId = 2,
                Name = "Division 2"
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

            string[] starting11utd = {
            "Onana", "Amrabat", "Varane", "Maguire", "Dalot",
            "Casemiro", "Mainoo", "Garnacho", "Fernandes", "Rashford", "Antony"
              };


            int n = 0;

            _context.Team.Add(new Team
            {
                Name = "Manchester United",
                LeagueId = 1,
                TeamId = 1
            });

            foreach (var player in starting11utd)
            {
                n++;
                _context.Player.Add(new Player
                {
                    Name = player,
                    PlayerId = n,
                    TeamId = 1
                });
            }

            _context.Team.Add(new Team
            {
                Name = "Real Madrid",
                LeagueId = 1,
                TeamId = 2
            });


            string[] starting11rma = {
            "Lunin", "Odriozola", "Nacho", "Militão", "Miguel Gutiérrez",
              "Valverde", "Camavinga", "Kroos", "Asensio", "Vini Jr", "Benzema"};

            foreach (var player in starting11rma)
            {
                n++;
                _context.Player.Add(new Player
                {
                    Name = player,
                    PlayerId = n,
                    TeamId = 2
                });
            }

            _context.Match.Add(new Match()
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                VenueId = 1,
                HomeTeamScore = 5,
                AwayTeamScore = 0,
                Date = DateTime.Today
            });


            _context.Match.Add(new Match()
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                VenueId = 2,
                HomeTeamScore = 0,
                AwayTeamScore = 4,
                Date = DateTime.Now.AddDays(-7)
            });

            _context.Match.Add(new Match()
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                VenueId = 2,
                HomeTeamScore = 1,
                AwayTeamScore = 3,
                Date = DateTime.Now.AddDays(-14)
            });

            await _context.SaveChangesAsync();
        }
    }
}

