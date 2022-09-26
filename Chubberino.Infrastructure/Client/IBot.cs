using Chubberino.Infrastructure.Credentials;
using LanguageExt;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client;

public interface IBot : IDisposable
{
    BotState State { get; set; }

    /// <summary>
    /// Is the bot a broadcaster/moderator/VIP?
    /// </summary>
    Boolean IsModerator { get; set; }

    LoginCredentials LoginCredentials { get; set; }

    /// <summary>
    /// Start the bot, logging in as the specified user.
    /// </summary>
    /// <param name="clientOptions"></param>
    /// <param name="credentials">User to log in as. If null, will prompt for which user to log in as.</param>
    /// <returns>The login credentials of the user that successfully logged in; null otherwise.</returns>
    Option<LoginCredentials> Start(IClientOptions clientOptions = null, LoginCredentials credentials = null);

    String GetPrompt();

    void Refresh(IClientOptions clientOptions = null);

    void ReadCommand(String command);
}