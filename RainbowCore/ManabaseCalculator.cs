using System.Globalization;
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

        public ManabaseSuggestion Calculate(string[] deck, string[] excludedLands, string[] ignoreAverageMv, int? stepUp = null)
        {
            // create suggestion, any relevant user information will be updated here
            var suggestion = new ManabaseSuggestion();

            // no deck provided, wrong request?
            if (deck == null || !deck.Any()) return suggestion;

            try
            {
                // retrieve cards from scryfall and log missing cards
                var cards = new CardsParser().GetCards(deck, out var missing);
                suggestion.CardsNotFound = missing;

                // these cards are strictly excluded from calculation because they have X in costs or are regularly reduced in costs
                var cardsToExclude = _csvReader.ReadFile<SingleLine>("exclude").Select(x => x.Name);
                suggestion.ExcludedCards = cards.Where(c => cardsToExclude.Any(e => e.IsThis(c))).Select(i => i.Name).ToList();

                // remove lands (if user gave list with lands in it already)
                var landsToRemove = cards.Where(c => c.IsLand()).ToList();
                suggestion.RemovedLands = landsToRemove.Select(l => l.Name).ToList();
                cards = cards.Except(landsToRemove).ToList();

                // add all relevant cards to list if needed later
                suggestion.RelevantCardList = cards.Select(c => c.Name).ToList();

                // calculate average mana value of deck
                suggestion.AverageManaValue = cards.Average(c => c.Cmc).ToString(CultureInfo.InvariantCulture);

                // calculate mana rock ratios
                var manarockRatio = CalculateManarockRatio(cards, ignoreAverageMv);
                suggestion.ManarockRatio = manarockRatio;

                var requirementsTracker = InitializeSourceRequirements(cards, out List<string> errors);
                suggestion.ColorRequirementsErrors = errors;
                suggestion.ColorRequirements = requirementsTracker.Requirements;

                //manarock calculations
                var manaRockProperties = _csvReader.ReadFile<ManarockProperty>("manarocks");
                var manaRocks = manaRockProperties.Select(m => m.TransformToManarock())
                    .Where(manaRock => requirementsTracker.DeckIdentity.CanInclude(manaRock.Identity)).ToList();

                manaRocks = manaRocks.OrderBy(l => l.Order).ToList();

                // create possible land picks
                var allLands = _csvReader.ReadFile<LandProperty>("lands").Select(l => l.TransformToLand()).ToList();
                var categories = _csvReader.ReadFile<Category>("categories");

                // remove excluded lands (user argument)
                if (excludedLands != null)
                {
                    allLands = allLands.Where(p => !excludedLands.Contains(p.Name)).ToList();
                }

                var lands = PossibleLands(requirementsTracker.DeckIdentity, allLands, categories);

                var manaSourcesSuggestion = CreateLandsSuggestion(requirementsTracker, manarockRatio, manaRocks, lands, requirementsTracker.DeckIdentity.Length);

                suggestion.Sources = manaSourcesSuggestion;
                
                suggestion.SourcesCount = 0;
                foreach (var s in suggestion.Sources)
                {
                    var count = s.Substring(0, s.IndexOf(" ", StringComparison.Ordinal));
                    suggestion.SourcesCount += Convert.ToInt32(count);
                }

                return suggestion;
            }
            catch (Exception e)
            {
                suggestion.Error = e.Message;
                return suggestion;
            }
        }

        private List<string> CreateLandsSuggestion(ColorSourceRequirementTracker requirementsTracker, ManarockRatio manarockRatio,
            List<Manarock> manaRocks, List<List<Land>> landChunks, int landChunkIndex)
        {
            var landsSuggestion = new List<Land>();
            var rocksSuggestion = new List<Manarock>();

            if (landChunkIndex > landChunks.Count)
                throw new Exception("mana base cant be generated, not enough lands with enough sources");

            var lands = landChunks[landChunkIndex-1];

            // 1st step: Add required amount of mana rocks to deck
            while (rocksSuggestion.Count < manarockRatio.ManaRocks)
            {
                var bestRock =
                    manaRocks.First(r => rocksSuggestion.All(x => x.Name != r.Name) && r.Produces.Contains(requirementsTracker.HighestColorRequirement));
                rocksSuggestion.Add(new Manarock(bestRock));

                foreach (var producedMana in bestRock.Produces)
                {
                    requirementsTracker.ReduceRequirement(producedMana);
                }
            }

            // 2nd step: Add one basic land of each color in deck identity
            foreach (var req in requirementsTracker.Requirements)
            {
                landsSuggestion.Add(GenerateBasicLand(req.Color));
                requirementsTracker.ReduceRequirement(req.Color);
            }


            // 3rd step: add lands to meet requirements
            while (requirementsTracker.TotalRequirementsCount() + landsSuggestion.Count > manarockRatio.Lands &&
                   lands.Any() && landsSuggestion.Count < manarockRatio.Lands)
            {
                var illegalRequirements = (from r in requirementsTracker.Requirements where r.IsFulfilled select r.Color).ToList();

                var l = lands.First(i => i.Produces.Except(illegalRequirements).Count() >= 2);
                landsSuggestion.Add(l);
                lands.Remove(l);
                foreach (var c in l.Produces)
                {
                    requirementsTracker.ReduceRequirement(c);
                }
            }

            // 4th step: fill up deck with basic lands if there is space left
            if (landsSuggestion.Count < manarockRatio.Lands)
            {
                foreach (var r in requirementsTracker.Requirements)
                {
                    var amount = r.Amount - r.AmountFulfilled;
                    for (var i = 0; i < amount; i++)
                    {
                        landsSuggestion.Add(GenerateBasicLand(r.Color));
                        requirementsTracker.ReduceRequirement(r.Color);
                    }
                }
            }

            if (requirementsTracker.AllRequirementsFulfilled)
            {
                var result = new List<string>();

                var groups = landsSuggestion.GroupBy(i => i.Name);
                foreach (var grp in groups)
                {
                    result.Add($"{grp.Count()} {grp.Key}");
                }
                result.AddRange(rocksSuggestion.Select(r => "1 " + r.Name));

                return result;
            }

            requirementsTracker.Reset();
            return CreateLandsSuggestion(requirementsTracker, manarockRatio, manaRocks, landChunks,
                landChunkIndex + 1);
        }

        private List<List<Land>> PossibleLands(char[] deckIdentity, List<Land> allLands, List<Category> categories)
        {
            var landChunks = new List<Land>[6];

            // fetch-able sources need to have one intersection with identity otherwise there is nothing to fetch
            var sameIdentityLands = allLands.Where(l =>
                deckIdentity.CanInclude(l.Identity) &&
                deckIdentity.Intersect(l.SingleMatch.ToLower().ToCharArray()).Any()).ToList();

            for (var i = 2; i <= 6; i++)
            {
                var landChunk = new List<Land>();

                foreach (var land in sameIdentityLands)
                {
                    var category =
                        categories.First(c =>
                            c.Cycle == land.Cycle); // there can only ever be one cycle unless data is faulty

                    land.Order = i switch
                    {
                        2 => category.TwoColor,
                        3 => category.ThreeColor,
                        4 => category.FourColor,
                        5 => category.FiveColor,
                        6 => category.ExtremeCase,
                        _ => land.Order
                    };

                    if (land.Order <= 0) continue;

                    landChunk.Add(land);
                }

                landChunks[i-1] = new List<Land>(landChunk.OrderBy(l => l.Order).ToList());
            }

            return landChunks.ToList();
            
        }

        private ColorSourceRequirementTracker InitializeSourceRequirements(List<ScryfallCard> cards, out List<string> errors)
        {
            errors = new List<string>();

            // get color requirements based on pips in deck (for example, 1BB requires higher count of sources than 2B)
            var colorPipsRequirements = _csvReader.ReadFile<ColorPipsRequirement>("pips");

            // get requirements
            var colorSourceRequirementsTracker = new ColorSourceRequirementTracker();
            foreach (var card in cards)
            {
                // ignoring free spells
                if (card.ManaCost == "{0}") continue;

                // ignore cards
                var colorPipsRequirement = colorPipsRequirements.Where(p => p.Cost == card.ManaValue).ToList();
                if (!colorPipsRequirement.Any())
                {
                    errors.Add($"Can't calculate color pip requirement for {card.Name}");
                    continue;
                }

                var manaCost = card.GetManaCost();

                // if mana costs contain a slash ignore them from now (hybrid mana cost) todo: implement hybrid mana cost calculations
                if (manaCost.Contains("/"))
                {
                    errors.Add($"Ignoring hybrid mana cost of {card.Name}");
                    continue;
                }

                foreach (var color in Const.Colors)
                {
                    var currentColor = manaCost.Count(m => m == color);
                    if (currentColor <= 0) continue; // no color found of this type
                    var currentPipsRequirement = colorPipsRequirement.Where(p => p.Pips == currentColor).ToList();
                    if (!currentPipsRequirement.Any() || currentPipsRequirement.Count != 1)
                    {
                        errors.Add($"Can't find pip requirement for {card.Name}");
                        continue;
                    }

                    var sources = currentPipsRequirement.First().Sources;
                    if (colorSourceRequirementsTracker.HasColor(color))
                    {
                        // if requirements for this color are already present, update if new value requires higher amount of sources
                        var previousValue = colorSourceRequirementsTracker.GetColorRequirementCount(color);
                        if (sources > previousValue)
                        {
                            colorSourceRequirementsTracker.SetColorRequirement(color, sources, card.Name);
                        }
                    }
                    else
                    {
                        // if requirement for this color is new, add it to list of requirements
                        colorSourceRequirementsTracker.SetColorRequirement(color, sources, card.Name);
                    }
                }
            }

            return colorSourceRequirementsTracker;
        }

        private ManarockRatio CalculateManarockRatio(List<ScryfallCard> cards, string[] ignoreAverageMv)
        {
            // get required land and manarock count, this will be derived from average mana value
            var manarockRatios = _csvReader.ReadFile<ManarockRatio>("manarockratios");

            var avgMv = cards.Where(x => !ignoreAverageMv.Contains(x.Name)).Average(c => c.Cmc);
            var manarockRatio = manarockRatios.FirstOrDefault(r => avgMv >= r.MinMv && avgMv <= r.MaxMv);
            if (manarockRatio == null) throw new Exception("can't derive lands and mana rocks requirement");

            return manarockRatio;
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
