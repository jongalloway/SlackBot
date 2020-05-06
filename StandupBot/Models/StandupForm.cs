using Newtonsoft.Json;

namespace StandupBot.Models.StandupForm
{
    public class Checkbox
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }

        [JsonProperty("initial_options")]
        public Options InitialOptions { get; set; }
    }

    public class Blocks
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("element")]
        public Element Element { get; set; }

        [JsonProperty("label")]
        public Placeholder Label { get; set; }
    }

    public class Element
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("action_id")]
        public string ActionId { get; set; }

        [JsonProperty("multiline")]
        public bool Multiline { get; set; }

        [JsonProperty("placeholder")]
        public Placeholder Placeholder { get; set; }
    }

    public class Element2
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("action_id")]
        public string ActionId { get; set; }

        [JsonProperty("placeholder")]
        public Placeholder Placeholder { get; set; }
    }

    public class Label
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Options
    {
        [JsonProperty("text")]
        public Title[] Text { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Placeholder
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("submit")]
        public Title Submit { get; set; }

        [JsonProperty("close")]
        public Title Close { get; set; }

        [JsonProperty("blocks")]
        public Blocks Blocks { get; set; }
    }

    public class Text
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string TextContent { get; set; }

        [JsonProperty("accessory")]
        public Checkbox Checkbox { get; set; }
    }

    public class Title
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("emoji")]
        public bool Emoji { get; set; }
    }
}