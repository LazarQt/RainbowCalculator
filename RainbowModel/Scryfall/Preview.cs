using Newtonsoft.Json;

namespace RainbowModel.Scryfall
{
    public class Preview
    {
        [JsonProperty("source")] public string Source { get; set; }

        [JsonProperty("source_uri")] public string SourceUri { get; set; }

        [JsonProperty("previewed_at")] public string PreviewedAt { get; set; }
    }
}