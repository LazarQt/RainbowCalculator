using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RainbowModel.Scryfall;
using RestSharp;

namespace RainbowCore
{
    public class ScryfallApi
    {
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
