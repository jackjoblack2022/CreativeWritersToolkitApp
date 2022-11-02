using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using CreativeWritersToolkitApp.Models;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Path = System.IO.Path;

namespace CreativeWritersToolkitApp.Views
{
    public partial class ApiKeyWindow : Window
    {
        public ApiFile? ApiFile { get; set; }
        public ApiKeyWindow()
        {
            InitializeComponent();
        }

        private void OnOpened(object sender, EventArgs args)
        {
            if (ApiFile.API != String.Empty)
                ApiTextBx.Text = ApiFile.API;
        }

        private void GetAccount_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://beta.openai.com/signup";
            OpenUrlInBrowser(url);
        }

        private void GetAPI_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://beta.openai.com/account/api-keys";
            OpenUrlInBrowser(url);
        }

        private void OpenUrlInBrowser(string url)
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

        private void SaveBtnClick(object sender, RoutedEventArgs args)
        {
            ApiFile.API = ApiTextBx.Text;
            if(ApiTextBx.Text == "Enter Your API Key Here" || ApiTextBx.Text == string.Empty)
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Warning", "You must enter your api key.", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);
                messageBox.Show();
                return;
            }

            ApiFile.API = ApiTextBx.Text;
            var path = Path.Combine(Environment.CurrentDirectory, @"Assets\ApiFile.txt");
            ApiFile.SaveAPI(path);
            this.Close();
        }
    }
}
