using System;
using WebCoursework.Models;

namespace WebCoursework.Models
{
	public class League
	{
        public int LeagueId { get; set; }
        public string Name { get; set; }

        public List<Team>? Teams { get; set; }
    }
}

