using System;
using System.Text.Json.Serialization;

namespace WebCoursework.Models
{
	public class Venue
	{
		public int VenueId { get; set; }
		public string Name { get; set;}
		public int Capacity { get; set; }

		//List of matches?
		[JsonIgnore]
		List<Match>? Matches { get; set; }
	}
}

