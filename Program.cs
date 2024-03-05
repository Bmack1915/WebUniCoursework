using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCoursework.Models;
using Microsoft.Extensions.Logging;
using WebCoursework.Services;
using WebCoursework.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection.Metadata; 

namespace WebCoursework;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(builder.Configuration.GetConnectionString("Connection")));

        //Register Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        //Register logger
        builder.Services.AddLogging(action => action.AddConsole());

        //Email Services
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddScoped<EmailService>();

        //JwtToken Service
        builder.Services.AddScoped<RolesController>();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                adminRole.NormalizedName = "ADMIN";
                context.Roles.Add(adminRole);
            }

            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var userRole = new IdentityRole("User");
                userRole.NormalizedName = "USER";
                context.Roles.Add(userRole);
            }

            context.SaveChanges();
        }
        catch (Exception ex)
        {
          
        }



        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}

