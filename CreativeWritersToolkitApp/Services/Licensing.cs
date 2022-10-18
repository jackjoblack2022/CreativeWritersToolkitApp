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

namespace CreativeWritersToolkitApp.Services
{
    public class Licensing : ILicensing
    {
        private const string url = "https://creativewritingtoolkit.com/v1/api/license-check.php?key=";
        /// <summary>
        /// Runs at the open and checks if software is authorized. 
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public bool IsActive(License license)
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
                        return true;
                }
                return false;
            }
            return false;
            
        }
        /// <summary>
        /// This method is if a license file doesn't exist, isn't loaded properly, or first run.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsActive(string key, string email)
        {
            var baseAddress = new Uri(url + key + "&email=" + email);
            var client = new HttpClient();
            var response = client.GetAsync(baseAddress.AbsoluteUri).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                string[] validation = responseContent.Split('|');
                foreach (var item in validation)
                {
                    if (item == "valid")
                        return true;
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
