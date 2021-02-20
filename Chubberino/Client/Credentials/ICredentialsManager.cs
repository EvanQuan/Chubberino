using System;

namespace Chubberino.Client.Credentials
{
    public interface ICredentialsManager
    {
        Boolean TryGetCredentials(out Credentials credentials);
    }
}
