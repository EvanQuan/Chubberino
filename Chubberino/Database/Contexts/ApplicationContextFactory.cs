namespace Chubberino.Database.Contexts
{
    public sealed class ApplicationContextFactory : IApplicationContextFactory
    {
        public IApplicationContext GetContext()
        {
            return new ApplicationContext();
        }
    }
}
