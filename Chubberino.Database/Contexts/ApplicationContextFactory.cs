namespace Chubberino.Database.Contexts;

public sealed class ApplicationContextFactory : IApplicationContextFactory
{
    public IApplicationContext GetContext()
        => new ApplicationContext();
}
