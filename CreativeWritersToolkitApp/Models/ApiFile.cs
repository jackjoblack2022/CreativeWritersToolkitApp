using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Models
{
    public class ApiFile
    {
        public string API { get; set; }

        public bool LoadAPI(string path)
        {
            if (File.Exists(path))
            {
                using(var stream = File.OpenText(path))
                {
                    API = stream.ReadToEnd();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveAPI(string path)
        {
            if (File.Exists(path))
            {
                File.AppendAllText(path, API);
                var messageBox = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Saved", "API Key Saved!", MessageBox.Avalonia.Enums.ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Info, Avalonia.Controls.WindowStartupLocation.CenterOwner);
                messageBox.Show();
                return;
            }
        }
    }
}
