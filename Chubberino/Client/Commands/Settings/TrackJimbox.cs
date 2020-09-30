using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class TrackJimbox : Setting
    {
        /// <summary>
        /// Progression in making a Jimbox.
        /// </summary>
        private enum JimboxStage
        {
            /// <summary>
            /// Not in any Jimbox progression.
            /// </summary>
            None = 0,

            /// <summary>
            /// Top border, composed of 4 of the same emote.
            /// </summary>
            Top = 1,

            /// <summary>
            /// Border emotes surrounding eyes: yyj1 yyj2
            /// </summary>
            Eyes = 2,

            /// <summary>
            /// Borer emotes surrounding mouth: yyj3 yyj4
            /// </summary>
            Mouth = 3,

            /// <summary>
            /// Bottom border, composed of 4 border emotes.
            /// </summary>
            Bottom = 4
        }

        /// <summary>
        /// Current progression.
        /// </summary>
        private JimboxStage CurrentStage { get; set; }


        private HashSet<String> Contributors { get; }

        /// <summary>
        /// Emote bordering the box.
        /// </summary>
        private String Border { get; set; }

        public TrackJimbox(IExtendedClient client)
            : base(client)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };

            Contributors = new HashSet<String>();
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            String cleanMessage = e.ChatMessage.Message.Trim(' ', Data.InvisibleCharacter);

            String[] tokens = cleanMessage.Split(' ');

            if (tokens.Length < 4)
            {
                CurrentStage = JimboxStage.None;
                return;
            }

            if (IsTop(tokens))
            {
                // Differentiate between top and bottom.
                if (CurrentStage == JimboxStage.Mouth)
                {
                    // We are at the bottom and have completed the jimbox.
                    CurrentStage = JimboxStage.Bottom;
                    Contributors.Add(e.ChatMessage.DisplayName);
                    SpoolSuccessMessage();
                    Border = null;
                    Contributors.Clear();
                }
                else
                {
                    // We just started a new jimbox.
                    CurrentStage = JimboxStage.Top;
                    Border = tokens[0];
                    Contributors.Clear();
                    Contributors.Add(e.ChatMessage.DisplayName);
                }
            }
            else if (CurrentStage == JimboxStage.Top && IsEyes(tokens))
            {
                CurrentStage = JimboxStage.Eyes;
                Contributors.Add(e.ChatMessage.DisplayName);
            }
            else if (CurrentStage == JimboxStage.Eyes && IsMouth(tokens))
            {
                CurrentStage = JimboxStage.Mouth;
                Contributors.Add(e.ChatMessage.DisplayName);
            }
            else
            {
                CurrentStage = JimboxStage.None;
                Border = null;
                Contributors.Clear();
            }
        }

        private void SpoolSuccessMessage()
        {
            if (Contributors.Count == 1)
            {
                TwitchClient.SpoolMessage($"@{Contributors.Single()} Nice {Border} jimbox! peepoClap");
            }
            else
            {
                TwitchClient.SpoolMessage($"@{String.Join(", @", Contributors)} Nice {Border} jimbox! Hooray teamwork! peepoClap");
            }
        }

        private Boolean IsTop(String[] tokens)
        {
            return tokens[1] == tokens[0]
                && tokens[2] == tokens[0]
                && tokens[3] == tokens[0];
        }

        private Boolean IsEyes(String[] tokens)
        {
            return tokens[0] == Border
                && tokens[1] == "yyj1"
                && tokens[2] == "yyj2"
                && tokens[3] == Border;
        }

        private Boolean IsMouth(String[] tokens)
        {
            return tokens[0] == Border
                && tokens[1] == "yyj3"
                && tokens[2] == "yyj4"
                && tokens[3] == Border;
        }
    }
}
