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
            Assert.IsTrue(task.StartTime >= DateTime.Now);
        }
    }
}