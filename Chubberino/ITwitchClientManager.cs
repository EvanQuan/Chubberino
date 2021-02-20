using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino
{
    public interface ITwitchClientManager
    {
        String PrimaryChannelName { get; set; }

        Boolean IsBot { get; }

        IExtendedClient Client { get; }

        void SpoolMessage(String message);

        Boolean TryInitialize(IBot bot, IClientOptions clientOptions = null, Boolean askForCredentials = true);

        Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null);
    }
}
