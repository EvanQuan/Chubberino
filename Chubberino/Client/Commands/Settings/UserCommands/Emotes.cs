using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Emotes : UserCommand
    {
        public IEmoteManager EmoteManager { get; }

        public Emotes(ITwitchClientManager client, IConsole console, IEmoteManager emoteManager)
            : base(client, console)
        {
            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = TwitchClientManager.IsBot;
            EmoteManager = emoteManager;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out IEnumerable<String> words)) { return; }

            if (!words.TryGetFirst(out var keyword, out var next)) { return; }

            // TODO add channel admins
            if (e.ChatMessage.Username.Equals("chubbehmouse", StringComparison.OrdinalIgnoreCase) || e.ChatMessage.Username.Equals(e.ChatMessage.Channel, StringComparison.OrdinalIgnoreCase))

            switch (keyword.ToLower())
            {
                case "r":
                case "reload":
                case "refresh":
                        EmoteManager.Refresh(e.ChatMessage.Channel);
                        TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, "Refreshed emotes.");
                    break;
                case "a":
                case "add":
                        {
                            if (next.TryGetFirst(out var categoryString, out var next2)
                                && categoryString.TryParseEnum(out EmoteCategory category)
                                && next2.TryGetFirst(out var emote, out _))
                            {
                                EmoteManager.Add(emote, category, e.ChatMessage.Channel);
                                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, $"Added {category} emote: {emote}");
                            }
                        }
                    break;
                case "d":
                case "delete":
                case "remove":
                        {
                            if (next.TryGetFirst(out var categoryString, out var next2)
                                && categoryString.TryParseEnum(out EmoteCategory category)
                                && next2.TryGetFirst(out var emote, out _))
                            {
                                if (EmoteManager.TryRemove(emote, category, e.ChatMessage.Channel))
                                {
                                    TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, $"Deleted {category} emote: {emote}");
                                }
                                else
                                {
                                    TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, $"Could not find {category} emote to delete: {emote}");
                                }
                            }
                        }
                    break;
            }
        }
    }
}
