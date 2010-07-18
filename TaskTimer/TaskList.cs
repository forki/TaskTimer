using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TaskTimer
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
            Tasks[Tasks.IndexOf(new Task(taskName))].Start();
        }

        public void Stop(string taskName)
        {
            //TODO: quickhack, da gelöschter task nicht mehr gestoppt werden kann
            try
            {
                Tasks[Tasks.IndexOf(new Task(taskName))].Stop();
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }
        }
    }
}