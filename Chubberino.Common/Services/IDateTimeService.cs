namespace Chubberino.Common.Services;

public interface IDateTimeService
{
    public DateTime Now { get; }
    public DateTime MinValue { get; }
    public DateTime MaxValue { get; }
}
