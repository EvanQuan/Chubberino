namespace Chubberino.Infrastructure.Configurations;

public interface IConfig
{
    IEnumerable<String> StartupChannelDisplayNames { get; }
}