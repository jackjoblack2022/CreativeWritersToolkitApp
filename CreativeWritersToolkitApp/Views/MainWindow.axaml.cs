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
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Views
{
    public partial class MainWindow : Window
    {
        Prompt prompt;
        bool? isLicensed;
        PromptFiles? promptDatabase;
        bool isFictionActivated = true;
        bool developmentCopy = true;
        string version = "0.8.0.0";
        int? numberOfTries;
        ApiFile? apiFile;

        public MainWindow()
        {
            InitializeComponent();
            var promptFiles = Path.Combine(Environment.CurrentDirectory, @"Assets\Prompts\");
            promptDatabase = new PromptFiles();
            promptDatabase.SortPrompts();
            PopulateComboBoxes(promptDatabase);
            prompt = null;
            apiFile = new ApiFile();
            Init();
        }
        

        public void Init()
        {
            var fileName = "license.json";
            var path = Path.Combine(Environment.CurrentDirectory, @"LicenseFiles\", fileName);
            var promptFiles = Path.Combine(Environment.CurrentDirectory, @"Prompts\");
            var apiPath = Path.Combine(Environment.CurrentDirectory, @"Assets\ApiFile.txt");
            if (ApiCheck(apiPath) == false)
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Warning", "There is no API key. Please get your api key first.", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error, WindowStartupLocation.CenterOwner);
                messageBox.Show();
            }
            //bool isLicensed = LicenseCheck(path);
            //if (isLicensed)
            //{
            //    this.isLicensed = true;
            //}
            //else
            //Messagebox show canceled dialog and close
        }

        private bool ApiCheck(string path)
        {
            var result = string.Empty;
            if(apiFile != null)
            {
                if (apiFile.LoadAPI(path) == false)
                {
                    if (apiFile.API == String.Empty)
                    {
                        return false;
                    }
                    else return true;
                }
                return true;
            }
            return false;
        }

        private void OnNFCBSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            prompt = promptDatabase.NfPrompts.ElementAt(NfComboBox.SelectedIndex);
            PromptBox.Text = prompt.PromptText;
        }

        private void OnFCBSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            prompt = promptDatabase.FPrompts.ElementAt(FComboBox.SelectedIndex);
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
            ApiWindow.ApiFile = apiFile;
            ApiWindow.Show();
            
        }

        private async Task<string> BeginOpenAi()
        {
            var manager = new OpenAIManager(prompt);
            var result = await manager.Run(apiFile);
            return result;
        }

        private async void RunBtn_Click(object sender, RoutedEventArgs args)
        {
            if(prompt == null || apiFile.API == String.Empty)
            {
                if(apiFile.API == null || apiFile.API == String.Empty)
                {
                    var messageWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Warning", "You must enter your api key first!", MessageBox.Avalonia.Enums.ButtonEnum.Ok);
                    messageWindow.Show();
                    return;
                }
                if(prompt == null)
                {
                    var messageWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Warning", "There is no prompt selected. Please select a prompt first", MessageBox.Avalonia.Enums.ButtonEnum.Ok);
                    messageWindow.Show();
                    return;
                }
                
            }
            this.IsEnabled = false;
            prompt.PromptText = PromptBox.Text;
            var result = await BeginOpenAi();
            ResultBx.Text = result;
            this.IsEnabled = true;
        }
    }
}
