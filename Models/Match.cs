﻿using System;
namespace WebCoursework.Models
{
	public class Match
	{
		public int MatchId { get; set; }
		public DateTime Date { get; set; }

		public int VenueId { get; set; }
		public int HomeTeadId { get; set; }
		public int AwayTeamId { get; set; }

		public Venue Venue { get; set; }
		public Team HomeTeam { get; set; }
		public Team AwayTeam { get; set; }
	
	}
}
