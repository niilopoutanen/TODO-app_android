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
    internal class Task
    {
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;

        public Task()
        {
            creationTime = DateTime.Now;
        }
        
        public DateTime CreationTime
        {
            get { return creationTime; }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}