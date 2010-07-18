using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTimer.Model
{
    internal class TaskList
    {
        public TaskList()
        {
            Tasks = new List<Task>();
        }

        public List<Task> Tasks { get; set; }

        public void AddTask(string taskName)
        {
            Tasks.Add(new Task(taskName));
        }

        public Task GetByName(string taskName)
        {
            return Tasks.FirstOrDefault(t => t.Name.Equals(taskName));
        }

        public bool Contains(string taskName)
        {
            return Tasks.Where(task => task.Name == taskName).Any();
        }

        public bool RemoveByName(string taskName)
        {
            return Remove(GetByName(taskName));
        }

        public bool Remove(Task task)
        {
            try
            {
                Tasks.Remove(task);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Start(string taskName)
        {
            var task = GetByName(taskName);
            if (task != null)
                task.Start();
        }

        public void Stop(string taskName)
        {
            var task = GetByName(taskName);
            if (task != null)
                task.Stop();
        }
    }
}