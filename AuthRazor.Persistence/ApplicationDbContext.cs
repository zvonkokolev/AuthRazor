using AuthRazor.Core;
using AuthRazor.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace AuthRazor.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AuthUser> AuthUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Debug.Write(configuration);

            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthUser>().HasData(new AuthUser
            {
                Id = 1,
                Email = "admin@htl.at",
                Password = AuthUtils.GenerateHashedPassword("admin@htl.at"),
                UserRole = "Administrator"
            });
            modelBuilder.Entity<AuthUser>().HasData(new AuthUser
            {
                Id = 2,
                Email = "user@htl.at",
                Password = AuthUtils.GenerateHashedPassword("user@htl.at"),
                UserRole = "User"
            });
        }
    }
}