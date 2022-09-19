using System;
using System.Collections.Generic;
using System.Linq;

namespace TODO_app
{
    internal class TaskItem
    {
        private DateTime creationTime;
        private DateTime dueDate;
        private string text;
        private bool isDone = false;

        public DateTime CreationTime
        {
            get { return creationTime; }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        public TaskItem()
        {
            creationTime = DateTime.Now;
        }

        internal static List<TaskItem> SortListByDueDate(List<TaskItem> taskList)
        {
            var tasks = from t in taskList
                          orderby t.dueDate
                          select t;
            return tasks.ToList();
        }

        internal static List<TaskItem> SortListByCreationDate(List<TaskItem> taskList)
        {
            var tasks = from t in taskList
                        orderby t.CreationTime
                        select t;
            return tasks.ToList();

        }

        internal static List<TaskItem> SortListByIsDone(List<TaskItem> taskList)
        {
            taskList.OrderBy(x => x.isDone).ToList();
            return taskList;
        }
    }
}