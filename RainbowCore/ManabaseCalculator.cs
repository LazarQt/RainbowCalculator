using RainbowCore.Extensions;
using RainbowModel;
using RainbowModel.Scryfall;

namespace RainbowCore
{
    public class ManabaseCalculator
    {
        private readonly CsvReader _csvReader;

        public ManabaseCalculator()
        {
            _csvReader = new CsvReader();
        }

        public ManabaseSuggestion Calculate(string[] deck, string[] excludedLands)
        {
            // create suggestion, any relevant user information will be updated here
            var suggestion = new ManabaseSuggestion();

            try
            {



                if (deck == null || !deck.Any()) return suggestion;

                var cards = BuildCalculationDeck(deck, suggestion);

                suggestion.TotalRelevantCards = cards.Count;

                // read initial files
                var manarockRatio = CalculateManarockRatio(cards);
                var requirementsTracker = InitializeSourceRequirements(cards);

                suggestion.ManarockRatio = manarockRatio;
                suggestion.ColorRequirements = requirementsTracker.Requirements;

                // create possible land picks
                var landProperties = _csvReader.ReadFile<LandProperty>("lands");
                var categories = _csvReader.ReadFile<Category>("categories");

                // remove excluded lands (user argument)
                if (excludedLands != null)
                {
                    landProperties = landProperties.Where(p => !excludedLands.Contains(p.Name)).ToList();
                }

                var lands = new List<Land>();
                var deckIdentity = requirementsTracker.DeckIdentity;
                foreach (var p in landProperties)
                {
                    var land = p.TransformToLand();
                    if (!deckIdentity.CanInclude(land.Identity)) continue;

                    // fetch-able sources need to have one intersection with identity otherwise there is nothing to fetch
                    if (!deckIdentity.Intersect(p.SingleMatch.ToLower().ToCharArray()).Any())
                        continue;

                    var category =
                        categories.First(c =>
                            c.Cycle == land.Cycle); // there can only ever be one cycle unless data is faulty

                    land.Order = deckIdentity.Length switch
                    {
                        2 => category.TwoColor,
                        3 => category.ThreeColor,
                        4 => category.FourColor,
                        5 => category.FiveColor,
                        _ => land.Order
                    };

                    // exclude -1 order lands
                    if (land.Order > 0) lands.Add(land);
                }

                lands = lands.OrderBy(l => l.Order).ToList();

                //manarock calculations
                var manaRockProperties = _csvReader.ReadFile<ManarockProperty>("manarocks");
                var manaRocks = new List<Manarock>();
                foreach (var m in manaRockProperties)
                {
                    var manaRock = m.TransformToManarock();
                    if (!deckIdentity.CanInclude(manaRock.Identity)) continue;
                    manaRocks.Add(manaRock);
                }

                manaRocks = manaRocks.OrderBy(l => l.Order).ToList();

                var landsSuggestion = new List<Land>();
                var rocksSuggestion = new List<Manarock>();

                // 1st step: Add required amount of mana rocks to deck
                while (rocksSuggestion.Count < manarockRatio.ManaRocks)
                {
                    var bestRock =
                        manaRocks.First(r => r.Produces.Contains(requirementsTracker.HighestColorRequirement));
                    rocksSuggestion.Add(new Manarock(bestRock));

                    foreach (var producedMana in bestRock.Produces)
                    {
                        requirementsTracker.ReduceRequirement(producedMana);
                    }

                    manaRocks.Remove(bestRock);
                }

                // 2nd step: Add one basic land of each color in deck identity
                foreach (var req in requirementsTracker.Requirements)
                {
                    landsSuggestion.Add(GenerateBasicLand(req.Color));
                    requirementsTracker.ReduceRequirement(req.Color);
                }


                // 3rd step: add lands to meet requirements
                while (requirementsTracker.TotalRequirementsCount + landsSuggestion.Count > manarockRatio.Lands &&
                       lands.Any() && landsSuggestion.Count < manarockRatio.Lands)
                {
                    var illegalRequirements = new List<char>();
                    foreach (var r in requirementsTracker.Requirements)
                    {
                        if (r.Amount <= 0)
                            illegalRequirements.Add(r.Color);
                    }

                    var l = lands.First(i => i.DoesNotProduce(illegalRequirements));
                    landsSuggestion.Add(l);
                    lands.Remove(l);
                    foreach (var c in l.Produces)
                    {
                        requirementsTracker.ReduceRequirement(c);
                    }
                }

                // 4th step: fill up deck with basic lands if there is space left
                foreach (var r in requirementsTracker.Requirements)
                {
                    var amount = r.Amount;
                    for (var i = 0; i < amount; i++)
                    {
                        landsSuggestion.Add(GenerateBasicLand(r.Color));
                    }

                    requirementsTracker.ReduceRequirement(r.Color, amount);
                }

                suggestion.Lands = new List<string>();
                var groups = landsSuggestion.GroupBy(i => i.Name);
                foreach (var grp in groups)
                {
                    suggestion.Lands.Add($"{grp.Count()} {grp.Key}");
                }

                suggestion.Lands.AddRange(rocksSuggestion.Select(r => "1 " + r.Name));

                return suggestion;
            }
            catch (Exception e)
            {
                suggestion.Error = e.Message;
                return suggestion;
            }
        }

