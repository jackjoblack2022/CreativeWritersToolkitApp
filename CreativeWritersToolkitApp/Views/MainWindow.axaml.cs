using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CreativeWritersToolkitApp.Models;
using CreativeWritersToolkitApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CreativeWritersToolkitApp.Views
{
    public partial class MainWindow : Window
    {
        bool? isLicensed;
        PromptFiles? promptDatabase;
        bool isFictionActivated = true;
        bool developmentCopy = true;
        string version = "1.0.0.0";
        int? numberOfTries;
        ApiFile? apiFile;

        public MainWindow()
        {
            InitializeComponent();
            var promptFiles = Path.Combine(Environment.CurrentDirectory, @"Assets\Prompts\");
            promptDatabase = new PromptFiles();
            promptDatabase.SortPrompts();
            PopulateComboBoxes(promptDatabase);
            
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

            promptDatabase = new PromptFiles();
        }

        private void OnNFCBSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var prompt = promptDatabase.NfPrompts.ElementAt(NfComboBox.SelectedIndex);
            PromptBox.Text = prompt.PromptText;
        }

        private void OnFCBSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var prompt = promptDatabase.FPrompts.ElementAt(FComboBox.SelectedIndex);
            PromptBox.Text = prompt.PromptText;
        }

        private void PopulateComboBoxes(PromptFiles promptFiles)
        {
            NfComboBox.Items = promptFiles.NfPrompts.Select(x => x.Name);
            FComboBox.Items = promptFiles.FPrompts.Select(x => x.Name);
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

        private void NfPromptSwitch_Clicked(object sender, RoutedEventArgs args)
        {
            NfComboBox.IsVisible = true;
            FComboBox.IsVisible = false;
        }
        private void FPromptSwitch_Clicked(object sender, RoutedEventArgs args)
        {
            if(isFictionActivated == true)
            {
                NfComboBox.IsVisible = false;
                FComboBox.IsVisible = true;
            }
            
        }

        private void CallAPIWindow(object sender, RoutedEventArgs args)
        {
            var ApiWindow = new ApiKeyWindow();
            ApiWindow.Show();
        }

        private void RunBtn_Click(object sender, RoutedEventArgs args)
        {
            ResultBx.Text = "This works!";
            
            var mb = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Test", "This is a Test");
            mb.Show();
        }
    }
}
