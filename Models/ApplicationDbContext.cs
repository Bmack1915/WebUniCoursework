using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;
using Microsoft.Extensions.Logging;
using WebCoursework.Services;
using WebCoursework.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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

