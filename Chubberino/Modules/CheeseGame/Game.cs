using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public class Game : IGame
    {
        public ApplicationContext Context { get; }

        public IMessageSpooler Spooler { get; set; }

        public IAddPointStrategy AddPointStrategy { get; }

        public Game(ApplicationContext context, IMessageSpooler spooler, IAddPointStrategy addPointStrategy)
        {
            Context = context;
            Spooler = spooler;
            AddPointStrategy = addPointStrategy;
        }

        public void AddPoints(ChatMessage message)
        {
            AddPointStrategy.AddPoints(message);
        }
    }
}
