using Chubberino.Database.Models;
using Monad;
using System;

namespace Chubberino.Infrastructure.Credentials;

public interface ICredentialsManager
{
    UserCredentials UserCredentials { get; }

    ApplicationCredentials ApplicationCredentials { get; }

    /// <summary>
    /// The login credentials if the user has already logged in.
    /// </summary>
    OptionResult<LoginCredentials> LoginCredentials { get; }

    /// <summary>
    /// Try to get the credentials of the user to log in as. Updates <see cref="LoginCredentials"/>
    /// </summary>
    /// <returns>true if some credentials were found and returned.</returns>
    Boolean TryUpdateLoginCredentials(LoginCredentials oldCredentials, out LoginCredentials newCredentials);
}
