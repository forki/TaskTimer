using System;

namespace TaskTimer.Model
{
    internal class TimeEntry
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }

        public long DurationInTicks()
        {
            if (Stop.Year != 1)
                return Stop.Ticks - Start.Ticks;
            return DateTime.Now.Ticks - Start.Ticks;
        }

        public long DurationInSeconds()
        {
            var endTime = Stop.Year != 1 ? Stop.Ticks : DateTime.Now.Ticks;
            return CalcSeconds(endTime);
        }

        private long CalcSeconds(long endTime)
        {
            var seconds = (endTime - Start.Ticks)/TimeSpan.TicksPerSecond;
            if ((endTime - Start.Ticks)%TimeSpan.TicksPerSecond > 0)
                seconds++;
            return seconds;
        }
    }
}