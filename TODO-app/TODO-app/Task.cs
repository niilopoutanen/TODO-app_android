﻿using System;

namespace TODO_app
{
    internal class Task
    {
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;

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

        public Task()
        {
            creationTime = DateTime.Now;
        }
    }
}