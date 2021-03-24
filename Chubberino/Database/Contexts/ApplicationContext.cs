using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Chubberino.Database.Contexts
{
    public sealed class ApplicationContext : DbContext, IApplicationContext
    {
        public const String LocalDatabaseConnectionString = @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;";

        public ApplicationContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(LocalDatabaseConnectionString);
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<StartupChannel> StartupChannels { get; set; }

        public DbSet<UserCredentials> UserCredentials { get; set; }

        public DbSet<ApplicationCredentials> ApplicationCredentials { get; set; }

        public DbSet<Boss> Bosses { get; set; }
    }
}
