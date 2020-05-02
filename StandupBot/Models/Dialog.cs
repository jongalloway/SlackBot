using Newtonsoft.Json;

namespace StandupBotModels.Dialog
{
    public class Accessory
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public Text text { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }

    public class Block
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public Text text { get; set; }

        [JsonProperty("accessory")]
        public Accessory accessory { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("blocks")]
        public Block[] blocks { get; set; }
    }

    public class Text
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }
    }
}