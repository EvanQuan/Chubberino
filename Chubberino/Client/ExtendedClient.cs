using Chubberino.Client.Abstractions;
using Microsoft.Extensions.Logging;
using System;
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

        private BotInfo BotInfo { get; set; }

        public ExtendedClient(
            BotInfo botInfo,
            IClient client = null,
            ClientProtocol protocol = ClientProtocol.WebSocket,
            ILogger<TwitchClient> logger = null)
            : base(client, protocol, logger)
        {
            BotInfo = botInfo;
        }

        private void SendMessage(String message)
        {
            if (message == PreviousMessage)
            {
                message += " " + Data.InvisibleCharacter;
            }

            try
            {
                SendMessage(BotInfo.ChannelName, message);
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
    }
}
