using Chubberino.Client;
using System;

namespace Chubberino
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        // [STAThread]
        public static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            using var bot = new Bot();

            if (!bot.Start())
            {
                Console.WriteLine("Failed to join channel");
                return;
            }

            Boolean shouldContinue = true;
            do
            {
                Console.Write(bot.GetPrompt());
                bot.ReadCommand(Console.ReadLine());

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
                    // case BotState.ShouldRestart:
                        // TODO
                        // break;
                }
            }
            while (shouldContinue);
        }
    }
}
