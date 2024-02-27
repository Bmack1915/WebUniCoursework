using System;
using System.Text.Json.Serialization;

namespace WebCoursework.Models
{
	public class Team
	{
        public int TeamId { get; set; }
        public string Name { get; set; }

        //FK
        public int LeagueId { get; set; }
        public List<Player>? Players { get; set; }
    }
}

