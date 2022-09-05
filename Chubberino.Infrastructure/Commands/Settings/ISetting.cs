using System;

namespace Chubberino.Infrastructure.Commands.Settings;

public interface ISetting : ICommand
{
    String Status { get; }

    /// <summary>
    /// Indicates the setting is enabled or disabled.
    /// </summary>
    /// <remarks>
    /// It is the responsiblity of the whatever is managing the commands to
    /// eternally set this value, even if a command has internal logic that
    /// is affected by this. This is because if the command needs to set
    /// this value, it will instead send an event to
    /// <see cref="OnSettingStateChange"/> instead.
    /// </remarks>
    Boolean IsEnabled { get; set; }

    event EventHandler<OnSettingStateChangeArgs> OnSettingStateChange;
}
