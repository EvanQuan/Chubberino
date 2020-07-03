using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Join : Command
    {
        private String JoinedChannelName { get; set; } = TwitchInfo.InitialChannelName;

        public Join(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        }

        private void TwitchClient_OnJoinedChannel(Object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            // For simplicity, we can only be in 1 channel at a time.
            if (!e.Channel.Equals(JoinedChannelName, StringComparison.OrdinalIgnoreCase))
            {
                TwitchClient.LeaveChannel(JoinedChannelName);
                Console.WriteLine($"Left channel {JoinedChannelName}");
            }
            JoinedChannelName = e.Channel;
            Spooler.SetChannel(JoinedChannelName);
            Console.WriteLine($"Joined channel {JoinedChannelName}");
        }

        public override void Execute(params String[] arguments)
        {
            if (arguments.Length < 1) { return; }

            
            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }
            TwitchClient.JoinChannel(arguments[0]);
        }
    }
}
