using System;
using System.Collections.Generic;
using System.Text;

namespace PawPadIO
{
    [Flags]
    public enum DayOfWeek
    {
        None = 0,

        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,

        Weekday = Monday | Tuesday | Wednesday | Thursday | Friday,
        Weekend = Saturday | Sunday,
        All = Weekday | Weekend,
    }

    public static class DayOfWeekExtensions
    {
        public static bool IsTodayValid(this DayOfWeek days)
            => IsTodayValid(days, DateTime.Now);

        public static bool IsTodayValid(this DayOfWeek days, DateTime date)
        {
            var day = (int)date.DayOfWeek;
            return days.HasFlag((DayOfWeek)(1 << day));
        }

        public static DateTime GetPreviousDay(this DayOfWeek days)
            => GetPreviousDay(days, DateTime.Now);

        public static DateTime GetPreviousDay(this DayOfWeek days, DateTime date)
        {
            if (days == DayOfWeek.None)
                return DateTime.MinValue;

            var day = (int)date.DayOfWeek;
            var daysToAdd = 0;
            while (daysToAdd <= 7)
            {
                day = (day - 1) % 7;
                if (day < 0)
                    day += 7;
                daysToAdd++;

                if (days.HasFlag((DayOfWeek)(1 << day)))
                    return date.Date.AddDays(-daysToAdd);
            }

            throw new OverflowException("Could not identify previous day of week");
        }

        public static DateTime GetNextDay(this DayOfWeek days)
            => GetNextDay(days, DateTime.Now);

        public static DateTime GetNextDay(this DayOfWeek days, DateTime date)
        {
            if (days == DayOfWeek.None)
                return DateTime.MaxValue;

            var day = (int)date.DayOfWeek;
            var daysToAdd = 0;
            while(daysToAdd <= 7)
            {
                day = (day+ 1) % 7;
                daysToAdd++;

                if (days.HasFlag((DayOfWeek)(1 << day)))
                    return date.Date.AddDays(daysToAdd);
            }

            throw new OverflowException("Could not identify next day of week");
        }
    }
}
