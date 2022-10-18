using Avalonia.Controls;
using CreativeWritersToolkitApp.Services;
using System;
using System.IO;

namespace CreativeWritersToolkitApp.Views
{
    public partial class MainWindow : Window
    {
        public bool isLicensed;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            var fileName = "license.json";
            var path = Path.Combine(Environment.CurrentDirectory, @"LicenseFiles\", fileName);
            bool isLicensed = LicenseCheck(path);
            if (isLicensed)
            {
                this.isLicensed = true;
            }
            //else
            //Messagebox show canceled dialog and close

            bool result = LoadPrompts();
            if (result)
            {
                //Do stuff
            }
            //else
                //show message box
        }

        //HACK This is a crummy hack because I'm lazy and tired. Don't code like this kids, it's bad form.
        public static string key;
        public static string email;

        /// <summary>
        /// Checks if a license file is valid. If file has not been updated with proper information, requests it from user, checks info, then builds and saves new file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool LicenseCheck(string path)
        {
            var licensing = new Licensing();
            var loadedLicenseFile = licensing.LicenseFile(path);
            if(loadedLicenseFile.Key == "0" || loadedLicenseFile.Email == "0")
            {
                //TODO show license box
                return licensing.IsActive(key, email);
            }
            else
            {
                return licensing.IsActive(loadedLicenseFile);
            }
        }

        //TODO Load prompts
        private bool LoadPrompts()
        {
            throw new NotImplementedException();
        }
    }
}
