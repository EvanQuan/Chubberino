namespace Chubberino.Infrastructure.Commands;

public interface IEventListener<TEventArgs>
    where TEventArgs : EventArgs
{
    void Invoke(Object sender, TEventArgs e);
}
