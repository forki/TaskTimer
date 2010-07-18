using System;

namespace TaskTimer.Model
{
    public class TimeEntry
    {
        public DateTime StartTime { get; private set; }
        public DateTime Stop { get; set; }

        public TimeEntry()
        {
            StartTime = DateTime.Now;
        }

        public long DurationInTicks()
        {
            if (Stop.Year != 1)
                return Stop.Ticks - StartTime.Ticks;
            return DateTime.Now.Ticks - StartTime.Ticks;
        }

        public long DurationInSeconds()
        {
            var endTime = Stop.Year != 1 ? Stop.Ticks : DateTime.Now.Ticks;
            return CalcSeconds(endTime);
        }

        private long CalcSeconds(long endTime)
        {
            var seconds = (endTime - StartTime.Ticks)/TimeSpan.TicksPerSecond;
            if ((endTime - StartTime.Ticks)%TimeSpan.TicksPerSecond > 0)
                seconds++;
            return seconds;
        }
    }
}