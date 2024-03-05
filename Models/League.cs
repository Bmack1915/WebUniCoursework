using System;
using System.Text.Json.Serialization;
using WebCoursework.Models;
using System.ComponentModel.DataAnnotations;

namespace WebCoursework.Models
{
	public class League
	{
        public int LeagueId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Team>? Teams { get; set; }
    }
}


