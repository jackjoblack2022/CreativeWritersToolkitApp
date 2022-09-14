using Newtonsoft.Json;

namespace CreativeWritersToolkitApp.Models
{
    public class Prompt
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string PEX { get; set; }
        
        [JsonProperty("prompt")]
        public string PromptText { get; set; }
        public int Temperature  { get; set; }
        public int Max_Tokens { get; set; }
        public float Top_P { get; set; }
        public float Frequency_Penalty { get; set; }
        public float Presence_Penalty { get; set; }

    }
}
