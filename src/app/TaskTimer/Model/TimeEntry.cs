using System;

namespace TaskTimer.Model
{
    public class TimeEntry
    {
        public TimeEntry()
        {
            StartTime = DateTime.Now;
        }

        public DateTime StartTime { get; private set; }
        public DateTime StopTime { get; private set; }

        public long DurationInTicks
        {
            get
            {
                if (IsStopped)
                    return StopTime.Ticks - StartTime.Ticks;
                return DateTime.Now.Ticks - StartTime.Ticks;
            }
        }

        public bool IsStopped
        {
            get { return StopTime != DateTime.MinValue; }
        }

        public long DurationInSeconds
        {
            get
            {
                var durationInTicks = DurationInTicks;
                var seconds = durationInTicks/TimeSpan.TicksPerSecond;
                if (durationInTicks%TimeSpan.TicksPerSecond > 0)
                    seconds++;
                return seconds;
            }
        }

        public void Stop()
        {
            StopTime = DateTime.Now;
        }
    }
}