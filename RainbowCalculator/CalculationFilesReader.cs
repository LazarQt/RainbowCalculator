using Newtonsoft.Json;
using RainbowCalculator.Model;
using RainbowCalculator.Util;
using RestSharp;

namespace RainbowCalculator
{
    public class CalculationFilesReader
    {
        private const string Root = "CalcFiles/";

        public List<Land> GetLands()
        {
            List<Category> categories = new List<Category>();
            var categoryLines = CsvUtil.ReadLines(@$"{Root}Categories.csv");
            foreach (var line in categoryLines)
            {
                categories.Add(new Category()
                {
                    Cycle = line[0],
                    Enters = line[1],
                    Order = Convert.ToDouble(line[2]),
                    Cutoff = Convert.ToInt32(line[3])
                });
            }

            List<Land> lands = new List<Land>();
            var landLines = CsvUtil.ReadLines(@$"{Root}Lands_Filtered.csv");
            foreach (var line in landLines)
            {
                var category = categories.FirstOrDefault(c => c.Cycle == line[4]);
                if (category == null) throw new Exception("unknown land category");

                // ignore expensive lands...  todo: create filter for this
                //if (category.Cycle == "True Duals" || category.Cycle == "Fetchlands") continue;
                var exclude =  category.Cycle == "True Duals" || category.Cycle == "Fetchlands";

                lands.Add(new Land()
                {
                    Name = line[0],
                    Identity = line[1],
                    Produces = line[2],
                    Order = category.Order,
                    Cutoff = category.Cutoff,
                    Exclude = exclude
                });
            }

            return lands;
        }

        public List<string> GetExcludedCards()
        {
            List<string> exclude = new List<string>();
            using (var reader = new StreamReader(@$"{Root}exclude.txt"))
            {
                while (!reader.EndOfStream)
                {
                    exclude.Add(reader.ReadLine().ToLower());
                }
            }
            return exclude;
        }

        public List<ColorPipsRequirement> GetColorPipsRequirements()
        {
            List<ColorPipsRequirement> pipSources = new List<ColorPipsRequirement>();
            var lines = CsvUtil.ReadLines(@$"{Root}PipsSources.csv");
            foreach (var line in lines)
            {
                pipSources.Add(new ColorPipsRequirement()
                {
                    Cost = Convert.ToInt32(line[0]),
                    Pips = Convert.ToInt32(line[1]),
                    Sources = Convert.ToInt32(line[2])
                });
            }
            return pipSources;
        }

        public List<LandAndManarockRequirement> GetLandAndManarockRequirements()
        {
            List<LandAndManarockRequirement> landsAndRocksSources = new List<LandAndManarockRequirement>();
            var lines = CsvUtil.ReadLines(@$"{Root}LandsAndRocks.csv");
            foreach (var line in lines)
            {
                landsAndRocksSources.Add(new LandAndManarockRequirement()
                {
                    LandsWithoutRocks = Convert.ToInt32(line[0]),
                    MinMv = Convert.ToDouble(line[1]),
                    MaxMv = Convert.ToDouble(line[2]),
                    ManaRocks = Convert.ToInt32(line[3]),
                    Lands = Convert.ToInt32(line[4]),
                });
            }
            return landsAndRocksSources;
        }

        public async Task<List<ScryfallCard>> GetCards(List<string> sourceCardList)
        {
            var result = new List<ScryfallCard>();

            var cardList = new List<string>(sourceCardList);

            while (cardList.Any())
            {
                var query = $"!\"{cardList.First()}\"";
                cardList.RemoveAt(0);

                while (cardList.Any() && query.Length < 800)
                {
                    query += $" or !\"{cardList.First()}\"";
                    cardList.RemoveAt(0);
                }

                var cards = await ExecuteCardSearch($"https://api.scryfall.com/cards/search?q={query}");
                result.AddRange(cards);
            }

            return result;
        }

        private async Task<List<ScryfallCard>> ExecuteCardSearch(string request)
        {

            var result = new List<ScryfallCard>();

            var response = await new RestClient().GetAsync(new RestRequest(request));
            if (response.Content == null) throw new Exception("card search error");

            var content = JsonConvert.DeserializeObject<CardsSearch>(response.Content);
            if (content == null) throw new Exception("no content");

            // it's possible that a search returns no cards in which case Data is null but it's not an error
            if(content.Data != null) result.AddRange(content.Data);

            return result;

        }

        //public string GetCards()
        //{
        //    static async Task<List<ScryfallCard>> GetCards(string request, List<ScryfallCard> lands = null)
        //    {
        //        if (lands == null) lands = new List<ScryfallCard>();

        //        var response = await new RestClient().GetAsync(new RestRequest(request));
        //        if (response.Content == null) throw new Exception("card search error");

        //        var content = JsonConvert.DeserializeObject<CardsSearch>(response.Content);
        //        if (content == null) throw new Exception("no content");
        //        lands.AddRange(content.Data);

        //        if (content.HasMore)
        //        {
        //            return await GetCards(content.NextPage, lands);
        //        }
        //        else
        //        {
        //            return lands;
        //        }
        //    }
        //}
    }
}
