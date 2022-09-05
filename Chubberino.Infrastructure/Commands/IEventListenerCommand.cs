using System;

namespace Chubberino.Infrastructure.Commands;

public interface IEventListenerCommand<TEventArgs> : IEventListener<TEventArgs>, ICommand
    where TEventArgs : EventArgs
{
}
