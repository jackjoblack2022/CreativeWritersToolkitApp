using Newtonsoft.Json;

namespace CreativeWritersToolkitApp.Models
{
    public class Prompt
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        //PEX = Uneditable part of the prompt file
        public string PEX { get; set; }
        
        [JsonProperty("prompt")]
        public string PromptText { get; set; }
        public string SubCategory { get; set; }
        public int NumberOfUse { get; set; }
        public bool IsFavorite { get; set; }
        public float Temperature  { get; set; }
        public int Max_Tokens { get; set; }
        public float Top_P { get; set; }
        public float Frequency_Penalty { get; set; }
        public float Presence_Penalty { get; set; }
        [JsonProperty("stop")]
        public string[]? Stop { get; set; }

        public int AffiliateLicense { get; set; }
    }
}
