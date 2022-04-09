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

            var pipSources = reader.GetPipsAndSources();

            var landsAndRocks = reader.GetLandsAndRocks();

            var excludedCards = reader.GetExcluded();

            var deckList = new List<string>();
            var deckStringList = deckString.Split('|');
            foreach (var s in deckStringList)
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
            foreach (var cardString in deckList)
            {
                if (!cards.Any(c => c.Name.ToLower().StartsWith(cardString.ToLower())))
                {
                    Console.WriteLine("could not find card " + cardString);
                    missing.Add(cardString);
                }
            }

            var excluded = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (excludedCards.Contains(c.Name.ToLower()))
                {
                    excluded.Add(c.Name);
                    cards.RemoveAt(i);
                }
            }

            // remove lands
            var removedLands = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (c.TypeLine.ToLower().Contains("land"))
                {
                    removedLands.Add(c.Name);
                    cards.RemoveAt(i);
                }
            }

            var avgMv = cards.Average(c => c.Cmc);

            var landsAndRocksRequirement = landsAndRocks.FirstOrDefault(r => avgMv >= r.MinMv && avgMv <= r.MaxMv);
            if (landsAndRocksRequirement == null) throw new Exception("lands and rocks not found");

            // get requirements
            var srcRequirements = new Dictionary<char, int>();
            var ignoredCardsForRequirement = new List<string>();
            var colors = new List<char>() { 'w', 'u', 'b', 'r', 'g' };
            var harshesReqs = new Dictionary<char, string>();
            foreach (var card in cards)
            {
                var pipCalc = pipSources.Where(p => p.Cost == card.Cmc);
                if (!pipCalc.Any())
                {
                    ignoredCardsForRequirement.Add(card.Name);
                    continue;
                }

                var manaCost = card.ManaCost;
                if (manaCost == null)
                {
                    manaCost = card.CardFaces.First().ManaCost;
                }
                manaCost = manaCost.ToLower();

                // remove eldrain split card thingies
                if (manaCost.Contains("//"))
                {
                    manaCost = manaCost.Substring(0, manaCost.IndexOf("//"));
                }

                foreach (var color in colors)
                {
                    var ofThisColor = manaCost.Count(m => m == color);
                    var r = pipSources.Where(p => p.Cost == card.Cmc && p.Pips == ofThisColor);
                    if (r.Any())
                    {
                        if (r.Count() != 1)
                        {
                            Console.WriteLine("Fil");
                        }
                        else
                        {
                            var sources = r.First().Sources;
                            if (srcRequirements.ContainsKey(color))
                            {
                                var previousValue = srcRequirements[color];
                                if (sources > previousValue)
                                {
                                    srcRequirements[color] = sources;
                                    harshesReqs[color] = card.Name;
                                }

                            }
                            else
                            {
                                srcRequirements[color] = sources;
                                harshesReqs[color] = card.Name;
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Fil");
                    }
                }



            }

            var eligibleLands = lands.Where(l => l.Cutoff <= srcRequirements.Count && l.Identity.ToLower().ToArray().ToList().All(a => srcRequirements.ContainsKey(a))).OrderBy(x => x.Order).ToList();

            var deckLands = new List<Land>();
            while(srcRequirements.Any(r => r.Value > 0))
            {
                if (eligibleLands.Count <= 0) throw new Exception("no lands available");
                var land = eligibleLands.First();
                deckLands.Add(land);

                foreach(var a in land.Produces.ToLower())
                {
                    if (!srcRequirements.ContainsKey(a)) continue;
                    srcRequirements[a] = srcRequirements[a] - 1;
                }

                eligibleLands.RemoveAt(0);
            }

            var removedTooMany = new List<string>();
            for(var i = deckLands.Count -1; i >= 0; i--)
            {
                var deckLand = deckLands[i];
                var remove = true;
                foreach(var c in deckLand.Produces.ToLower().Replace("c","").ToArray())
                {

                    if (srcRequirements.ContainsKey(c) && srcRequirements[c] >= 0)
                    {
                        remove = false;
                        break;
                    }
                }

                if(remove)
                {
                    removedTooMany.Add(deckLand.Name);
                    deckLands.RemoveAt(i);
                    foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
                    {
                        srcRequirements[c] += 1;
                    }
                }
            }

            var q = "";
            foreach(var l in deckLands)
            {
                q += $"!\"{l.Name}\" or ";
            }
            
            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0, 1) } };
        }
    }

    public class Face
    {
        public int Cmc { get; set; }
        public string Cost { get; set; }
    }
}

// todo: include DFC at some point
//var costs = new List<Face>();
//foreach(var card in cards)
//{
//    foreach (var color in colors)
//    {
//        var cost = card.ManaCost;
//        if (cost == null)
//        {
//            foreach(var face in card.CardFaces)
//            {
//                costs.Add(new Face()
//                {
//                    Cmc = face.cmc
//                });
//            }
//        } else
//        {
//            costs.Add(cost);
//        }
//    }
//}