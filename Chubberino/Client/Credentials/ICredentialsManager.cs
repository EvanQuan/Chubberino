using System;

namespace Chubberino.Client.Credentials
{
    public interface ICredentialsManager
    {
        /// <summary>
        /// Try to get the credentials of the user to log in as.
        /// </summary>
        /// <param name="credentials">Credentials found.</param>
        /// <returns>true if some credentials were found and returned.</returns>
        Boolean TryGetCredentials(out LoginCredentials credentials);
    }
}
