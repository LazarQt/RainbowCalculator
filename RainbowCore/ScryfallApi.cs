using Newtonsoft.Json;
using RainbowModel.Scryfall;
using RestSharp;

namespace RainbowCore
{
    public class ScryfallApi
    {
        /// <summary>
        /// Generate list of cards based on string list
        /// </summary>
        /// <param name="sourceCardList">Cards to be looked for in Scryfall database</param>
        /// <returns>Card list with Scryfall card objects</returns>
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
            if (content.Data != null) result.AddRange(content.Data);

            return result;
        }
    }
}
