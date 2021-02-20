using System;

namespace Chubberino.Client
{
    public interface IConsole
    {
        void WriteLine(String message);

        void Write(String message);

        String ReadLine();
    }
}
