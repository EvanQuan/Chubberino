﻿using System.IO;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Infrastructure.Commands;

public abstract class Command : ICommand
{
    public const String NoHelpImplementedMessage = "No help implemented yet.";

    public Name Name { get; init; }

    protected ITwitchClientManager TwitchClientManager { get; private set; }
    protected TextWriter Writer { get; }

    protected Command(ITwitchClientManager client, TextWriter writer)
    {
        TwitchClientManager = client;
        Writer = writer;
        Name = GetType().Name;
        TwitchClientManager.OnTwitchClientRefreshed += TwitchClientManager_OnTwitchClientRefreshed;
    }

    private void TwitchClientManager_OnTwitchClientRefreshed(Object sender, OnTwitchClientRefreshedArgs e)
    {
        e.OldClient.IfSome(client => Unregister(client));
        Register(e.NewClient);
    }

    public abstract void Execute(IEnumerable<String> arguments);

    public virtual Boolean Set(String property, IEnumerable<String> arguments) => false;

    public virtual String Get(IEnumerable<String> arguments) => null;

    public virtual String GetHelp() => NoHelpImplementedMessage;

    public virtual Boolean Add(String property, IEnumerable<String> arguments) => false;

    public virtual Boolean Remove(String property, IEnumerable<String> arguments) => false;

    public virtual void Register(ITwitchClient client)
    {

    }

    public virtual void Unregister(ITwitchClient client)
    {

    }
}
