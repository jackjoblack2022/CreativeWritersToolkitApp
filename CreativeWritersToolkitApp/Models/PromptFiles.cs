using CreativeWritersToolkitApp.Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avalonia.Controls.Shapes;

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

        /// <summary>
        /// Gets the Prompt Files
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        
        private List<Prompt> LoadPrompts(string path)
        {
            //TODO Change this to check all directories and change .zip file to .BPlatt
            var promptFiles = Directory.GetFiles(path, "*.zip", SearchOption.AllDirectories).ToList();
            List<Prompt> result = new List<Prompt>();
            foreach (var prompt in promptFiles)
            {
                using (var archive = ZipFile.OpenRead(prompt))
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
            }
            
            return result;
        }
    }
}
