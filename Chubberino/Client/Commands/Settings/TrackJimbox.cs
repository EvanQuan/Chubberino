using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

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
        private JimboxStage Stage { get; set; }


        private HashSet<String> Contributors { get; }

        /// <summary>
        /// Emote bordering the box.
        /// </summary>
        private String Border { get; set; }

        public TrackJimbox(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Contributors = new HashSet<String>();
            IsEnabled = true;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            String cleanMessage = e.ChatMessage.Message.Trim(' ', Data.InvisibleCharacter);

            String[] tokens = cleanMessage.Split(' ');

            if (tokens.Length != 4)
            {
                Stage = 0;
                return;
            }

            if (IsTop(tokens))
            {
                // Differentiate between top and bottom.
                if (Stage == JimboxStage.Mouth)
                {
                    // We are at the bottom and have completed the jimbox.
                    Stage = JimboxStage.Bottom;
                    Contributors.Add(e.ChatMessage.Username);
                    SpoolSuccessMessage();
                    Border = null;
                    Contributors.Clear();
                }
                else
                {
                    // We just started a new jimbox.
                    Stage = JimboxStage.Top;
                    Border = tokens[0];
                    Contributors.Add(e.ChatMessage.Username);
                }
            }
            else if (IsEyes(tokens))
            {
                if (Stage == JimboxStage.Top)
                {
                    Stage = JimboxStage.Eyes;
                    Contributors.Add(e.ChatMessage.Username);
                }
                else
                {
                    Stage = JimboxStage.None;
                    Border = null;
                    Contributors.Clear();
                }
            }
            else if (IsMouth(tokens))
            {
                if (Stage == JimboxStage.Eyes)
                {
                    Stage = JimboxStage.Mouth;
                    Contributors.Add(e.ChatMessage.Username);
                }
                else
                {
                    Stage = JimboxStage.None;
                    Border = null;
                    Contributors.Clear();
                }
            }
        }

        private void SpoolSuccessMessage()
        {
            if (Contributors.Count == 1)
            {
                Spooler.SpoolMessage($"@{Contributors.Single()} Congratz on making a {Border} jimbox. Great job! peepoClap");
            }
            else
            {
                Spooler.SpoolMessage($"@{String.Join(", @", Contributors)} Congratz on working together to make a {Border} jimbox. Hooray teamwork! peepoClap");
            }
        }

        private Boolean IsTop(String[] tokens)
        {
            return tokens.All(token => token == tokens[0]);
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
