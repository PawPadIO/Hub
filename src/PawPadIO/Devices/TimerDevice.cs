using System;
using System.Collections.Generic;
using System.Text;

namespace PawPadIO.Devices
{
    public class TimerEvent : Device
    {
        public TimerEvent(string name, DateTimeOffset dateTime)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            Name = name;
            When = dateTime;
        }

        public string Name { get; }

        public DateTimeOffset When { get; }
    }

    public class AlarmEvent : Device
    {
        public AlarmEvent(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public string Name { get; }

        public DayOfWeek Days { get; set; } = DayOfWeek.All;

        public TimeSpan When { get; set; }

        public DateTime Expires { get; set; }

        public DateTime NextAlarm(DateTime date)
        {
            if (Days == DayOfWeek.None)
                return DateTime.MaxValue;

            if (Days.IsTodayValid(date) && date.TimeOfDay < When)
                return date.Date.Add(When);

            return Days.GetNextDay(date).Add(When);
        }
    }

    public class TimerEventArgs : EventArgs
    {
        public TimerEvent Timer { get; internal set; }
    }
    public class AlarmEventArgs : EventArgs
    {
        public AlarmEvent Alarm { get; internal set; }
    }
}
