using Avalonia.Controls;
using Avalonia.Input;
using CreativeWritersToolkitApp.Models;
using CreativeWritersToolkitApp.Services;
using System;
using System.IO;

namespace CreativeWritersToolkitApp.Views
{
    public partial class MainWindow : Window
    {
        bool isLicensed;
        PromptFiles promptDatabase;
        string localizedDataPathDirectory;
        bool isFictionActivated = false;
        bool developmentCopy = true;
        string version = "1.0.0.0";
        int numberOfTries;
        ApiFile apiFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            var fileName = "license.json";
            var path = Path.Combine(Environment.CurrentDirectory, @"LicenseFiles\", fileName);
            var promptFiles = Path.Combine(Environment.CurrentDirectory, @"Prompts\");
            bool isLicensed = LicenseCheck(path);
            if (isLicensed)
            {
                this.isLicensed = true;
            }
            //else
            //Messagebox show canceled dialog and close

            apiFile = new ApiFile();

            promptDatabase = new PromptFiles(promptFiles);
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

        private void OnRunCommand(object sender, PointerEventArgs e)
        {

        }
    }
}
