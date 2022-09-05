using System;
using System.IO;
using System.Linq;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Microsoft.EntityFrameworkCore;
using Monad;
using TwitchLib.Client.Models;

namespace Chubberino.Infrastructure.Credentials;

public sealed class CrendentialsManager : ICredentialsManager
{
    public TextReader Reader { get; }

    public TextWriter Writer { get; }

    public IApplicationContextFactory ContextFactory { get; }

    public UserCredentials UserCredentials => LazyUserCredentials.Value;

    public ApplicationCredentials ApplicationCredentials => LazyApplicationCredentials.Value;

    public OptionResult<LoginCredentials> LoginCredentials => loginCredentials.ToOption();

    private Lazy<ApplicationCredentials> LazyApplicationCredentials { get; }

    private Lazy<UserCredentials> LazyUserCredentials { get; }

    private LoginCredentials loginCredentials;

    private Lazy<Name> PrimaryChannelName { get; }

    public CrendentialsManager(TextWriter writer, TextReader reader, IApplicationContextFactory contextFactory)
    {
        Writer = writer;
        Reader = reader;
        ContextFactory = contextFactory;


        LazyUserCredentials = new Lazy<UserCredentials>(() =>
        {
            using var context = ContextFactory.GetContext();
            return context.UserCredentials.AsNoTracking().First();
        });


        LazyApplicationCredentials = new Lazy<ApplicationCredentials>(() =>
        {
            using var context = contextFactory.GetContext();
            return context.ApplicationCredentials.AsNoTracking().First();
        });

        PrimaryChannelName = new(() =>
        {
            return Name.From(ApplicationCredentials.InitialTwitchPrimaryChannelName);
        });
    }

    public Boolean TryUpdateLoginCredentials(LoginCredentials oldCredentials, out LoginCredentials newCredentials)
    {
        if (oldCredentials is not null)
        {
            newCredentials = oldCredentials;
            return true;
        }

        using var context = ContextFactory.GetContext();

        var users = context.UserCredentials.OrderBy(x => x.ID);

        if (!users.Any())
        {
            loginCredentials = null;
            newCredentials = null;
            return false;
        }

        loginCredentials = PromptLogIn(users.ToArray());
        newCredentials = loginCredentials;
        return true;
    }

    /// <summary>
    /// Prompt user to select a user to login as.
    /// </summary>
    /// <param name="users">Users to chose from.</param>
    /// <returns>The credentials of the chosen user.</returns>
    private LoginCredentials PromptLogIn(UserCredentials[] users)
    {
        do
        {
            Writer.WriteLine("Log in as:");
            for (Int32 i = 0; i < users.Length; i++)
            {
                var user = users[i];
                Writer.WriteLine($"\t{i}. {user.TwitchUsername}{(user.IsBot ? " [Bot]" : "")}");
            }

            Int32 result = Int32.TryParse(Reader.ReadLine(), out result) ? result : -1;

            Boolean resultIsValid = 0 <= result && result < users.Length;

            if (resultIsValid)
            {
                var user = users[result];
                return new LoginCredentials(
                    new ConnectionCredentials(user.TwitchUsername, user.AccessToken),
                    user.IsBot,
                    PrimaryChannelName.Value);
            }
        }
        while (true);
    }
}
