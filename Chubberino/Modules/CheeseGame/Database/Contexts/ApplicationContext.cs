using Chubberino.Modules.CheeseGame.Models;
using Microsoft.EntityFrameworkCore;

namespace Chubberino.Modules.CheeseGame.Database.Contexts
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

        public DbSet<Player> Players { get; set; }
    }
}
