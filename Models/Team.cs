using System;
namespace WebCoursework.Models
{
	public class Team
	{
        public int TeamId { get; set; }
        public string Name { get; set; }

        public List<Player>? Players { get; set; }
    }
}

