using RainbowModel.Scryfall;

namespace RainbowCore
{
    public class CardsParser
    {
        /// <summary>
        /// Sanitize a list of cards and try to find them in Scryfall database
        /// </summary>
        /// <param name="deck">Un-sanitized deck list</param>
        /// <param name="missingCards">Cards that could not be found</param>
        /// <returns>List of Scryfall cards</returns>
        public List<ScryfallCard> GetCards(string[] deck, out List<string> missingCards)
        {
            var deckList = new List<string>();
            foreach (var s in deck)
            {
                var cardName = s;

                // trim
                cardName = cardName.Trim();

                // skip empty entries
                if (cardName.Equals(string.Empty)) continue;

                // skip existing entries
                if (deckList.Any(i => i == cardName)) continue;
                
                deckList.Add(cardName.Trim());
            }

            // retrieve cards
            var cards = Task.Run(() => new ScryfallApi().GetCards(deckList)).Result;

            // make a list of missing cards (wrong user input?)
            var missing = new List<string>();
            foreach (var cardString in deckList)
            {
                if (!cards.Any(c => c.Name.ToLower().StartsWith(cardString.ToLower())))
                {
                    missing.Add(cardString);
                }
            }
            missingCards = missing;

            return cards;
        }
    }
}
