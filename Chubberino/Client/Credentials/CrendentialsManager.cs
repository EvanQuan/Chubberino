using Chubberino.Database.Contexts;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Credentials
{
    public sealed class CrendentialsManager : ICredentialsManager
    {
        public IConsole Console { get; }
        public IApplicationContext Context { get; }

        public CrendentialsManager(IConsole console, IApplicationContext context)
        {
            Console = console;
            Context = context;
        }

        public Boolean TryGetCredentials(out LoginCredentials credentials)
        {
            credentials = null;

            var users = Context.UserCredentials.OrderBy(x => x.ID).ToArray();

            if (!users.Any())
            {
                credentials = null;
                return false;
            }

            Boolean resultIsValid = false;
            do
            {
                Console.WriteLine("Log in as:");
                for (Int32 i = 0; i < users.Length; i++)
                {
                    var user = users[i];
                    Console.WriteLine($"\t{i}. {user.TwitchUsername}{(user.IsBot ? " [Bot]" : "")}");
                }

                Int32 result = Int32.TryParse(Console.ReadLine(), out result) ? result : -1;

                resultIsValid = 0 <= result && result < users.Length;

                if (resultIsValid)
                {
                    var user = users[result];
                    credentials = new LoginCredentials(
                        new ConnectionCredentials(user.TwitchUsername, user.AccessToken),
                        user.IsBot);

                    return true;
                }
            }
            while (true);

        }

    }
}
