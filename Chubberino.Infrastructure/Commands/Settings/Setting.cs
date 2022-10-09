using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;

namespace Chubberino.Infrastructure.Commands.Settings;

public abstract class Setting : Command, ISetting
{
    public virtual String Status { get; } = String.Empty;

    public Boolean IsEnabled { get; set; }

    protected Setting(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public event EventHandler<OnSettingStateChangeArgs> OnSettingStateChange;

    public override void Execute(IEnumerable<String> arguments)
    {
        UpdateState(SettingState.Toggle);
    }

    protected void UpdateState(SettingState state)
    {
        OnSettingStateChange?.Invoke(this, new(state));
    }
}
