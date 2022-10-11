using System.IO;
using Chubberino.Bots.Channel.IoC;
using Chubberino.Infrastructure.Client;

namespace Chubberino.Bots.Channel;

public class Program
{
    private static TextWriter Writer { get; set; } = Console.Out;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    public static void Main()
    {
        var (bot, credentials) = DependencyManager.SetupIoC(null);

        Boolean shouldContinue = credentials != null;

        Boolean shouldGetPrompt = true;

        while (shouldContinue)
        {
            try
            {
                if (shouldGetPrompt)
                {
                    Writer.Write(bot.GetPrompt());
                    bot.ReadCommand(Console.ReadLine());
                }

                switch (bot.State)
                {
                    // Continue onto the next prompt
                    default:
                    case BotState.ShouldContinue:
                        break;
                    // End the program.
                    case BotState.ShouldStop:
                        shouldContinue = false;
                        break;
                }

                shouldGetPrompt = true;
            }
            catch (Exception ex)
            {
                Writer.WriteLine(ex.Message);
                shouldGetPrompt = false;
            }
        }
    }
}
