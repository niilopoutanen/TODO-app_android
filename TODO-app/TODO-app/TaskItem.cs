using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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