using RainbowModel.Scryfall;

namespace RainbowCore
{
    public class CardsParser
    {
        public List<ScryfallCard> GetCards(string deckString, out List<string> missingCards)
        {
            var deckList = new List<string>();
            var deckStringList = deckString.Split('|');
            foreach (var s in deckStringList)
            {
                var cardName = s;

                // trim
                cardName = cardName.Trim();

                // skip empty entries
                if (cardName.Equals(string.Empty)) continue;

                // skip existing entries
                if (deckList.Any(i => i == cardName)) continue;

                // remove leading numbers (in case someone enters '15 Island')
                while (int.TryParse(cardName.FirstOrDefault().ToString(), out _))
                {
                    cardName = cardName.Remove(0, 1);
                }

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
                    Console.WriteLine("could not find card " + cardString);
                    missing.Add(cardString);
                }
            }
            missingCards = missing;

            return cards;
        }
    }
}
