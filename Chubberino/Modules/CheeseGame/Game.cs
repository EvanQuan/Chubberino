using Chubberino.Modules.CheeseGame.Database.Contexts;

namespace Chubberino.Modules.CheeseGame
{
    public class Game
    {
        public ApplicationContext Context { get; }

        public Game(ApplicationContext context)
        {
            Context = context;
        }
    }
}
