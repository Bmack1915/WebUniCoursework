using System;
using System.Text.Json.Serialization;

namespace WebCoursework.Models
{
	public class Match
	{
		public int MatchId { get; set; }
		public DateTime Date { get; set; }
		public int VenueId { get; set; }
		public int HomeTeamId { get; set; }
		public int AwayTeamId { get; set; }
		public int HomeTeamScore { get; set; }
		public int AwayTeamScore { get; set; }

		[JsonIgnore]
		public Venue? Venue { get; set; }
		[JsonIgnore]
		public Team? HomeTeam { get; set; }
		[JsonIgnore]
		public Team? AwayTeam { get; set; }
	
	}
}

