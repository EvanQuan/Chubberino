using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TwitchLib.Client.Events;
using WolframAlphaNet;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Wolfram : UserCommand
    {
        private WolframAlpha WolframAlpha { get; }

        private const String FirstSentencePattern = @"^.*?[.?!](?=\s+\p{P}*[\p{Lu}\p{N}]|\s*$)";

        private Regex FirstSentenceRegex { get; }

        public Wolfram(IExtendedClient client, TextWriter console, WolframAlpha wolfram) : base(client, console)
        {
            WolframAlpha = wolfram;
            FirstSentenceRegex = new Regex(FirstSentencePattern, RegexOptions.Compiled);
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out var words)) { return; }

            var request = String.Join(' ', words);

            var result = WolframAlpha.Query(request);

            if (result.Success)
            {
                var pod = result.Pods.FirstOrDefault(pod => pod.Title == "Result" || pod.Title == "Wikipedia summary");

                if (pod != null && pod.SubPods != null)
                {
                    StringBuilder messageBuilder = new StringBuilder();

                    messageBuilder.Append(e.ChatMessage.DisplayName);
                    messageBuilder.Append(" ");

                    if (pod.Title == "Result")
                    {
                        foreach (var subpod in pod.SubPods)
                        {
                            messageBuilder.Append(subpod.Plaintext);
                        }
                    }
                    else
                    {
                        StringBuilder wikipediaEntryBuilder = new StringBuilder();
                        foreach (var subpod in pod.SubPods)
                        {
                            wikipediaEntryBuilder.Append(subpod.Plaintext);
                            wikipediaEntryBuilder.Append(" ");
                        }

                        var match = FirstSentenceRegex.Match(wikipediaEntryBuilder.ToString());

                        if (match.Success)
                        {
                            messageBuilder.Append(match.Value);
                        }
                    }

                    TwitchClient.SpoolMessage(messageBuilder.ToString());
                }
            }
            else
            {
                TwitchClient.SpoolMessage(e.ChatMessage.DisplayName + " Could not query WolframAlpha successfully.");
            }
        }
    }
}
