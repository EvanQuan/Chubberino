using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class TrackPyramids : Setting
    {
        /// <summary>
        /// Minimum height for a pyramid to be recognized.
        /// </summary>
        private const Int32 MinimumRelevantPyramidHeight = 3;

        private HashSet<String> PyramidContributorUsernames { get; }

        public String PyramidBlock { get; set; }

        public Int32 CurrentPyramidHeight { get; set; } = 0;
        public Int32 TallestPyramidHeight { get; set; } = 0;

        private Boolean BuildingUp { get; set; } = true;

        public TrackPyramids(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            PyramidContributorUsernames = new HashSet<String>();
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            // Not matter what, we already check for the start of a new pyramid.
            if (TryGetFirstPyramidBlock(e.ChatMessage.Message, out String block))
            {
                // If this is end of the current pyramid
                if (block == PyramidBlock && TallestPyramidHeight >= MinimumRelevantPyramidHeight && CurrentPyramidHeight == 2)
                {
                    PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    SpoolPyramidSuccessMessage();
                }
                else
                {
                    // If a previous pyramid was broken by starting a new pyramid
                    SpoolPyramidFailedMessage(e.ChatMessage.Username);
                }

                // Successfully started a new pyramid
                PyramidBlock = block;
                CurrentPyramidHeight = 1;
                TallestPyramidHeight = 1;
                PyramidContributorUsernames.Clear();
                PyramidContributorUsernames.Add(e.ChatMessage.Username);
            }
            else if (PyramidBlock != null)
            {
                // We already have a pyramid

                String[] tokens = e.ChatMessage.Message.Split(" ");

                IEnumerable<String> uncleanTokens = tokens.Where(element => element == PyramidBlock || String.IsNullOrWhiteSpace(element) || element[0] == Data.InvisibleCharacter);

                Int32 uncleanTokensCount = uncleanTokens.Count();

                if (uncleanTokens.Count() == tokens.Length)
                {
                    Int32 cleanTokensCount = uncleanTokens.Where(element => element == PyramidBlock).Count();

                    if (BuildingUp && cleanTokensCount == CurrentPyramidHeight + 1)
                    {
                        // Continuing to build up
                        CurrentPyramidHeight++;
                        TallestPyramidHeight++;
                        PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    }
                    else if (cleanTokensCount == CurrentPyramidHeight - 1)
                    {
                        // Switched to build down or continuing down
                        BuildingUp = false;
                        CurrentPyramidHeight--;
                        PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    }
                    else
                    {
                        // Broke the pyramid in a way that does not start a new pyramid.
                        SpoolPyramidFailedMessage(e.ChatMessage.Username);
                        BuildingUp = true; // for the next pyramid
                        CurrentPyramidHeight = 0;
                        TallestPyramidHeight = 0;
                        PyramidBlock = null;
                        PyramidContributorUsernames.Clear();
                    }
                }
                else
                {
                    // Still not a pyramid
                    BuildingUp = true;
                    CurrentPyramidHeight = 0;
                    TallestPyramidHeight = 0;
                    PyramidBlock = null;
                    PyramidContributorUsernames.Clear();
                }
            }
        }

        private Boolean TryGetFirstPyramidBlock(String message, out String block)
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
            if (PyramidContributorUsernames.Count == 1)
            {
                Spooler.SpoolMessage($"@{PyramidContributorUsernames.Single()} Nice {TallestPyramidHeight}-story tall {PyramidBlock} pyramid! peepoClap");
            }
            else
            {
                Spooler.SpoolMessage($"@{String.Join(", @", PyramidContributorUsernames)} Nice {TallestPyramidHeight}-story tall {PyramidBlock} pyramid! Hooray teamwork! peepoClap");
            }
        }

        /// <summary>
        /// Notify when a pyramid has been broken.
        /// </summary>
        private void SpoolPyramidFailedMessage(String userThatBrokePyramid)
        {
            // Only send message on relavant pyramids to avoid false positives.
            if (TallestPyramidHeight < MinimumRelevantPyramidHeight) { return; }

            if (PyramidContributorUsernames.Contains(userThatBrokePyramid))
            {
                if (PyramidContributorUsernames.Count == 1)
                {
                    // User broke their own pyramid.
                    Spooler.SpoolMessage($"@{userThatBrokePyramid} You just ruined your own {TallestPyramidHeight}-story tall {PyramidBlock} pyramid. Sadge");
                }
                else
                {
                    // User broke co-operative pyramid.
                    Spooler.SpoolMessage($"@{userThatBrokePyramid} Imagine working with @{String.Join(", @", PyramidContributorUsernames.Where(name => !name.Equals(userThatBrokePyramid, StringComparison.OrdinalIgnoreCase)))} to build a {TallestPyramidHeight}-story tall {PyramidBlock} pyramid and then ruining it. 4WeirdW");
                }
            }
            else
            {
                // User broke someone else's pyramid.
                Spooler.SpoolMessage($"@{userThatBrokePyramid} Imagine ruining a {TallestPyramidHeight}-story {PyramidBlock} tall pyramid built by @{String.Join(", @", PyramidContributorUsernames)}. Oh wait, you just did. PogO");

            }
        }

        public override String GetHelp()
        {
            return @"
";
        }
    }
}
