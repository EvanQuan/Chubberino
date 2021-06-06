using Chubberino.Database.Models;
using System;

namespace Chubberino.Client.Credentials
{
    public interface ICredentialsManager
    {
        UserCredentials UserCredentials { get; }

        ApplicationCredentials ApplicationCredentials { get; }

        /// <summary>
        /// Try to get the credentials of the user to log in as.
        /// </summary>
        /// <param name="credentials">Credentials found.</param>
        /// <returns>true if some credentials were found and returned.</returns>
        Boolean TryGetCredentials(out LoginCredentials credentials);
    }
}
