using System;
using System.Text.Json.Serialization;
using WebCoursework.Models;
using System.ComponentModel.DataAnnotations;

namespace WebCoursework.Models
{
	public class League
	{
        public int LeagueId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        //[JsonIgnore]
        public List<Team>? Teams { get; set; }
    }
}


