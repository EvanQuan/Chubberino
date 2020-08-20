using Chubberino.Client.Abstractions;
using System;
using System.Threading;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client
{
    public abstract class Manager : IManager
    {
        private Boolean disposedValue;

        public Boolean IsRunning { get; private set; }

        protected ITwitchClient TwitchClient { get; private set; }
        public TimeSpan Interval { get; set; }
        public Action Task { get; set; }

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
                System.Threading.Tasks.Task.Run(Manage);
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
                Thread.Sleep(Interval);
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
