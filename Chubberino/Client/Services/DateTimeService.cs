using System;

namespace Chubberino.Client.Services
{
    public sealed class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
