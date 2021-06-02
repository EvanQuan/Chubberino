using Chubberino.Database.Models;

namespace Chubberino.Database
{
    public interface ICredentials
    {
        UserCredentials UserCredentials { get; }

        ApplicationCredentials ApplicationCredentials { get; }
    }
}
