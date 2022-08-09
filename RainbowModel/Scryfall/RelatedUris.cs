using Newtonsoft.Json;

namespace RainbowModel.Scryfall
{
    public class RelatedUris
    {
        [JsonProperty("gatherer")]
        public string Gatherer { get; set; }

        [JsonProperty("tcgplayer_infinite_articles")]
        public string TcgplayerInfiniteArticles { get; set; }

        [JsonProperty("tcgplayer_infinite_decks")]
        public string TcgplayerInfiniteDecks { get; set; }

        [JsonProperty("edhrec")]
        public string Edhrec { get; set; }

        [JsonProperty("mtgtop8")]
        public string Mtgtop8 { get; set; }
    }
}