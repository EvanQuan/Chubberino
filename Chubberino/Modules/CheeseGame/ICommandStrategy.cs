using Chubberino.Client.Abstractions;

namespace Chubberino.Modules.CheeseGame
{
    public interface ICommandStrategy
    {
        IMessageSpooler Spooler { get; set; }
    }
}
