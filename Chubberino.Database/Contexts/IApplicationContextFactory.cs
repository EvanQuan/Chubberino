namespace Chubberino.Database.Contexts
{
    public interface IApplicationContextFactory
    {
        IApplicationContext GetContext();
    }
}
