using CreativeWritersToolkitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Services
{
    public class Licensing : ILicensing
    {
        private const string url = "https://creativewritingtoolkit.com/v1/api/license-check.php?key=";
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
    }
}
