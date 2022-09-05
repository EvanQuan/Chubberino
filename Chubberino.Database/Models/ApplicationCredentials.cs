using System;

namespace Chubberino.Database.Models;

public sealed class ApplicationCredentials
{
    public Int32 ID { get; set; }

    /// <summary>
    /// Twitch username of initial channel to join on program startup.
    /// </summary>
    public String InitialTwitchPrimaryChannelName { get; set; }

    /// <summary>
    /// Client application ID to access Twitch API.
    /// </summary>
    /// <remarks>
    /// Learn more: https://dev.twitch.tv/docs/authentication#registration
    /// </remarks>
    public String TwitchAPIClientID { get; set; }

    /// <summary>
    /// Wolfram Alpha application ID to access Wolfram Alpha API.
    /// </summary>
    public String WolframAlphaAppID { get; set; }
}
