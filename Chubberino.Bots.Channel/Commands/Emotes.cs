using System.Collections.Generic;
using System.IO;
using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;

namespace Chubberino.Bots.Channel.Commands;

public sealed class Emotes : UserCommand
{
    public IEmoteManager EmoteManager { get; }

    public Emotes(ITwitchClientManager client, TextWriter writer, IEmoteManager emoteManager)
        : base(client, writer)
    {
        EmoteManager = emoteManager;
    }

    public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
        => e.Words
            .TryGetFirstAndNext()
            .IfSome(value =>
            {
                // TODO add channel admins
                var username = e.ChatMessage.Username;

                var userIsChannelAdmin = username.Equals("chubbehmouse", StringComparison.OrdinalIgnoreCase)
                    || username.Equals(e.ChatMessage.Channel, StringComparison.OrdinalIgnoreCase);

                if (!userIsChannelAdmin)
                {
                    return;
                }

                var keyword = value.Element.ToLower();
                var next = value.Next;

                switch (keyword)
                {
                    case "a":
                    case "add":
                        next
                            .TryGetFirstAndNext()
                            .IfSome(AddEmote(e));
                        break;
                    case "d":
                    case "delete":
                    case "remove":
                        {
                            next
                                .TryGetFirstAndNext()
                                .IfSome(DeleteEmote(e));
                        }
                        break;
                    default:
                        keyword
                            .TryParseEnum<EmoteCategory>()
                            .IfSome(DisplayAllEmotesOfCategory(e));
                        break;
                }
            });

    private Action<EmoteCategory> DisplayAllEmotesOfCategory(OnUserCommandReceivedArgs e)
        => category =>
        {
            if (category != EmoteCategory.Invalid)
            {
                var emotes = EmoteManager.Get(e.ChatMessage.Channel, category);

                StringBuilder builder = new StringBuilder()
                    .Append(e.ChatMessage.DisplayName)
                    .Append(", ")
                    .Append(category)
                    .Append(" emotes: ")
                    .Append(String.Join(' ', emotes));

                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, builder.ToString());
            }
        };

    private Action<(String Element, IEnumerable<String> Next)> DeleteEmote(OnUserCommandReceivedArgs e)
        => value =>
        {
            var categoryString = value.Element;
            var next2 = value.Next;

            categoryString
                .TryParseEnum<EmoteCategory>()
                .IfSome(category =>
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
                            .Append(" emotes: ")
                            .Append(String.Join(' ', results.Succeeded));
                    }

                    if (results.Failed.Any())
                    {
                        builder
                            .Append(" Failed to remove ")
                            .Append(category)
                            .Append(" emotes: ")
                            .Append(String.Join(' ', results.Failed));
                    }

                    TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, builder.ToString());
                });
        };

    private Action<(String Element, IEnumerable<String> Next)> AddEmote(OnUserCommandReceivedArgs e)
        => value =>
        {
            var categoryString = value.Element;
            var next2 = value.Next;

            categoryString
                .TryParseEnum<EmoteCategory>()
                .IfSome(category =>
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
                            .Append(" emotes: ")
                            .Append(String.Join(' ', results.Succeeded));
                    }

                    if (results.Failed.Any())
                    {
                        builder
                            .Append(" Failed to add ")
                            .Append(category)
                            .Append(" emotes: ")
                            .Append(String.Join(' ', results.Failed));
                    }

                    TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, builder.ToString());
                });
        };
}
