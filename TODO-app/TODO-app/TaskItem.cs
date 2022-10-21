using System;
using System.Collections.Generic;
using System.Linq;

namespace TODO_app
{
    internal class TaskItem
    {
        private DateTime _creationTime;
        private DateTime _dueDate;
        private string _text;
        private bool _isDone = false;
        private string _taskType;
        private int _amountNeeded;
        private int _amountDone = 0;

        /// <summary>
        /// The time when the object was created
        /// </summary>
        public DateTime CreationTime
        {
            get { return _creationTime; }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public bool IsDone
        {
            get { return _isDone; }
            set { _isDone = value; }
        }
        public string TaskType
        {
            get { return _taskType; }
            set { _taskType = value; }
        }
        public int AmountNeeded
        {
            get { return _amountNeeded; }
            set { _amountNeeded = value; }
        }
        public int AmountDone
        {
            get { return _amountDone; }
            set { _amountDone = value; }
        }

        public TaskItem(DateTime creationTime)
        {
            _creationTime = creationTime;
        }

        internal static List<TaskItem> SortListByDueDate(List<TaskItem> taskList)
        {
            var tasks = from t in taskList
                          orderby t._dueDate
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
            List<TaskItem> newOrder = new List<TaskItem>();
            foreach (TaskItem t in taskList)
            {
                if (t.IsDone == false)
                {
                    newOrder.Add(t);
                }
            }

            foreach (TaskItem t in taskList)
            {
                if (!newOrder.Contains(t))
                {
                    newOrder.Add(t);
                }
            }
            
            return newOrder;
        }
    }
}