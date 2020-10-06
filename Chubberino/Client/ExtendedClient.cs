using Chubberino.Client.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Exceptions;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    /// <summary>
    /// Queues messages, and sends them over timed intervals.
    /// </summary>
    public sealed class ExtendedClient : TwitchClient, IExtendedClient
    {
        private String PreviousMessage { get; set; }

        private IBot Bot { get; }

        private TextWriter Console { get; }

        private ISpinWait SpinWait { get; }

        public ExtendedClient(
            IBot bot,
            IClient client,
            ClientProtocol protocol,
            TextWriter console,
            ISpinWait spinWait,
            ILogger<TwitchClient> logger)
            : base(client, protocol, logger)
        {
            Bot = bot;
            Console = console;
            SpinWait = spinWait;
        }

        private void SendMessage(String message)
        {
            if (message == PreviousMessage)
            {
                message += " " + Data.InvisibleCharacter;
            }

            try
            {
                SendMessage(Bot.ChannelName, message);
                PreviousMessage = message;
            }
            catch (BadStateException e)
            {
                Console.WriteLine("ERROR: Failed to send message");
                Console.WriteLine(e.Message);
            }
        }

        public void SpoolMessage(String message)
        {
            SendMessage(message);
        }

        public Boolean EnsureJoinedToChannel(String channelName)
        {
            Boolean isConnected = SpinWait.SpinUntil(() =>
            {
                if (!IsConnected)
                {
                    Connect();
                    SpinWait.Sleep(TimeSpan.FromSeconds(1));
                    return IsConnected;
                }
                return true;

            },
            TimeSpan.FromSeconds(10));

            if (!isConnected) { return false; }

            Boolean isJoined = SpinWait.SpinUntil(() =>
            {
                JoinChannel(channelName);
                return JoinedChannels.Any(x => x.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase));

            },
            TimeSpan.FromSeconds(10));

            return isJoined;
        }
    }
}
