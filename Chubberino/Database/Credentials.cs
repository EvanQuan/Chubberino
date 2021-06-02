using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chubberino.Database
{
    public sealed class Credentials
    {
        public Credentials(IApplicationContextFactory applicationContextFactory)
        {
            using var context = applicationContextFactory.GetContext();

            UserCredentials = context.UserCredentials.AsNoTracking().First();

            ApplicationCredentials = context.ApplicationCredentials.AsNoTracking().First();
        }

        public UserCredentials UserCredentials { get; }

        public ApplicationCredentials ApplicationCredentials { get; }
    }
}
