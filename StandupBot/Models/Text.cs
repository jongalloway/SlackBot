using System;
using Newtonsoft.Json;

namespace StandupBot.Models
{
    public class TextBlock
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }
    }
}
