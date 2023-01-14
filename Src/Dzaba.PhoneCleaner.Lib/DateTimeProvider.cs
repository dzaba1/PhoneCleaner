using System;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface IDateTimeProvider
    {
        DateTime Now();
        DateTime UtcNow();
    }

    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
