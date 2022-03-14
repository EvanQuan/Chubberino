using System;

namespace Chubberino.Common.Services
{
    public sealed class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public DateTime MinValue => DateTime.MinValue;

        public DateTime MaxValue => DateTime.MaxValue;
    }
}
