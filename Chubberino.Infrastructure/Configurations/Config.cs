using System;
using System.Collections.Generic;
using System.Linq;
using Chubberino.Database.Contexts;

namespace Chubberino.Infrastructure.Configurations;

public sealed class Config : IConfig
{
    public Config(IApplicationContextFactory contextFactory)
    {
        using var context = contextFactory.GetContext();

        StartupChannelDisplayNames = context.StartupChannels.Select(x => x.DisplayName).ToArray();
    }

    public IEnumerable<String> StartupChannelDisplayNames { get; }
}
