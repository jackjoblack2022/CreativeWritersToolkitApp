using CreativeWritersToolkitApp.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using License = CreativeWritersToolkitApp.Models.License;
using System.Threading;
using CreativeWritersToolkitApp.Views;

namespace CreativeWritersToolkitApp.Services
{
    public class Licensing
    {
        public event EventHandler OnSuccessfulLicensing;
        public event EventHandler OnFailedLicensing;

        public License? License { get; set; }
        public bool? IsAppLicensed { get; set; }
        public bool IsFictionActived { get; set; }
        public bool OnLicenseWindowCanceled { get; private set; }
        private LicenseWindow LicenseWindow { get; set; }
        private string LicensePath { get; set; }

        private const string url = "https://creativewritingtoolkit.com/v1/api/license-check.php?key=";

        public bool CheckLicense(string LicensePath)
        {
            this.LicensePath = LicensePath;
            if (File.Exists(LicensePath))
            {
                if (LicenseCheck(LicensePath))
                {
                    OnSuccessfulLicensing?.Invoke(this, new EventArgs());
                    return true;
                }
                else
                {
                    OnFailedLicensing?.Invoke(this, new EventArgs());
                    return false;
                }
                
            }
            else
            {
                License = CreateLicenseFile(LicensePath);
                if (LicenseCheck(LicensePath))
                {
                    OnSuccessfulLicensing?.Invoke(this, new EventArgs());
                    return true;
                }
                else return false;
            }
        }

        private bool LicenseCheck(string LicensePath)
        {
            License = LicenseFile(LicensePath);
            if (GetActivatedStatus(License) == false)
            {
                IsAppLicensed = false;
                LicenseWindow = new LicenseWindow();
                LicenseWindow.License = License;
                LicenseWindow.Register += LicenseWindow_Register;
                LicenseWindow.Canceled += LicenseWindow_Canceled;
                LicenseWindow.Show();
                return false;
            }
            else
            { 
                IsAppLicensed = true;
                return true;
            }
        }

        private void LicenseWindow_Canceled(object? sender, Events.LicenseWindowCancelArgs e)
        {

            IsAppLicensed = false;
            MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Warning", "You cannot use this software without registering!", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error, Avalonia.Controls.WindowStartupLocation.CenterOwner);
            LicenseWindow.Canceled -= LicenseWindow_Canceled;
            LicenseWindow.Register -= LicenseWindow_Register;
            LicenseWindow.Close();
            OnFailedLicensing?.Invoke(this, new EventArgs());
            
        }

        private void LicenseWindow_Register(object? sender, EventArgs e)
        {
            if (GetActivatedStatus(License))
            {
                IsAppLicensed = true;
                CreateLicenseFile(LicensePath, License.Key, License.Email);
                var messageBox = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Registered!", "The software is activated and ready to use!", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Success, Avalonia.Controls.WindowStartupLocation.CenterOwner);
                messageBox.Show();
                LicenseWindow.Canceled -= LicenseWindow_Canceled;
                LicenseWindow.Register -= LicenseWindow_Register;
                LicenseWindow.Close();
                OnSuccessfulLicensing?.Invoke(this, new EventArgs());
            }
            else
            {
                var messagebox = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Warning", "The email and key you entered were incorrect. Please try again.", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error);
                messagebox.Show();
            }
        }

        private License CreateLicenseFile(string licensePath)
        {
            License license = new License()
            {
                Email = "0",
                Key = "0",
            };
            var seralizer = new JsonSerializer();
            seralizer.NullValueHandling = NullValueHandling.Ignore;
            
            using(var sw = new StreamWriter(licensePath))
            using(var writer = new JsonTextWriter(sw))
            {
                seralizer.Serialize(writer, license);
                return license;
            }
        }

        private License CreateLicenseFile(string licensePath, string key, string email)
        {
            License license = new License()
            {
                Email = email,
                Key = key,
            };
            var seralizer = new JsonSerializer();
            seralizer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(licensePath))
            using (var writer = new JsonTextWriter(sw))
            {
                seralizer.Serialize(writer, license);
                return license;
            }
        }

        /// <summary>
        /// Runs at the open and checks if software is authorized. 
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public bool GetActivatedStatus(License license)
        {
            var baseAddress = new Uri(url + license.Key + "&email=" + license.Email);
            var client = new HttpClient();
            var response = client.GetAsync(baseAddress.AbsoluteUri).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                string[] validation = responseContent.Split('|');
                foreach (var item in validation)
                {
                    if (item == "valid")
                    {
                        IsAppLicensed = true;
                    }
                    if (item == "Fiction Module")
                    {
                        IsFictionActived = true;
                        return true;
                    }
                        
                }
                return false;
            }
            return false;
            
        }
        /// <summary>
        /// Loads a license file from a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public License LicenseFile(string path)
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    var seralizer = new JsonSerializer();
                    License license = (License)seralizer.Deserialize(file, typeof(License));
                    if (license != null)
                        return license;
                    else return new License();
                }
            }
            //TODO catch exceptions in license file
            catch (Exception)
            {
                throw;
            }
            
        
        }
    }
}
