using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Bots.Common.Commands.Settings.Pyramids;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings
{
    public sealed class TrackPyramids : Setting
    {
        /// <summary>
        /// Minimum height for a pyramid to be recognized.
        /// </summary>
        private const Int32 MinimumRelevantPyramidHeight = 3;

        public PyramidTracker Pyramid { get; }

        public TrackPyramids(ITwitchClientManager client, TextWriter writer)
            : base(client, writer)
        {
            Pyramid = new PyramidTracker();
        }

        public override void Register(ITwitchClient client)
        {
            client.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        public override void Unregister(ITwitchClient client)
        {
            client.OnMessageReceived -= TwitchClient_OnMessageReceived;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            // Not matter what, we already check for the start of a new pyramid.
            if (TryGetFirstPyramidBlock(e.ChatMessage.Message, out String block))
            {
                // If this is end of the current pyramid
                if (block == Pyramid.Block && Pyramid.TallestHeight >= MinimumRelevantPyramidHeight && Pyramid.CurrentHeight == 2)
                {
                    Pyramid.ContributorDisplayNames.Add(e.ChatMessage.DisplayName);
                    SpoolPyramidSuccessMessage();
                }
                else
                {
                    // If a previous pyramid was broken by starting a new pyramid
                    SpoolPyramidFailedMessage(e.ChatMessage.DisplayName);
                }

                // Successfully started a new pyramid
                Pyramid.Start(e.ChatMessage.DisplayName, block);
            }
            else if (Pyramid.HasStarted)
            {
                // We already have a pyramid

                String[] tokens = e.ChatMessage.Message.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                IEnumerable<String> uncleanTokens = tokens.Where(element => element == Pyramid.Block || element[0] == Data.InvisibleCharacter);

                Int32 uncleanTokensCount = uncleanTokens.Count();

                if (uncleanTokens.Count() == tokens.Length)
                {
                    Int32 cleanTokensCount = uncleanTokens.Where(element => element == Pyramid.Block).Count();

                    if (Pyramid.BuildingUp && cleanTokensCount == Pyramid.CurrentHeight + 1)
                    {
                        // Continuing to build up
                        Pyramid.BuildUp(e.ChatMessage.DisplayName);
                    }
                    else if (cleanTokensCount == Pyramid.CurrentHeight - 1)
                    {
                        // Switched to build down or continuing down
                        Pyramid.BuildDown(e.ChatMessage.DisplayName);
                    }
                    else
                    {
                        // Broke the pyramid in a way that does not start a new pyramid.
                        Pyramid.Reset();
                    }
                }
                else
                {
                    // Still not a pyramid
                    Pyramid.Reset();
                }
            }
        }


        private static Boolean TryGetFirstPyramidBlock(String message, out String block)
        {
            block = message.Split(" ").FirstOrDefault();

            return message.Trim(' ', Data.InvisibleCharacter) == block;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);
        }

        private void SpoolPyramidSuccessMessage()
        {
            if (Pyramid.ContributorDisplayNames.Count == 1)
            {
                TwitchClientManager.SpoolMessage($"@{Pyramid.ContributorDisplayNames.Single()} Nice {Pyramid.TallestHeight}-story tall {Pyramid.Block} pyramid! peepoClap");
            }
            else
            {
                TwitchClientManager.SpoolMessage($"@{String.Join(", @", Pyramid.ContributorDisplayNames)} Nice {Pyramid.TallestHeight}-story tall {Pyramid.Block} pyramid! Hooray teamwork! peepoClap");
            }
        }

        /// <summary>
        /// Notify when a pyramid has been broken.
        /// </summary>
        private void SpoolPyramidFailedMessage(String userThatBrokePyramid)
        {
            // Only send message on relavant pyramids to avoid false positives.
            if (Pyramid.TallestHeight < MinimumRelevantPyramidHeight) { return; }

            if (Pyramid.ContributorDisplayNames.Contains(userThatBrokePyramid))
            {
                if (Pyramid.ContributorDisplayNames.Count == 1)
                {
                    // User broke their own pyramid.
                    TwitchClientManager.SpoolMessage($"@{userThatBrokePyramid} You just ruined your own {Pyramid.TallestHeight}-story tall {Pyramid.Block} pyramid. Sadge");
                }
                else
                {
                    // User broke co-operative pyramid.
                    TwitchClientManager.SpoolMessage($"@{userThatBrokePyramid} Imagine working with @{String.Join(", @", Pyramid.ContributorDisplayNames.Where(name => !name.Equals(userThatBrokePyramid, StringComparison.OrdinalIgnoreCase)))} to build a {Pyramid.TallestHeight}-story tall {Pyramid.Block} pyramid and then ruining it. 4WeirdW");
                }
            }
            else
            {
                // User broke someone else's pyramid.
                TwitchClientManager.SpoolMessage($"@{userThatBrokePyramid} Nice job ruining the {Pyramid.TallestHeight}-story {Pyramid.Block} tall pyramid built by @{String.Join(", @", Pyramid.ContributorDisplayNames)}. PogO");

            }
        }

        public override String GetHelp()
        {
            return @"
";
        }
    }
}
