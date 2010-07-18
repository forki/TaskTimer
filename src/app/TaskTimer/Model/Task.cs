using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTimer.Model
{
    public class Task : IComparable
    {
        public Task(string taskName)
        {
            Name = taskName;
            TimeEntries = new List<TimeEntry>();
        }

        public List<TimeEntry> TimeEntries { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; private set; }

        public long TimeDifference { get; private set; }

        public long TimeDifferenceInMinutes
        {
            get
            {
                var minutes = TimeDifference/60;
                if (TimeDifference%60 > 0)
                    minutes++;
                return minutes;
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return 0;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (Task) && Equals((Task) obj);
        }

        public void Start()
        {
            TimeEntries.Add(new TimeEntry());
            IsActive = true;
        }

        public void Stop()
        {
            TimeEntries.Last().Stop();
            IsActive = false;
        }

        public long DurationInSeconds()
        {
            var seconds = TimeEntries.Sum(timeEntry => timeEntry.DurationInSeconds);
            return Math.Max(seconds + TimeDifference, 0);
        }

        public long DurationInMinutes()
        {
            var seconds = DurationInSeconds();
            var minutes = seconds/60;
            if (seconds%60 > 0)
                minutes++;
            return minutes;
        }

        public long DurationInSeconds(DateTime date)
        {
            var seconds =
                TimeEntries
                    .Where(timeEntry => timeEntry.StartTime.Date == date.Date || timeEntry.StopTime.Date == date.Date)
                    .Sum(timeEntry => timeEntry.DurationInSeconds);
            return Math.Max(seconds + TimeDifference, 0);
        }

        public long DurationInMinutes(DateTime date)
        {
            var seconds = DurationInSeconds(date);
            var minutes = seconds/60;
            if (seconds%60 > 0)
                minutes++;
            return minutes;
        }

        public void AddSeconds(long seconds)
        {
            //TODO
            //if (DurationInSeconds()+_seconds<0
            TimeDifference += seconds;
        }

        public void AddMinutes(long minutes)
        {
            AddSeconds(minutes*60);
        }

        public bool Equals(Task other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}