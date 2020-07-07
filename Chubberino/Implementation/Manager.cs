using Chubberino.Implementation.Abstractions;
using System;
using System.Threading.Tasks;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Implementation
{
    public abstract class Manager : IManager
    {
        private Boolean disposedValue;

        public Boolean IsRunning { get; private set; }

        protected ITwitchClient TwitchClient { get; private set; }

        protected Manager(ITwitchClient client)
        {
            TwitchClient = client;
        }

        public void Start()
        {
            if (IsRunning) { return; }

            if (TwitchClient.IsConnected)
            {
                IsRunning = true;
                Task.Run(Manage);
            }
            else
            {
                throw new ClientNotConnectedException($"Cannot start until client is connected and has joined a channel");
            }
        }

        private void Manage()
        {
            while (IsRunning)
            {
                ManageTasks();
            }
        }

        protected abstract void ManageTasks();

        protected virtual void Dispose(Boolean disposing)
        {
            if (!disposedValue)
            {
                IsRunning = false;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
