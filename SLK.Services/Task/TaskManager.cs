using System.Collections.Generic;

namespace SLK.Services.Task
{
    public static class TaskManager
    {
        private static List<TaskDescription> _tasksList = new List<TaskDescription>();

        public static void AddTask(TaskDescription task)
        {
            _tasksList.Add(task);
        }

        public static List<TaskDescription> GetTasks()
        {
            _tasksList.RemoveAll(t => t.Progress >= 100);
            return _tasksList;
        }

        public static int GetTasksCount()
        {
            _tasksList.RemoveAll(t => t.Progress >= 100);
            return _tasksList.Count;
        }
    }
}
