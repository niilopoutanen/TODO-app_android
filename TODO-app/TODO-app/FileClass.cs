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

namespace TODO_app
{
    internal class FileClass
    {
        //Folder location and filename
        private static IFolder _folder = FileSystem.Current.LocalStorage;
        private string _fileName = Path.Combine(_folder.ToString(),"TODO2.0.json");

        public FileClass()
        {
            //Checks if everything is all right
            CreateFile();
            CheckIfFileExists();
        }


        /// <summary>
        ///Throws an error if no path cant be found
        /// </summary>
        private void CheckIfFileExists()
        {

            //Check if file location is empty and throw exception
            if (_folder == null)
            {
                throw new InvalidOperationException("File Path cannot be found");
            }
        }

        /// <summary>
        /// If file doesent exists it will create it
        /// </summary>
        private void CreateFile()
        {

            if (ExistenceCheckResult.FileExists != _folder.CheckExistsAsync(_fileName).Result)
            {
                _folder.CreateFileAsync(_fileName, CreationCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// Writes all text to internal storage. Needs List of Task objects.
        /// </summary>
        /// <param name="tasks"></param>
        private void WriteFile(List<Task> tasks)
        {
            //Empty file
            File.WriteAllText(_fileName, "");

            //Convert objects to string
            List<string> writeTasks = new List<string>();
            foreach (Task task in tasks)
            {
               writeTasks.Add(JsonSerializer.Serialize<Task>(task));
            }

            //Writes to file
            File.WriteAllLines(_fileName,writeTasks);
        }


        /// <summary>
        /// Reads from internal storage. Returns list of Task objects.
        /// </summary>
        /// <returns></returns>
        private List<Task> ReadFile()
        {
            List<Task> tasks = new List<Task>();
            string json = File.ReadAllText(_fileName);
            tasks.Add(JsonSerializer.Deserialize<Task>(json));
            return tasks;
        }
    }
}