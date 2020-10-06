using Autofac;
using System;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client.Abstractions
{
    public interface IBot : IDisposable
    {
        BotState State { get; }

        IExtendedClient TwitchClient { get; set; }

        ILifetimeScope Scope { get; set; }

        Boolean Start();

        String GetPrompt();

        void Refresh(IClientOptions clientOptions);

        void ReadCommand(String command);
    }
}