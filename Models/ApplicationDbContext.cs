using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebCoursework.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

    }
}

