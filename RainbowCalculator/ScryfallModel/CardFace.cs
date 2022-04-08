using System.Collections.Generic;
using Newtonsoft.Json;

namespace RainbowCalculator.Model
{
    public class CardFace
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mana_cost")]
        public string ManaCost { get; set; }

        [JsonProperty("type_line")]
        public string TypeLine { get; set; }

        [JsonProperty("oracle_text")]
        public string OracleText { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("power")]
        public string Power { get; set; }

        [JsonProperty("toughness")]
        public string Toughness { get; set; }

        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("watermark")]
        public string Watermark { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("artist_id")]
        public string ArtistId { get; set; }

        [JsonProperty("illustration_id")]
        public string IllustrationId { get; set; }

        [JsonProperty("image_uris")]
        public ImageUris ImageUris { get; set; }

        [JsonProperty("loyalty")]
        public string Loyalty { get; set; }
    }
}