        private ColorSourceRequirementTracker InitializeSourceRequirements(List<ScryfallCard> cards)
        {
            // get color requirements based on pips in deck (for example, 1BB requires higher count of sources than 2B)
            var colorPipsRequirements = _csvReader.ReadFile<ColorPipsRequirement>("pips");

            // get requirements
            var colorSourceRequirementsTracker = new ColorSourceRequirementTracker();
            foreach (var card in cards)
            {
                // ignore cards
                var colorPipsRequirement = colorPipsRequirements.Where(p => p.Cost == card.ManaValue).ToList();
                if (!colorPipsRequirement.Any())
                {
                    Console.WriteLine($"Can't calculate color pip requirement for {card.Name}");
                    continue;
                    // todo: log which cards can not be calculated in case some crazy high pips are implemented in the future
                }

                var manaCost = card.GetManaCost();

                // if mana costs contain a slash ignore them from now (hybrid mana cost) todo: implement hybrid mana cost calculations
                if (manaCost.Contains("/"))
                {
                    Console.WriteLine($"Ignoring hybrid mana cost of {card.Name}");
                    continue;
                    // todo: log for debugging purposes
                }

                foreach (var color in Const.Colors)
                {
                    var currentColor = manaCost.Count(m => m == color);
                    if (currentColor <= 0) continue; // no color found of this type
                    var currentPipsRequirement = colorPipsRequirement.Where(p => p.Pips == currentColor).ToList();
                    if (!currentPipsRequirement.Any() || currentPipsRequirement.Count != 1)
                    {
                        Console.WriteLine($"Can't find pip requirement for {card.Name}");
                        continue;
                        // todo: pip requirement not found? this means new cards have been implemented. this should be logged
                    }

                    var sources = currentPipsRequirement.First().Sources;
                    if (colorSourceRequirementsTracker.HasColor(color))
                    {
                        // if requirements for this color are already present, update if new value requires higher amount of sources
                        var previousValue = colorSourceRequirementsTracker.GetColorRequirementCount(color);
                        if (sources > previousValue)
                        {
                            colorSourceRequirementsTracker.SetColorRequirement(color, sources);
                        }
                    }
                    else
                    {
                        // if requirement for this color is new, add it to list of requirements
                        colorSourceRequirementsTracker.SetColorRequirement(color, sources);
                    }
                }
            }

            return colorSourceRequirementsTracker;
        }

        private ManarockRatio CalculateManarockRatio(List<ScryfallCard> cards)
        {
            // get required land and manarock count, this will be derived from average mana value
            var manarockRatios = _csvReader.ReadFile<ManarockRatio>("manarockratios");

            var avgMv = cards.Average(c => c.Cmc);
            var manarockRatio = manarockRatios.FirstOrDefault(r => avgMv >= r.MinMv && avgMv <= r.MaxMv);
            if (manarockRatio == null) throw new Exception("can't derive lands and mana rocks requirement");

            return manarockRatio;
        }

        private List<ScryfallCard> BuildCalculationDeck(string[] deck, ManabaseSuggestion suggestion)
        {
            // these cards are strictly excluded from calculation because they have X in costs or are regularly reduced in costs
            var excludedCards = _csvReader.ReadFile<SingleLine>("exclude");

            // get actual cards from deck string
            var cards = new CardsParser().GetCards(deck, out var missing);

            suggestion.CardsNotFound = missing;

            // remove all excluded cards which are not part of the equation
            var excluded = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (excludedCards.Any(x => x.Name.ToLower() == c.Name.ToLower()))
                {
                    excluded.Add(c.Name);
                    cards.RemoveAt(i);
                }
            }

            suggestion.ExcludedCards = excluded;

            // remove lands (if user gave list with lands in it already)
            var removedLands = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (!c.TypeLine.ToLower().Contains(Const.Card.Land.ToLower())) continue;
                removedLands.Add(c.Name);
                cards.RemoveAt(i);
            }

            suggestion.RemovedLands = removedLands;

            return cards;
        }

        private Land GenerateBasicLand(char color)
        {
            return color switch
            {
                'w' => new Land() { Identity = new[] { color }, Name = Const.Card.Plains, Produces = new[] { 'w' } },
                'u' => new Land() { Identity = new[] { color }, Name = Const.Card.Island, Produces = new[] { 'u' } },
                'b' => new Land() { Identity = new[] { color }, Name = Const.Card.Swamp, Produces = new[] { 'b' } },
                'r' => new Land() { Identity = new[] { color }, Name = Const.Card.Mountain, Produces = new[] { 'r' } },
                'g' => new Land() { Identity = new[] { color }, Name = Const.Card.Forest, Produces = new[] { 'g' } },
                _ => throw new Exception("no basic land found with color" + color)
            };
        }
    }
}
