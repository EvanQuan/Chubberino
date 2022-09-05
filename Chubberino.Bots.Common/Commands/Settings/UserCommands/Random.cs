using System;
using System.IO;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using RandomSource = System.Random;

namespace Chubberino.Bots.Common.Commands.Settings.UserCommands;

public sealed class Random : UserCommand
{
    public Random(ITwitchClientManager client, TextWriter writer, RandomSource random) : base(client, writer)
    {
        RandomSource = random;
    }

    public RandomSource RandomSource { get; }

    public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
    {
        switch (e.Words.Length)
        {
            case 0:
                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} {GetRandom(0, 100)}");
                break;
            case 1:
                TwitchClientManager.SpoolMessage(
                    e.ChatMessage.Channel,
                    $"{e.ChatMessage.DisplayName} You must include a minimum and maximum integer",
                    Priority.Low);
                break;
            default:
                if (!Int32.TryParse(e.Words[0], out Int32 min) || !Int32.TryParse(e.Words[1], out Int32 max))
                {
                    TwitchClientManager.SpoolMessage(
                        e.ChatMessage.Channel,
                        $"The minimum and maximum values must be integers",
                        Priority.Low);
                    break;
                }
                TwitchClientManager.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} {GetRandom(min, max)}");
                break;

        }
    }

    /// <summary>
    /// Gets a random <see cref="Int64"/> between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="min">Minimum value (inclusive)</param>
    /// <param name="max">Maximum value (inclusive)</param>
    public Int64 GetRandom(Int64 min, Int64 max)
    {
        return min + RandomSource.NextInt64() % (max + 1 - min);
    }

    public override String GetHelp()
    {
        return @"
Gets a random integer between a minimum and maximum.

usage: random <minimum> <maximum>

    <minimum> - the minimum value in the range (inclusive)
    <maximum> - the maximum value in the range (inclusive)
";
    }
}
