using System.IO;
using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings;

/// <summary>
/// Sets the user color to a random <see cref="ChatColorPresets"/> after
/// each message. Since setting the color requires sending a message, the
/// color may not change quick enough if messages are being sent near the
/// throttle limit.
/// </summary>
public sealed class Color : Setting
{
    public IList<IColorSelector> Selectors { get; }

    public String CurrentColor { get; set; }

    private IColorSelector CurrentSelector { get; set; }

    public override String Status =>
        $"\n\tType: {CurrentSelector.Name}";

    public Color(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
        Selectors = new List<IColorSelector>();
        CurrentSelector = Selectors.FirstOrDefault();
    }

    public override void Register(ITwitchClient client)
    {
        client.OnMessageSent += TwitchClient_OnMessageSent;
        client.OnMessageReceived += Client_OnMessageReceived;
    }

    public override void Unregister(ITwitchClient client)
    {
        client.OnMessageSent -= TwitchClient_OnMessageSent;
        client.OnMessageReceived -= Client_OnMessageReceived;
    }

    public Color AddColorSelector(IColorSelector colorSelector)
    {
        Selectors.Add(colorSelector);
        CurrentSelector = colorSelector;
        return this;
    }

    /// <summary>
    /// When bot sends a message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TwitchClient_OnMessageSent(Object sender, OnMessageSentArgs e)
    {
        // Avoid infinite recursion.
        if (e.SentMessage.Message.StartsWith('.')) { return; }

        ChangeColor();
    }

    /// <summary>
    /// When message is sent manually.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Client_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Username.Equals(e.ChatMessage.BotUsername, StringComparison.OrdinalIgnoreCase))
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        CurrentColor = CurrentSelector.GetNextColor();

        TwitchClientManager.SpoolMessage($".color {CurrentColor}");
    }

    public override Boolean Set(String property, IEnumerable<String> arguments)
    {
        switch (property.ToLower())
        {
            case "t":
            case "type":
                IColorSelector proposedSelector = Selectors
                    .FirstOrDefault(x => x.Name.Value == arguments.FirstOrDefault().ToLower());

                if (proposedSelector == null)
                {
                    return false;
                }

                CurrentSelector = proposedSelector;
                return true;
            default:
                return false;
        }
    }
}
