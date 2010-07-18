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
            var task = new TimeEntry();
            Assert.IsTrue(task.StartTime <= DateTime.Now);
        }

        [Test]
        public void StoppedTimeEntryShouldHaveStopTimeInitialized()
        {
            var task = new TimeEntry();
            Assert.AreEqual(DateTime.MinValue, task.StopTime);

            task.Stop();
            Assert.IsTrue(task.StopTime <= DateTime.Now);
        }
    }
}