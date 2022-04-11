using RainbowCalculator.Extensions;
using RainbowCalculator.Model;
using RainbowCalculator.Util;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public LandSuggestion[] Retrieve(string deckString)
        {
            var reader = new CalculationFilesReader();

            // get all lands that can be included in deck
            var lands = reader.GetLands();

            // get color requirements based on pips in deck (for example, 1BB requires higher count of sources than 2B)
            var colorPipsRequirements = reader.GetColorPipsRequirements();

            // get required land and manarock count, this will be derived from average mana value
            var landAndManarockRequirements = reader.GetLandAndManarockRequirements();

            // these cards are strictly excluded from calculation because they have X in costs or are regularly reduced in costs
            var excludedCards = reader.GetExcludedCards();

            // get actual cards from deck string
            var missing = new List<string>();
            var cards = new CardsParser().GetCards(deckString, out missing);
            // report missing cards
            Logger.Log("Missing cards", missing);

            // remove all excluded cards which are not part of the equation
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
            Logger.Log("Removed due to X etc.", excluded);

            // remove lands (if user gave list with lands in it already)
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
            Logger.Log("Removed lands (why give lands with calculation?)", removedLands);

            var avgMv = cards.Average(c => c.Cmc);
            var landsAndRocksRequirement = landAndManarockRequirements.FirstOrDefault(r => avgMv >= r.MinMv && avgMv <= r.MaxMv);
            if (landsAndRocksRequirement == null) throw new Exception("can't derive lands and mana rocks requirement");
            Logger.Log($"Average MV: {avgMv}");

            // get requirements
            var srcRequirements = new Dictionary<char, int>(); // how many sources of each color are needed
            var ignoredCardsForRequirement = new List<string>(); // ignored cards because no color pip table entry exists (for example too high MV)
            var harshesReqs = new Dictionary<char, string>(); // display which cards are responsible for harshest color requirement
            foreach (var card in cards)
            {
                // ignore cards
                var pipCalc = colorPipsRequirements.Where(p => p.Cost == card.Cmc);
                if (!pipCalc.Any())
                {
                    ignoredCardsForRequirement.Add(card.Name);
                    continue;
                }

                var manaCost = card.GetManaCost();
                foreach (var color in Const.Colors)
                {
                    var ofThisColor = manaCost.Count(m => m == color);
                    var r = colorPipsRequirements.Where(p => p.Cost == card.Cmc && p.Pips == ofThisColor);
                    if (r.Any() && r.Count() == 1) 
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
            }

            var eligibleLands = lands.Where(l => l.Cutoff <= srcRequirements.Count && l.Identity.ToLower().ToArray().ToList().All(a => srcRequirements.ContainsKey(a))).OrderBy(x => x.Order).ToList();

            var deckLands = new List<Land>();

            //foreach (var req in srcRequirements)
            //{
            //    var added = 0;
            //    while (added < req.Value)
            //    {
            //        if (req.Key == 'w') deckLands.Add(new Land() { Identity = "W", Name = "Plains", Produces = "W" });
            //        if (req.Key == 'u') deckLands.Add(new Land() { Identity = "U", Name = "Island", Produces = "U" });
            //        if (req.Key == 'b') deckLands.Add(new Land() { Identity = "B", Name = "Swamp", Produces = "B" });
            //        if (req.Key == 'r') deckLands.Add(new Land() { Identity = "R", Name = "Mountain", Produces = "R" });
            //        if (req.Key == 'g') deckLands.Add(new Land() { Identity = "G", Name = "Forest", Produces = "G" });
            //        added++;
            //    }
            //}

            //while(deckLands.Count > landsAndRocksRequirement.LandsWithoutRocks)
            //{

            //}

            // fill up with lands until requirements are met
            while (srcRequirements.Any(r => r.Value > 0))
            {
                if (eligibleLands.Count <= 0) throw new Exception("no lands available");
                var land = eligibleLands.First();
                deckLands.Add(land);

                foreach (var a in land.Produces.ToLower())
                {
                    if (!srcRequirements.ContainsKey(a)) continue;
                    srcRequirements[a] = srcRequirements[a] - 1;
                }

                eligibleLands.RemoveAt(0);
            }

            var removedTooMany = new List<string>();
            for (var i = deckLands.Count - 1; i >= 0; i--)
            {
                var deckLand = deckLands[i];
                var remove = true;
                foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
                {

                    if (srcRequirements.ContainsKey(c) && srcRequirements[c] >= 0)
                    {
                        remove = false;
                        break;
                    }
                }

                if (remove)
                {
                    removedTooMany.Add(deckLand.Name);
                    deckLands.RemoveAt(i);
                    foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
                    {
                        srcRequirements[c] += 1;
                    }
                }
            }

            var removedTooMany2 = new List<string>();
            for (var i = deckLands.Count - 1; i >= 0; i--)
            {
                var deckLand = deckLands[i];
                var leftOutColor = new List<char>();
                var remove = true;
                foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
                {

                    if (srcRequirements.ContainsKey(c) && srcRequirements[c] < 0)
                    {
                        leftOutColor.Add(c);
                    } 
                }

                if (leftOutColor.Count == 1 && deckLand.Produces.ToLower().Replace("c", "").ToArray().Length == 2)
                {
                    removedTooMany2.Add(deckLand.Name);
                    deckLands.RemoveAt(i);
                    var x = deckLand.Produces.ToLower().Replace("c", "").ToArray().Except(leftOutColor).ToList();
                    if (x.Count != 1) throw new Exception("this cant be true");
                    deckLands.Add(GetBasic(x.FirstOrDefault()));
                }
            }

            var q = "";
            foreach (var l in deckLands)
            {
                q += $"!\"{l.Name}\" or ";
            }

            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0, 1) } };
        }

        private Land GetBasic(char k)
        {
            if (k == 'w') return new Land() { Identity = "W", Name = "Plains", Produces = "W" };
            if (k == 'u') return new Land() { Identity = "U", Name = "Island", Produces = "U" };
            if (k == 'b') return new Land() { Identity = "B", Name = "Swamp", Produces = "B" };
            if (k == 'r') return new Land() { Identity = "R", Name = "Mountain", Produces = "R" };
            if (k == 'g') return new Land() { Identity = "G", Name = "Forest", Produces = "G" };
            throw new Exception("no basic found with type" + k);
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