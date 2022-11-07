using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Remote.Protocol.Input;
using CreativeWritersToolkitApp.Models;
using CreativeWritersToolkitApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Views
{
    public partial class MainWindow : Window
    {
        string promptFiles;
        Prompt prompt;
        bool? isLicensed;
        string licensePath;
        PromptFiles? promptDatabase;
        bool isFictionActivated = true;
        bool developmentCopy = true;
        string version = "1.0.1.0";
        int? numberOfTries;
        ApiFile? apiFile;
        string apiPath;
        Licensing licensing;

        public MainWindow()
        {
            InitializeComponent();
            promptFiles = Path.Combine(Environment.CurrentDirectory, @"Assets\Prompts\");
            promptDatabase = new PromptFiles();
            promptDatabase.SortPrompts();
            PopulateComboBoxes(promptDatabase);
            prompt = null;
            apiFile = new ApiFile();
            Init();
            CheckForUpdates();

        }
        
        private async void ImportBtn_Run(object sender, RoutedEventArgs args)
        {
            var importDialog = new OpenFileDialog();
            var filter = new FileDialogFilter();
            filter.Name = "Bplatt Files";
            filter.Extensions.Add("bplatt");
            importDialog.Filters.Add(filter);

            var importPath = await importDialog.ShowAsync(this);
            if(importPath == null)
            {
                return;
            }
            var uriPaths = importPath[0].Split(@"\");

            var promptName = uriPaths.Last();

            var targetPath = Path.Combine(promptFiles, promptName);
            File.Copy(importPath[0], targetPath, true);
            promptDatabase = new PromptFiles();
            promptDatabase.SortPrompts();
            PopulateComboBoxes(promptDatabase);

        }

        private async void CheckForUpdate(object sender, RoutedEventArgs e)
        {
            var liveVersion = LoadVersion();
            var url = "https://2022sfproducts.s3.amazonaws.com/TheCreativeWritersToolkit/The+Creative+Writer's+Toolkit.zip";
            if (liveVersion != String.Empty)
            {
                if (version != liveVersion)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("New Version?", "There is a new version available. Would you like to download it?", MessageBox.Avalonia.Enums.ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Info, WindowStartupLocation.CenterOwner);
                    var result = await messageBox.Show();
                    if (result == MessageBox.Avalonia.Enums.ButtonResult.Yes)
                    {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        using (Process process = Process.Start(new ProcessStartInfo
                        {
                            FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
                            Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"-e {url}" : "",
                            CreateNoWindow = true,
                            UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        })) ;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }
            }
        }

        private async Task CheckForUpdates()
        {
            var liveVersion = LoadVersion();
            var url = "https://2022sfproducts.s3.amazonaws.com/TheCreativeWritersToolkit/The+Creative+Writer's+Toolkit.zip";
            if(liveVersion != String.Empty)
            {
                if(version != liveVersion)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("New Version?", "There is a new version available. Would you like to download it?", MessageBox.Avalonia.Enums.ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Info, WindowStartupLocation.CenterOwner);
                    var result = await messageBox.Show();
                    if(result == MessageBox.Avalonia.Enums.ButtonResult.Yes)
                    {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        using (Process process = Process.Start(new ProcessStartInfo
                        {
                            FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
                            Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"-e {url}" : "",
                            CreateNoWindow = true,
                            UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        })) ;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }
            }
        }

        public static string LoadVersion()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://2022sfproducts.s3.amazonaws.com/TheCreativeWritersToolkit/version.txt");
            var response = client.GetAsync(client.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else return string.Empty;

        }

        public async Task Init()
        {
            RunBtn.IsEnabled = false;
            NfComboBox.IsEnabled = false;
            FComboBox.IsEnabled = false;
            NfPromptBtn.IsEnabled = false;
            FPromptBtn.IsEnabled = false;
            PromptBox.IsEnabled = false;
            ResultBx.IsEnabled = false;

            var licenseFile = "license.json";
            licensePath = Path.Combine(Environment.CurrentDirectory, @"LicenseFiles\", licenseFile);
            var promptFiles = Path.Combine(Environment.CurrentDirectory, @"Prompts\");
            apiPath = Path.Combine(Environment.CurrentDirectory, @"Assets\ApiFile.txt");
            licensing = new Licensing();
            licensing.OnSuccessfulLicensing += Licensing_OnSuccessfulLicensing;
            licensing.OnFailedLicensing += Licensing_OnFailedLicensing;
            licensing.CheckLicense(licensePath);
            
        }

        private void Licensing_OnFailedLicensing(object? sender, EventArgs e)
        {
            if (licensing.IsAppLicensed == false)
            {
                RunBtn.IsEnabled = false;
                NfComboBox.IsEnabled = false;
                FComboBox.IsEnabled = false;
                NfPromptBtn.IsEnabled = false;
                FPromptBtn.IsEnabled = false;
                PromptBox.IsEnabled = false;
                ResultBx.IsEnabled = false;
            }
        }

        private void Licensing_OnSuccessfulLicensing(object? sender, EventArgs e)
        {
            RunBtn.IsEnabled = true;
            NfComboBox.IsEnabled = true;
            NfPromptBtn.IsEnabled = true;
            PromptBox.IsEnabled = true;
            ResultBx.IsEnabled = true;

            if (licensing.IsFictionActived == false)
            {
                FPromptBtn.IsEnabled = false;
                FComboBox.IsEnabled = false;
            }
            else
            {
                FPromptBtn.IsEnabled = true;
                FComboBox.IsEnabled = true;
            }

            if (ApiCheck(apiPath) == false)
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Warning", "There is no API key. Please get your api key first.", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error, WindowStartupLocation.CenterOwner);
                messageBox.Show();
            }
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
            var nfPromptList = promptFiles.NfPrompts;
            var fPromptList = promptFiles.FPrompts;

            var nfIndex = nfPromptList.FindIndex(x => x.Name == "STEP 1. Please select a prompt file here.");
            var nfItem = nfPromptList[nfIndex];
            nfPromptList[nfIndex] = nfPromptList[0];
            nfPromptList[0] = nfItem;

            var fIndex = fPromptList.FindIndex(x => x.Name == "STEP 1. Please select a prompt file here.");
            var fItem = fPromptList[fIndex];
            fPromptList[fIndex] = fPromptList[0];
            fPromptList[0] = fItem;

            NfComboBox.Items = promptFiles.NfPrompts;
            FComboBox.Items = promptFiles.FPrompts;
            NfComboBox.SelectedIndex = 0;
            FComboBox.SelectedIndex = 0;
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

        private void CallRegisterLicenseWindow(object sender, RoutedEventArgs args)
        {
            var LicenseWindow = new LicenseWindow();
            LicenseWindow.Show();
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
