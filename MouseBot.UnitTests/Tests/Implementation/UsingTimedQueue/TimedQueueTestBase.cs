using MouseBot.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MouseBot.UnitTests.Tests.Implementation.UsingTimedQueue
{
    public abstract class TimedQueueTestBase
    {
        protected TimedQueue Sut { get; }

        public TimedQueueTestBase()
        {
            Sut = new TimedQueue();
        }
    }
}
