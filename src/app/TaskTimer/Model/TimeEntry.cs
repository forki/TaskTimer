using System;

namespace TaskTimer.Model
{
    public class TimeEntry
    {
        public DateTime StartTime { get; private set; }
        public DateTime StopTime { get; private set; }

        public TimeEntry()
        {
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            StartTime= DateTime.Now;
        }

        public long DurationInTicks()
        {
            if (StopTime.Year != 1)
                return StopTime.Ticks - StartTime.Ticks;
            return DateTime.Now.Ticks - StartTime.Ticks;
        }

        public long DurationInSeconds()
        {
            var endTime = StopTime.Year != 1 ? StopTime.Ticks : DateTime.Now.Ticks;
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