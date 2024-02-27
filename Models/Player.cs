using System;
using System.ComponentModel.DataAnnotations;

namespace WebCoursework.Models
{
	public class Player
	{ 
		public int PlayerId { get; set; }
		public string Name { get; set; }

		public int TeamId { get; set; }
		public Team Team { get; set; }
	}
}

