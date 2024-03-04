using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;

namespace WebCoursework.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<WebCoursework.Models.League> League { get; set; } = default!;
        public DbSet<WebCoursework.Models.Match> Match { get; set; } = default!;
        public DbSet<WebCoursework.Models.Player> Player { get; set; } = default!;
        public DbSet<WebCoursework.Models.Team> Team { get; set; } = default!;
        public DbSet<WebCoursework.Models.Venue> Venue { get; set; } = default!;

        

    }
}

