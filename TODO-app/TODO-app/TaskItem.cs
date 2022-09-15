using System;

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
    }
}