using CreativeWritersToolkitApp.Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CreativeWritersToolkitApp.Models
{
    public class PromptFiles
    {
        public List<Prompt> Prompts { get; private set; }

        /// <summary>
        /// Creates and loads the database of prompts based on the given folder(s).
        /// </summary>
        public PromptFiles(string file) 
        { 
            Prompts = LoadPrompts(file);
        }

        private List<Prompt> LoadPrompts(string file)
        {
            List<Prompt> result = new List<Prompt>();
            using (var archive = ZipFile.OpenRead(file))
            {
                foreach (var json in archive.Entries)
                {
                    if (json.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var sr = new StreamReader(json.Open()))
                        {
                            var serializer = new JsonSerializer();
                            var jsonPrompt = (Prompt)serializer.Deserialize(sr, typeof(Prompt));
                            result.Add(jsonPrompt);
                        }
                    }
                }
            }
            return result;
        }
    }
}
