using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, e.ChatMessage.DisplayName + ", refreshed emotes.");
                    break;
                case "a":
                case "add":
                        {
                            if (next.TryGetFirst(out var categoryString, out var next2)
                                && categoryString.TryParseEnum(out EmoteCategory category))
                            {
                                // Filter out single character words, such as chatterino message duplicate characters.
                                var emotes = next2.Where(x => x.Length >= 2);
                                var results = EmoteManager.AddAll(emotes, category, e.ChatMessage.Channel);

                                StringBuilder builder = new StringBuilder()
                                    .Append(e.ChatMessage.DisplayName)
                                    .Append(',');

                                if (results.Succeeded.Any())
                                {
                                    builder
                                        .Append(" Added ")
                                        .Append(category)
                                        .Append(" emote: ")
                                        .Append(String.Join(' ', results.Succeeded));
                                }

                                if (results.Failed.Any())
                                {
                                    builder
                                        .Append(" Failed to add ")
                                        .Append(category)
                                        .Append("emotes: ")
                                        .Append(String.Join(' ', results.Failed));
                                }

                                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, builder.ToString());
                            }
                        }
                        break;
                case "d":
                case "delete":
                case "remove":
                        {
                            if (next.TryGetFirst(out var categoryString, out var next2)
                                && categoryString.TryParseEnum(out EmoteCategory category))
                            {
                                var emotes = next2.Where(x => x.Length > 1);

                                var results = EmoteManager.RemoveAll(emotes, category, e.ChatMessage.Channel);

                                StringBuilder builder = new StringBuilder()
                                    .Append(e.ChatMessage.DisplayName)
                                    .Append(',');

                                if (results.Succeeded.Any())
                                {
                                    builder
                                        .Append(" Removed ")
                                        .Append(category)
                                        .Append("emotes: ")
                                        .Append(String.Join(' ', results.Succeeded));
                                }

                                if (results.Failed.Any())
                                {
                                    builder
                                        .Append(" Failed to remove ")
                                        .Append(category)
                                        .Append("emotes: ")
                                        .Append(String.Join(' ', results.Failed));
                                }

                                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, builder.ToString());
                            }
                        }
                        break;
                    default:
                        {
                            if (keyword.TryParseEnum(out EmoteCategory category) && category != EmoteCategory.Invalid)
                            {
                                var emotes = EmoteManager.Get(e.ChatMessage.Channel, category);

                                StringBuilder builder = new StringBuilder()
                                    .Append(e.ChatMessage.DisplayName)
                                    .Append(", ")
                                    .Append(category)
                                    .Append("emotes: ")
                                    .Append(String.Join(' ', emotes));
                            }
                        }
                        break;
                        

            }
        }
    }
}
