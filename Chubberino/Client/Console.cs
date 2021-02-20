using System;

namespace Chubberino.Client
{
    public sealed class Console : IConsole
    {
        public String ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void Write(String message)
        {
            System.Console.Write(message);
        }

        public void WriteLine(String message)
        {
            System.Console.WriteLine(message);
        }
    }
}
