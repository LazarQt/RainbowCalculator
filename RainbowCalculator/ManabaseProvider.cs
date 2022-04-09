using RainbowCalculator.Model;
using RainbowCalculator.Util;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public LandSuggestion[] Retrieve(string deckString)
        {
            var reader = new CalculationFilesReader();

            var lands = reader.GetLands();

            var excludedCards = reader.GetExcluded();

            var deckList = new List<string>();
            var deckStringList = deckString.Split('|');
            foreach(var s in deckStringList)
            {
                var cardName = s;
                
                // trim
                cardName = cardName.Trim();

                // skip empty entries
                if (cardName.Equals(string.Empty)) continue;

                // remove leading numbers (in case someone enters '15 Island')
                while (int.TryParse(cardName.FirstOrDefault().ToString(), out _))
                {
                    cardName = cardName.Remove(0, 1);
                }

                deckList.Add(cardName.Trim());
            }

            var cards = Task.Run(() => reader.GetCards(deckList)).Result;

            var missing = new List<string>();
            foreach(var cardString in deckList)
            {
                if(!cards.Any(c=> c.Name.ToLower().StartsWith(cardString.ToLower())))
                {
                    Console.WriteLine("could not find card " + cardString);
                    missing.Add(cardString);
                }
            }

            var excluded = new List<string>();
            for(var i = deckList.Count - 1; i >= 0; i--)
            {
                var c = deckList[i];
                if (excludedCards.Contains(c.ToLower()))
                {
                    excluded.Add(c);
                    deckList.RemoveAt(i);
                }
            }


            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0,1) } };
        }
    }
}
