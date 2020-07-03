using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseBot
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

            bot.Start();

            do
            {
                Console.Write($"[{bot.ChannelName}] > ");
                bot.ReadCommand(Console.ReadLine());
            }
            while (bot.ShouldContinue);
        }
    }
}
