using Chubberino.Client.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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

        private IConsole Console { get; }

        private ISpinWait SpinWait { get; }

        public ExtendedClient(
            IClient client,
            ClientProtocol protocol,
            IConsole console,
            ISpinWait spinWait,
            ILogger<TwitchClient> logger)
            : base(client, protocol, logger)
        {
            Console = console;
            SpinWait = spinWait;
        }

        public void SpoolMessage(String channelName, String message)
        {
            if (message == PreviousMessage)
            {
                message += " " + Data.InvisibleCharacter;
            }

            try
            {
                SendMessage(channelName, message);
                PreviousMessage = message;
            }
            catch (BadStateException e)
            {
                Console.WriteLine("ERROR: Failed to send message");
                Console.WriteLine(e.Message);
            }
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
