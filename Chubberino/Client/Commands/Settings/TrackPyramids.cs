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
        private const Int32 MinimumRelevantPyramidHeight = 2;

        private HashSet<String> PyramidContributorUsernames { get; }

        public String PyramidBlock { get; set; }

        public Int32 CurrentPyramidHeight { get; set; } = 0;
        public Int32 MaximumPyramidHeight { get; set; } = 0;

        private Boolean BuildingUp { get; set; } = true;

        public TrackPyramids(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            PyramidContributorUsernames = new HashSet<String>();
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            IsEnabled = true;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            // Not matter what, we already check for the start of a new pyramid.
            if (TryGetFirstPyramidBlock(e.ChatMessage.Message, out String block))
            {
                // If this is end of the current pyramid
                if (block == PyramidBlock && MaximumPyramidHeight >= MinimumRelevantPyramidHeight && CurrentPyramidHeight == 2)
                {
                    PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    if (PyramidContributorUsernames.Count == 1)
                    {
                        Spooler.SpoolMessage($"@{e.ChatMessage.Username} Congratz on building a {MaximumPyramidHeight}-story tall {PyramidBlock} pyramid. Great job! peepoClap");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"@{String.Join(", @", PyramidContributorUsernames)} Congratz on working together to build a {MaximumPyramidHeight}-story tall {PyramidBlock} pyramid. Hooray teamwork! peepoClap");
                    }
                }

                // Successfully started a new pyramid
                PyramidBlock = block;
                CurrentPyramidHeight = 1;
                MaximumPyramidHeight = 1;
                PyramidContributorUsernames.Clear();
                PyramidContributorUsernames.Add(e.ChatMessage.Username);
            }
            else if (PyramidBlock != null)
            {
                // We already have a pyramid

                String[] tokens = e.ChatMessage.Message.Split(" ");

                if (tokens.All(elements => elements == PyramidBlock))
                {
                    if (BuildingUp && tokens.Length == CurrentPyramidHeight + 1)
                    {
                        // Continuing to build up
                        CurrentPyramidHeight++;
                        MaximumPyramidHeight++;
                        PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    }
                    else if (tokens.Length == CurrentPyramidHeight - 1)
                    {
                        // Switched to build down or continuing down
                        BuildingUp = false;
                        CurrentPyramidHeight--;
                        PyramidContributorUsernames.Add(e.ChatMessage.Username);
                    }
                    else
                    {
                        // Broke the pyramid in a way that does not start a new pyramid.
                        BuildingUp = true; // for the next pyramid
                        CurrentPyramidHeight = 0;
                        MaximumPyramidHeight = 0;
                        PyramidBlock = null;
                        PyramidContributorUsernames.Clear();
                    }
                }
                else
                {
                    // Still not a pyramid
                    BuildingUp = true;
                    CurrentPyramidHeight = 0;
                    MaximumPyramidHeight = 0;
                    PyramidBlock = null;
                    PyramidContributorUsernames.Clear();
                }
            }
        }

        private Boolean TryGetFirstPyramidBlock(String message, out String block)
        {
            block = message.Split(" ").FirstOrDefault();

            return message == block;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);
        }
    }
}
