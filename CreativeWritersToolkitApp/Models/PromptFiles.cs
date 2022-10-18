using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Models
{
    public class PromptFiles
    {
        public List<Prompt> Prompts { get; private set; }

        /// <summary>
        /// Creates and loads the database of prompts based on the given folder(s).
        /// </summary>
        public PromptFiles(string path) 
        { 
            Prompts = LoadPrompts(path);
        }

        //TODO Load prompts - decide between this way or code behind.
        private List<Prompt> LoadPrompts(string path)
        {
            var prompts = new List<Prompt>();

            throw new NotImplementedException();
            //return prompts;
        }
    }
}
