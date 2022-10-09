using System.IO;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using TwitchLib.Client.Models;

namespace Chubberino.Infrastructure.Credentials;

public sealed class CrendentialsManager : ICredentialsManager
{
    public TextReader Reader { get; }

    public TextWriter Writer { get; }

    public IApplicationContextFactory ContextFactory { get; }

    public UserCredentials UserCredentials => LazyUserCredentials.Value;

    public ApplicationCredentials ApplicationCredentials => LazyApplicationCredentials.Value;

    public Option<LoginCredentials> LoginCredentials => loginCredentials.ToSome();

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

    public Option<LoginCredentials> TryUpdateLoginCredentials(LoginCredentials oldCredentials = null)
    {
        if (oldCredentials is not null)
        {
            return oldCredentials;
        }

        using var context = ContextFactory.GetContext();

        var users = context.UserCredentials.OrderBy(x => x.ID);

        if (!users.Any())
        {
            loginCredentials = null;
            return Option<LoginCredentials>.None;
        }

        loginCredentials = PromptLogIn(users.ToArray());
        return loginCredentials;
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
