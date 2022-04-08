using System.Collections.Generic;
using Newtonsoft.Json;

namespace RainbowCalculator.Model
{
    public class ScryfallCard
    {
        [JsonProperty("object")] public string Object { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("arena_id")] public int ArenaId { get; set; }

        [JsonProperty("oracle_id")] public string OracleId { get; set; }

        [JsonProperty("multiverse_ids")] public List<int> MultiverseIds { get; set; }

        [JsonProperty("mtgo_id")] public int MtgoId { get; set; }

        [JsonProperty("tcgplayer_id")] public int TcgplayerId { get; set; }

        [JsonProperty("cardmarket_id")] public int CardmarketId { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("lang")] public string Lang { get; set; }

        [JsonProperty("released_at")] public string ReleasedAt { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }

        [JsonProperty("scryfall_uri")] public string ScryfallUri { get; set; }

        [JsonProperty("layout")] public string Layout { get; set; }

        [JsonProperty("highres_image")] public bool HighresImage { get; set; }

        [JsonProperty("image_status")] public string ImageStatus { get; set; }

        [JsonProperty("image_uris")] public ImageUris ImageUris { get; set; }

        [JsonProperty("mana_cost")] public string ManaCost { get; set; }

        [JsonProperty("cmc")] public double Cmc { get; set; }

        [JsonProperty("type_line")] public string TypeLine { get; set; }

        [JsonProperty("oracle_text")] public string OracleText { get; set; }

        [JsonProperty("loyalty")] public string Loyalty { get; set; }
        [JsonProperty("power")] public string Power { get; set; }

        [JsonProperty("toughness")] public string Toughness { get; set; }

        [JsonProperty("colors")] public List<string> Colors { get; set; }

        [JsonProperty("color_identity")] public List<string> ColorIdentity { get; set; }

        [JsonProperty("keywords")] public List<string> Keywords { get; set; }

        [JsonProperty("produced_mana")] public List<string> ProducedMana { get; set; }

        [JsonProperty("card_faces")] public List<CardFace> CardFaces { get; set; }

        [JsonProperty("all_parts")] public List<AllPart> AllParts { get; set; }

        [JsonProperty("legalities")] public Legalities Legalities { get; set; }

        [JsonProperty("games")] public List<string> Games { get; set; }

        [JsonProperty("reserved")] public bool Reserved { get; set; }

        [JsonProperty("foil")] public bool Foil { get; set; }

        [JsonProperty("nonfoil")] public bool Nonfoil { get; set; }

        [JsonProperty("oversized")] public bool Oversized { get; set; }

        [JsonProperty("promo")] public bool Promo { get; set; }

        [JsonProperty("reprint")] public bool Reprint { get; set; }

        [JsonProperty("variation")] public bool Variation { get; set; }

        [JsonProperty("set")] public string Set { get; set; }

        [JsonProperty("set_name")] public string SetName { get; set; }

        [JsonProperty("set_type")] public string SetType { get; set; }

        [JsonProperty("set_uri")] public string SetUri { get; set; }

        [JsonProperty("set_search_uri")] public string SetSearchUri { get; set; }

        [JsonProperty("scryfall_set_uri")] public string ScryfallSetUri { get; set; }

        [JsonProperty("rulings_uri")] public string RulingsUri { get; set; }

        [JsonProperty("prints_search_uri")] public string PrintsSearchUri { get; set; }

        [JsonProperty("collector_number")] public string CollectorNumber { get; set; }

        [JsonProperty("digital")] public bool Digital { get; set; }

        [JsonProperty("rarity")] public string Rarity { get; set; }

        [JsonProperty("watermark")] public string Watermark { get; set; }

        [JsonProperty("flavor_text")] public string FlavorText { get; set; }

        [JsonProperty("card_back_id")] public string CardBackId { get; set; }

        [JsonProperty("artist")] public string Artist { get; set; }

        [JsonProperty("artist_ids")] public List<string> ArtistIds { get; set; }

        [JsonProperty("illustration_id")] public string IllustrationId { get; set; }

        [JsonProperty("border_color")] public string BorderColor { get; set; }

        [JsonProperty("frame")] public string Frame { get; set; }
        [JsonProperty("frame_effects")] public List<string> FrameEffects { get; set; }

        [JsonProperty("full_art")] public bool FullArt { get; set; }

        [JsonProperty("textless")] public bool Textless { get; set; }

        [JsonProperty("booster")] public bool Booster { get; set; }

        [JsonProperty("story_spotlight")] public bool StorySpotlight { get; set; }

        [JsonProperty("edhrec_rank")] public int EdhrecRank { get; set; }

        [JsonProperty("preview")] public Preview Preview { get; set; }

        [JsonProperty("prices")] public Prices Prices { get; set; }

        [JsonProperty("related_uris")] public RelatedUris RelatedUris { get; set; }

        [JsonProperty("purchase_uris")] public PurchaseUris PurchaseUris { get; set; }
    }

}