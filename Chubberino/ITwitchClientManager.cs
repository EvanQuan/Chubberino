using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino
{
    public interface ITwitchClientManager
    {
        IExtendedClient Client { get; }

        String PrimaryChannelName { get; set; }

        void SpoolMessage(String message);

        Boolean TryInitializeClient(IBot bot, IClientOptions clientOptions = null, Boolean askForCredentials = true);

        Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null);
    }
}
