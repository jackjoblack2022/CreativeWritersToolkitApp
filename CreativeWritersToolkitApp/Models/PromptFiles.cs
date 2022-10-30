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
using Path = System.IO.Path;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia;

namespace CreativeWritersToolkitApp.Models
{
    public class PromptFiles
    {
        public List<Prompt> NfPrompts { get; private set; }
        public List<Prompt> FPrompts { get; private set; }

        /// <summary>
        /// Creates and loads the database of prompts based on the given folder(s).
        /// </summary>
        public PromptFiles() 
        { 
            NfPrompts = new List<Prompt>();
            FPrompts = new List<Prompt>();
        }

        /// <summary>
        /// Gets the Prompt Files
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        
        private IEnumerable<string> TestGetFiles(string path, params string[] exts)
        {
            var extensions = exts
                .Select(x => x.StartsWith(".") ? x : "." + x)
                .ToArray();
            return Directory.EnumerateFiles(path)
                .Where(fn => extensions.Contains(Path.GetExtension(fn), StringComparer.InvariantCultureIgnoreCase));
        }

        public bool SortPrompts()
        {
            // Option 1: The designer does not have access to the given path. So we exit here in design mode
            //if (Design.IsDesignMode) 
            //    return true;


            // Option 2: Use the assetLoader
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var path = Path.Combine(Environment.CurrentDirectory, @"Assets\Prompts");
            var bplattFiles = assets.GetAssets(new Uri("avares://CreativeWritersToolkitApp/Assets/Prompts/"),null)
                .Where(file => file.AbsolutePath.EndsWith(".bplatt", StringComparison.OrdinalIgnoreCase));
            if (bplattFiles.Count() == 0)
                return false;

            foreach(var file in bplattFiles)
            {
                var promptList = LoadPrompts(file.AbsolutePath);
                foreach(var prompt in promptList)
                {
                    if (prompt.Category == "nonfiction")
                        NfPrompts.Add(prompt);
                    if(prompt.Category == "fiction")
                        FPrompts.Add(prompt);
                }
            }
            NfPrompts.OrderBy(x => x.Name);
            FPrompts.OrderBy(x => x.Name);
            return true;
        }

        private List<Prompt> LoadPrompts(string file)
        {
            List<Prompt> result = new List<Prompt>();

            return result;

            // Your task :-)
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
