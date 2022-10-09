using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

public interface IColorSelector : INameable
{
    String GetNextColor();
}
