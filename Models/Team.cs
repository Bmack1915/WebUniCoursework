using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebCoursework.Models
{
	public class Team
	{
        public int TeamId { get; set; }
        public string Name { get; set; }

        //FK
        public int LeagueId { get; set; }
        [JsonIgnore]
        public List<Player>? Players { get; set; }
    }
}

