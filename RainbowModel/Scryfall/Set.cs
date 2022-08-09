using Newtonsoft.Json;

namespace RainbowModel.Scryfall
{
    public class Set
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("mtgo_code")]
        public string MtgoCode { get; set; }

        [JsonProperty("arena_code")]
        public string ArenaCode { get; set; }

        [JsonProperty("tcgplayer_id")]
        public int TcgplayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("scryfall_uri")]
        public string ScryfallUri { get; set; }

        [JsonProperty("search_uri")]
        public string SearchUri { get; set; }

        [JsonProperty("released_at")]
        public string ReleasedAt { get; set; }

        [JsonProperty("set_type")]
        public string SetType { get; set; }

        [JsonProperty("card_count")]
        public int CardCount { get; set; }

        [JsonProperty("printed_size")]
        public int PrintedSize { get; set; }

        [JsonProperty("digital")]
        public bool Digital { get; set; }

        [JsonProperty("nonfoil_only")]
        public bool NonfoilOnly { get; set; }

        [JsonProperty("foil_only")]
        public bool FoilOnly { get; set; }

        [JsonProperty("icon_svg_uri")]
        public string IconSvgUri { get; set; }
    }
}
