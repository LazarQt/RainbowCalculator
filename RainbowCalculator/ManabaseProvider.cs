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

            var x = GetPrelimLands(cards, lands, colorPipsRequirements, landsAndRocksRequirement,
                0);


            //// rebalance ?
            //while (srcRequirements.Any(s => s.Value != 0) && eligibleLands.Any())
            //{
            //    var requirements = srcRequirements.Where(s => s.Value > 0).Select(x => x.Key);
            //    var strictNonRequirements = srcRequirements.Where(s => s.Value < 0).Select(x => x.Key);

            //    var mostNeeded = srcRequirements.First(a => a.Value == srcRequirements.Max(s => s.Value)).Key;
            //    var leastNeeded = srcRequirements.First(a => a.Value == srcRequirements.Min(s => s.Value)).Key;

            //    var match = eligibleLands.FirstOrDefault(l => l.CanProduce(new char[] { mostNeeded }) && l.DoesNotProduce(new char[] { leastNeeded }));

            //    if (srcRequirements.Any(r => r.Value == 0))
            //    {
            //        Logger.Log("arive aat 0");
            //    }


            //    if (srcRequirements.All(v => v.Value <= 0))
            //    {
            //        // basics after everything else done

            //        Logger.Log("");
            //    }
            //    if (match == null)
            //    {
            //        if (requirements.Count() == 1)
            //        {
            //            match = GetBasic(requirements.First());
            //        }
            //        else
            //        {
            //            throw new Exception("no more lands!");
            //        }
            //    }//

            //    var removeMatch = deckLands.LastOrDefault(l => l.CanProduce(new char[] { leastNeeded }));
            //    if (removeMatch == null) throw new Exception("nothing to remove!");

            //    deckLands.Remove(removeMatch);
            //    deckLands.Add(match);

            //    foreach (var removeReq in removeMatch.ProduceColors())
            //    {
            //        if (srcRequirements.ContainsKey(removeReq)) srcRequirements[removeReq] += 1;
            //    }

            //    foreach (var addReq in match.ProduceColors())
            //    {
            //        if (srcRequirements.ContainsKey(addReq)) srcRequirements[addReq] -= 1;
            //    }

            //    eligibleLands.Remove(match);
            //}

            //var removedTooMany = new List<string>();
            //for (var i = deckLands.Count - 1; i >= 0; i--)
            //{
            //    var deckLand = deckLands[i];
            //    var remove = true;
            //    foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
            //    {

            //        if (srcRequirements.ContainsKey(c) && srcRequirements[c] >= 0)
            //        {
            //            remove = false;
            //            break;
            //        }
            //    }

            //    if (remove)
            //    {
            //        removedTooMany.Add(deckLand.Name);
            //        deckLands.RemoveAt(i);
            //        foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
            //        {
            //            srcRequirements[c] += 1;
            //        }
            //    }
            //}

            //var removedTooMany2 = new List<string>();
            //for (var i = deckLands.Count - 1; i >= 0; i--)
            //{
            //    var deckLand = deckLands[i];
            //    var leftOutColor = new List<char>();
            //    var remove = true;
            //    foreach (var c in deckLand.Produces.ToLower().Replace("c", "").ToArray())
            //    {

            //        if (srcRequirements.ContainsKey(c) && srcRequirements[c] < 0)
            //        {
            //            leftOutColor.Add(c);
            //        }
            //    }

            //    if (leftOutColor.Count == 1 && deckLand.Produces.ToLower().Replace("c", "").ToArray().Length == 2)
            //    {
            //        removedTooMany2.Add(deckLand.Name);
            //        deckLands.RemoveAt(i);
            //        var x = deckLand.Produces.ToLower().Replace("c", "").ToArray().Except(leftOutColor).ToList();
            //        if (x.Count != 1) throw new Exception("this cant be true");
            //        deckLands.Add(GetBasic(x.FirstOrDefault()));
            //    }
            //}

            //var q = "";
            //foreach (var l in deckLands)
            //{
            //    q += $"!\"{l.Name}\" or ";
            //}

            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0, 1) } };
        }

        private List<Land> GetPrelimLands(List<ScryfallCard> cards, List<Land> lands,
            List<ColorPipsRequirement> colorPipsRequirements, LandAndManarockRequirement landsAndRocksRequirement,
            int cuttOfCount)
        {


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
                if (manaCost.Contains("/"))
                {
                    // ignoring hybrid!
                    Console.WriteLine("ignoring" + card.Name);
                    continue;
                }
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


            ////////////////////


            var deckLands = new List<Land>();

            var eligibleLands = lands.Where(
                l => l.Cutoff <= srcRequirements.Count + cuttOfCount && l.Identity.ToLower().ToArray().ToList().All(a => srcRequirements.ContainsKey(a))).
                OrderByDescending(x => x.Cutoff).ThenBy(x => x.Order).ToList();
            foreach (var x in srcRequirements) deckLands.Add(GetBasic(x.Key));
            if (srcRequirements.Count + cuttOfCount <= 5)
            {
                //eligibleLands = eligibleLands.Where(l => !l.Exclude).ToList();
            }

            // fill up until land count met
            while (eligibleLands.Any() && deckLands.Count < landsAndRocksRequirement.LandsWithoutRocks && srcRequirements.Any(r => r.Value > 0))
            {
                Land land;
                if (eligibleLands.Count <= 0)
                {
                    //throw new Exception("no lands available");
                    land = GetBasic(srcRequirements.First(a => a.Value == srcRequirements.Max(s => s.Value)).Key);
                }
                else
                {
                    land = eligibleLands.First();
                    if (eligibleLands.Any()) eligibleLands.RemoveAt(0);
                }

                var basics = new string[] { "Plains", "Island", "Swamp", "Mountain", "Forest" };
                if (deckLands.Any(l => l.Name == land.Name && !basics.Contains(l.Name))) continue;

                var mostNeeded = srcRequirements.First(a => a.Value == srcRequirements.Max(s => s.Value)).Key;
                var leastNeeded = srcRequirements.First(a => a.Value == srcRequirements.Min(s => s.Value)).Key;

                if (land.CanProduce(new char[] { leastNeeded }) && land.DoesNotProduce(new char[] { mostNeeded }))
                {
                    continue;
                }

                deckLands.Add(land);

                foreach (var a in land.Produces.ToLower())
                {
                    if (!srcRequirements.ContainsKey(a)) continue;
                    srcRequirements[a] = srcRequirements[a] - 1;
                }
            }

            if (srcRequirements.Where(x => x.Value >= 0).Sum(s => s.Value) < landsAndRocksRequirement.LandsWithoutRocks - deckLands.Count)
            {
                // fill with basics!
                foreach (var s in srcRequirements)
                {
                    for (var i = 0; i < s.Value; i++)
                    {
                        deckLands.Add(GetBasic(s.Key));
                        srcRequirements[s.Key] -= 1;
                    }
                }
            }
            else if (srcRequirements.Any(r => r.Value > 0))
            {
                if (srcRequirements.Count + cuttOfCount >= 7)
                {
                    throw new Exception("can't create manabase with this configuration");
                }
                return GetPrelimLands(cards, lands, colorPipsRequirements, landsAndRocksRequirement,
                    cuttOfCount + 1);
            }


            if (srcRequirements.All(s => s.Value <= 0))
            {
                var dFinal = deckLands.OrderByDescending(o => o.Order);
                return deckLands;
            }
            else
            {
                if (srcRequirements.Sum(s => s.Value) <= landsAndRocksRequirement.LandsWithoutRocks - deckLands.Count)
                {
                    Console.WriteLine("fill with basics!");
                    foreach (var s in srcRequirements)
                    {
                        for (var i = 0; i < s.Value; i++)
                        {
                            deckLands.Add(GetBasic(s.Key));

                        }
                    }
                }
                return GetPrelimLands(cards, lands, colorPipsRequirements, landsAndRocksRequirement,
                    cuttOfCount + 1);
            }
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