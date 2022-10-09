using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Common.Commands.Settings.Strategies;

public sealed class StopSettingStrategy : IStopSettingStrategy
{
    public StopSettingStrategy(ITwitchClientManager client)
    {
        TwitchClientManager = client;
    }

    private static LanguageExt.HashSet<String> StopWords { get; } = new()
    {
        "bot",
    };

    public ITwitchClientManager TwitchClientManager { get; }

    public Boolean ShouldStop(ChatMessage chatMessage)
    {
        if (!chatMessage.IsModerator) { return false; }

        if (chatMessage.Username.Equals("streamelements", StringComparison.OrdinalIgnoreCase)) { return false; }

        String[] messageWords = chatMessage.Message.Split(' ');

        return messageWords.Any(word => StopWords.Contains(word.ToLower()) || word.Equals(TwitchClientManager.Name.Value, StringComparison.OrdinalIgnoreCase));
    }
}
