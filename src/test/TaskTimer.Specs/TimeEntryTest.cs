using System;
using NUnit.Framework;
using TaskTimer.Model;

namespace TaskTimer.Specs
{
    [TestFixture]
    public class TimeEntryTest
    {
        [Test]
        public void CreatedTimeEntryShouldHaveStartTimeInitialized()
        {
            var timeEntry = new TimeEntry();
            Assert.IsTrue(timeEntry.StartTime <= DateTime.Now);
            Assert.IsFalse(timeEntry.IsStopped);
        }

        [Test]
        public void StoppedTimeEntryShouldHaveStopTimeInitialized()
        {
            var timeEntry = new TimeEntry();
            Assert.AreEqual(DateTime.MinValue, timeEntry.StopTime);

            timeEntry.Stop();
            Assert.IsTrue(timeEntry.IsStopped);
            Assert.IsTrue(timeEntry.StopTime <= DateTime.Now);
        }
    }
}