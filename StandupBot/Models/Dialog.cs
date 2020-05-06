using Newtonsoft.Json;

namespace StandupBot.Models.Dialog
{
    public class Accessory
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public TextBlock text { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }

    public class Block
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public TextBlock text { get; set; }

        [JsonProperty("accessory")]
        public Accessory accessory { get; set; }
    }

    public class DialogRoot
    {
        [JsonProperty("blocks")]
        public Block[] blocks { get; set; }
    }
}