using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using PCLStorage;

namespace TODO_app
{
    internal class FileClass
    {
        IFolder folder = FileSystem.Current.LocalStorage;
        string fileName = "TODO1";

        public FileClass()
        {
            CheckIfFileExists();
            CreateFile();
        }


        /// <summary>
        ///Throws an error if no path cant be found
        /// </summary>
        private void CheckIfFileExists()
        {

            //Check if file location is empty and throw exception
            if (folder == null)
            {
                throw new InvalidOperationException("File Path cannot be found");
            }
        }

        private void CreateFile()
        {

            if (ExistenceCheckResult.FileExists != folder.CheckExistsAsync(fileName).Result)
            {
                folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }
        }
    }
}