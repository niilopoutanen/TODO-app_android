using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using PCLStorage;
using Org.Json;
using Android.Util;
using System.Text.Json;
using System.Reflection;

namespace TODO_app
{
    internal class FileClass
    {
        //Folder location and filename
        
        private string _fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "TODO2.0.JSON");


        public FileClass()
        {
            //Checks if everything is all right
            CreateFile();
        }


        /// <summary>
        /// If file doesent exists it will create it
        /// </summary>
        private void CreateFile()
        {

            if (!File.Exists(_fileName))
            {
                File.Create(_fileName);
            }
        }

        /// <summary>
        /// Writes all text to internal storage. Needs List of Task objects.
        /// </summary>
        /// <param name="tasks"></param>
        internal void WriteFile(List<TaskItem> tasks)
        {
            //Empty file
            //File.WriteAllText(_fileName, "");

            //Convert objects to string
            foreach (TaskItem task in tasks)
            {
                string writeTask =JsonSerializer.Serialize(task);
                File.WriteAllText(_fileName, writeTask);
            }

            //Writes to file
            
        }


        /// <summary>
        /// Reads from internal storage. Returns list of Task objects.
        /// </summary>
        /// <returns></returns>
        internal List<TaskItem> ReadFile()
        {
            
            List<TaskItem> tasks = new List<TaskItem>();

            foreach (string line in System.IO.File.ReadLines(_fileName))
            {
                //Check that the line is not empty
                if (line != null && line != "")
                {
                    tasks.Add(JsonSerializer.Deserialize<TaskItem>(line));
                }
                
            }
            return tasks;
        }
    }
}