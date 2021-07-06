using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using WolframAlphaNet;

namespace Chubberino.Bots.Channel.Commands
{
    public sealed class Wolfram : UserCommand
    {
        private WolframAlpha WolframAlpha { get; }

        private const String FirstSentencePattern = @"^.*?[.?!](?=\s+\p{P}*[\p{Lu}\p{N}]|\s*$)";

        private Regex FirstSentenceRegex { get; }

        public Wolfram(ITwitchClientManager client, TextWriter console, WolframAlpha wolfram) : base(client, console)
        {
            WolframAlpha = wolfram;
            FirstSentenceRegex = new Regex(FirstSentencePattern, RegexOptions.Compiled);
        }

        public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
        {
            var request = String.Join(' ', e.Words);

            var result = WolframAlpha.Query(request);

            if (result.Success)
            {
                var pod = result.Pods.FirstOrDefault(pod => pod.Title == "Result" || pod.Title == "Wikipedia summary");

                if (pod != null && pod.SubPods != null)
                {
                    StringBuilder messageBuilder = new();

                    messageBuilder.Append(e.ChatMessage.DisplayName);
                    messageBuilder.Append(' ');

                    if (pod.Title == "Result")
                    {
                        foreach (var subpod in pod.SubPods)
                        {
                            messageBuilder.Append(subpod.Plaintext);
                        }
                    }
                    else
                    {
                        StringBuilder wikipediaEntryBuilder = new();
                        foreach (var subpod in pod.SubPods)
                        {
                            wikipediaEntryBuilder.Append(subpod.Plaintext);
                            wikipediaEntryBuilder.Append(' ');
                        }

                        var match = FirstSentenceRegex.Match(wikipediaEntryBuilder.ToString());

                        if (match.Success)
                        {
                            messageBuilder.Append(match.Value);
                        }
                    }

                    TwitchClientManager.SpoolMessage(messageBuilder.ToString());
                }
            }
            else
            {
                TwitchClientManager.SpoolMessage(e.ChatMessage.DisplayName + " Could not query WolframAlpha successfully.");
            }
        }
    }
}
