using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Models;
using Microsoft.EntityFrameworkCore;

namespace Chubberino.Database.Contexts
{
    public sealed class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(TwitchInfo.DatabaseConnectionString);
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<StartupChannel> StartupChannels { get; set; }
    }
}
