namespace Chubberino.Infrastructure.Commands.Settings;

public sealed class OnSettingStateChangeArgs : EventArgs
{
    public OnSettingStateChangeArgs(SettingState state)
    {
        RequestState = state;
    }

    /// <summary>
    /// Request to make setting enabled.
    /// </summary>
    public SettingState RequestState { get; }
}
