using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebCoursework.Models
{
	public class Player
	{ 
		public int PlayerId { get; set; }
        public string Name { get; set; }

        //FK
        public int TeamId { get; set; }
		[JsonIgnore]
		public Team? Team { get; set; }
	}
}

