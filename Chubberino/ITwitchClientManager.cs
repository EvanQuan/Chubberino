using Chubberino.Client;
using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino
{
    public interface ITwitchClientManager
    {
        String PrimaryChannelName { get; set; }

        Boolean IsBot { get; }

        ITwitchClient Client { get; }

        Boolean TryInitialize(IBot bot, IClientOptions clientOptions = null, Boolean askForCredentials = true);

        Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null);

        void SpoolMessage(String channelName, String message, Priority priority = Priority.Medium);

        Boolean EnsureJoinedToChannel(String channel);
    }
}
