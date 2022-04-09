using Newtonsoft.Json;
using RainbowCalculator.Model;
using RainbowCalculator.Util;
using RestSharp;

namespace RainbowCalculator
{
    public class CalculationFilesReader
    {
        public List<Land> GetLands()
        {
            List<Category> categories = new List<Category>();
            var categoryLines = CsvUtil.ReadLines(@"Categories.csv");
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
            var landLines = CsvUtil.ReadLines(@"Lands_Filtered.csv");
            foreach (var line in landLines)
            {
                var category = categories.FirstOrDefault(c => c.Cycle == line[4]);
                if (category == null) throw new Exception("unknown land category");
                lands.Add(new Land()
                {
                    Name = line[0],
                    Identity = line[1],
                    Produces = line[2],
                    Order = category.Order,
                    Cutoff = category.Cutoff
                });
            }

            return lands;
        }

        public List<string> GetExcluded()
        {
            List<string> exclude = new List<string>();
            using (var reader = new StreamReader(@"exclude.txt"))
            {
                while (!reader.EndOfStream)
                {
                    exclude.Add(reader.ReadLine().ToLower());
                }
            }
            return exclude;
        }

        public List<PipSources> GetPipsAndSources()
        {
            List<PipSources> pipSources = new List<PipSources>();
            var lines = CsvUtil.ReadLines(@"PipsSources.csv");
            foreach (var line in lines)
            {
                pipSources.Add(new PipSources()
                {
                    Cost = Convert.ToInt32(line[0]),
                    Pips = Convert.ToInt32(line[1]),
                    Sources = Convert.ToInt32(line[2])
                });
            }
            return pipSources;
        }

        public List<LandsAndRocksReq> GetLandsAndRocks()
        {
            List<LandsAndRocksReq> landsAndRocksSources = new List<LandsAndRocksReq>();
            var lines = CsvUtil.ReadLines(@"LandsAndRocks.csv");
            foreach (var line in lines)
            {
                landsAndRocksSources.Add(new LandsAndRocksReq()
                {
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
