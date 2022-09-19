using Java.Util;
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
            taskList.OrderBy(x => x.dueDate).ToList();
            return taskList;
        }

        internal static List<TaskItem> SortListByCreationDate(List<TaskItem> taskList)
        {
            taskList.OrderBy(x => x.creationTime).ToList();
            return taskList;
        }

        internal static List<TaskItem> SortListByIsDone(List<TaskItem> taskList)
        {
            taskList.OrderBy(x => x.isDone).ToList();
            return taskList;
        }
    }
}