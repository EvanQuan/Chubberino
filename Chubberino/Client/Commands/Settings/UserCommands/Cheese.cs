using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Cheese : UserCommand
    {
        public Cheese(IExtendedClient client, TextWriter console, IGame game) : base(client, console)
        {
            Game = game;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out IEnumerable<String> words)) { return; }

            if (!words.Any())
            {
                Game.AddPoints(e.ChatMessage);
            }

        }

        public IGame Game { get; }
    }
}
