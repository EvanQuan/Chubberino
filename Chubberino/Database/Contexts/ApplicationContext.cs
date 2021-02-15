using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Models;
using Microsoft.EntityFrameworkCore;

namespace Chubberino.Database.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base()
        {

        }

        //public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(TwitchInfo.DatabaseConnectionString);
        }

        public virtual DbSet<Player> Players { get; set; }

        public virtual DbSet<StartupChannel> StartupChannels { get; set; }
    }
}
