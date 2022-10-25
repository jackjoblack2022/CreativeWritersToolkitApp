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
        public string API { get; private set; }

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
    }
}
