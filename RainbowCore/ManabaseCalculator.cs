using CsvHelper;
using RainbowCore.Extensions;
using RainbowModel;
using RainbowModel.Scryfall;
using static RainbowCore.Const;

namespace RainbowCore
{
    public class ManabaseCalculator
    {
        private readonly CsvReader _csvReader;

        public ManabaseCalculator()
        {
            _csvReader = new CsvReader();
        }

        public LandSuggestion[] Calculate(string deckString)
        {
            // testing

            // mono color low mana value
            deckString = "";
            for (var i = 0; i <= 60; i++)
            {
                deckString += "Brainstorm|";
            }

            // mono color high mana value
            deckString = "";
            for (var i = 0; i <= 60; i++)
            {
                deckString += "Gigantosaurus|";
            }

            // edric
            deckString = "|Artificer's Assistant |Bad River |Barkchannel Pathway |Beastmaster Ascension |Bident of Thassa |Biomass Mutation |Botanical Sanctum |Breeding Pool |Caustic Caterpillar |Champion of Lambholt |Chasm Skulker |Cloud of Faeries |Cloud Pirates |Cloudfin Raptor |Coastal Piracy |Command Tower |Counterspell |Dowsing Dagger |Druids' Repository |Dryad Sophisticate |Edric, Spymaster of Trest |Elvish Mystic |Fabled Passage |Flying Men |Foil |Fyndhorn Elves |Grand Coliseum |Grasslands |Growing Rites of Itlimoc |Gudul Lurker |Heroic Intervention |Hinterland Harbor |Hope of Ghirapur |Horizon Chimera |Hypnotic Siren |Invisible Stalker |Jace's Phantasm |Jhessian Infiltrator |Jolrael, Mwonvuli Recluse |Kappa Tech-Wrecker |Lantern Bearer |Llanowar Elves |Looter il-Kor |Mausoleum Wanderer |Merfolk Windrobber |Mindshrieker |Mist-Cloaked Herald |Mountain Valley |Nature's Will |Nightveil Sprite |Notorious Throng |Part the Waterveil |Path of Ancestry |Pteramander |Ranger Class |Reconnaissance Mission |Rejuvenating Springs |Reliquary Tower |Shimmerdrift Vale |Silhana Ledgewalker |Silver Raven |Siren Stormtamer |Slither Blade |Spectral Sailor |Strixhaven Stadium |Sylvan Safekeeper |Temporal Trespass |Tetsuko Umezawa, Fugitive |Throne of the God-Pharaoh |Time Stop |Time Warp |Toski, Bearer of Secrets |Treetop Scout |Triton Shorestalker |Trygon Predator |Unified Will |Vineglimmer Snarl |Walk the Aeons |Waterlogged Grove |Wizard Class |Yavimaya Coast";
            deckString = "|Carpet of Flowers |Wild Growth |Natural Order |Rhystic Study |Sylvan Library |Howling Moon |Mana Vault |Sol Ring |Chrome Mox |Mana Crypt |Oko, Thief of Crowns |Nissa, Who Shakes the World |Garruk Wildspeaker |Mystic Remora |Karn's Temporal Sundering |Force of Vigor |Wash Away |Alrund's Epiphany |Walk the Aeons |Temporal Manipulation |Time Warp |Temporal Mastery |Green Sun's Zenith |Notorious Throng |Fierce Guardianship |Mystical Tutor |Submerge |Noxious Revival |Pongify |Flusterstorm |Mindbreak Trap |Rapid Hybridization |Force of Negation |Daze |An Offer You Can't Refuse |Crop Rotation |Misdirection |Force of Will |Cyclonic Rift |Swan Song |Mental Misstep |Endurance |Elvish Mystic |Jolrael, Mwonvuli Recluse |Arbor Elf |Boreal Druid |Ledger Shredder |Malevolent Hermit |Llanowar Elves |Master of the Wild Hunt |Craterhoof Behemoth |Elvish Spirit Guide |Oakhame Adversary |Toski, Bearer of Secrets |Manglehorn |Arasta of the Endless Web |Priest of Titania |Birds of Paradise |Lotus Cobra |Ghostly Pilferer |Sakura-Tribe Elder |Fyndhorn Elves |Tendershoot Dryad |Koma, Cosmos Serpent |Collector Ouphe |Sakura-Tribe Scout |Bloom Tender |Joraga Treespeaker |Prismatic Vista |Yavimaya Coast |Polluted Delta |Breeding Pool |Otawara, Soaring City |Command Tower |Waterlogged Grove |Mystic Sanctuary |Urza's Saga |Boseiju, Who Endures |Boseiju, Who Shelters All |Yavimaya, Cradle of Growth |Rejuvenating Springs |Windswept Heath |Mana Confluence |Wooded Foothills |Wirewood Lodge |Verdant Catacombs |Flooded Strand |Misty Rainforest |Dryad Arbor |Gemstone Caverns |Ancient Tomb |Scalding Tarn |Forest |Island |Edric, Spymaster of Trest ";
            deckString = "|Aurification |Axis of Mortality |Battlefield Forge |Blightstep Pathway |Blood Baron of Vizkopa |Bomb Squad |Boros Locket |Boros Signet |Bounty Agent |Bounty Hunter |Brightclimb Pathway |Caves of Koilos |Children of Korlis |Clifftop Retreat |Command Tower |Commander's Sphere |Concealed Courtyard |Devout Witness |Dragonskull Summit |Dread |Dwarven Miner |Enduring Angel |Exquisite Blood |Fabled Passage |Fire Covenant |Fires of Invention |Goblin Spymaster |Godless Shrine |Gorilla Shaman |Grand Abolisher |Haunted Ridge |High Priest of Penance |Hoard Robber |Idyllic Tutor |Inspiring Vantage |Island Sanctuary |Isolated Chapel |Kambal, Consul of Allocation |Luminarch Ascension |Maddening Imp |Master of Cruelties |Mathas, Fiend Seeker |Michiko Konda, Truth Seeker |Mortuary |Mother of Runes |Mountain |Necropotence |Needleverge Pathway |No Mercy |Nomad Outpost |Notorious Assassin |Ophiomancer |Oracle en-Vec |Orzhov Locket |Orzhov Signet |Phyrexian Arena |Plaguebearer |Plains |Price of Glory |Queen Marchesa |Rakdos Signet |Reliquary Tower |Royal Assassin |Rune-Tail, Kitsune Ascendant |Sacred Foundry |Savai Triome |Season of the Witch |Shadowblood Ridge |Silent Clearing |Skyline Despot |Smoldering Marsh |Smothering Tithe |Sol Ring |Solitary Confinement |Spectator Seating |Sulfurous Springs |Swamp |Talisman of Hierarchy |Temple of Malice |Teysa, Envoy of Ghosts |Torment of Hailfire |Tsabo's Assassin |Vault of Champions |Venser's Journal |Viashino Heretic |Vindicate |War's Toll |Worship";
            deckString = "|Arcane Sanctum |Argothian Wurm |Assemble the Legion |Bend or Break |Biomancer's Familiar |Boneyard Parley |Brash Taunter |Breena, the Demagogue |Canopy Vista |Capital Punishment |Captive Audience |Choice of Damnations |City of Brass |Clackbridge Troll |Clifftop Retreat |Command Tower |Cruel Entertainment |Curse of the Cabal |Death or Glory |Deserted Beach |Diaochan, Artful Beauty |Do or Die |Dryad of the Ilysian Grove |Epiphany at the Drownyard |Evolving Wilds |Exotic Orchard |Fabled Passage |Faeburrow Elder |Forbidden Orchard |Forest |Game Plan |Gang Up |Grand Coliseum |Haunted Ridge |Head Games |Humble Defector |Hunted Dragon |Hunted Horror |Hunted Lammasu |Hunted Phantasm |Hunted Troll |Hushbringer |Island |Isolated Chapel |Jungle Shrine |Kenrith, the Returned King |Kynaios and Tiro of Meletis |Kyodai, Soul of Kamigawa |Mana Confluence |Mountain |Nomad Outpost |Nyxbloom Ancient |Opulent Palace |Orzhov Advokist |Overgrown Farmland |Path of Ancestry |Pir's Whim |Plains |Prairie Stream |Rainbow Vale |Reflecting Pool |Scheming Symmetry |Secret Rendezvous |Selvala, Explorer Returned |Shadrix Silverquill |Shivan Wumpus |Smoldering Marsh |Smothering Tithe |Sol Ring |Split Decision |Steam Augury |Sulfur Falls |Swamp |Sylvan Offering |Tainted Remedy |Tempt with Discovery |The World Tree |Truth or Tale |Wheel and Deal |Wishclaw Talisman |Woodland Cemetery |Zirda, the Dawnwaker";
            deckString = "|Adeline, Resplendent Cathar |Akroma's Will |Alesha, Who Smiles at Death |Anafenza, the Foremost |Anguished Unmaking |Arcane Signet |Arid Mesa |Assassin's Trophy |Augur of Autumn |Aurelia, the Warleader |Avacyn's Pilgrim |Beast Within |Beastmaster Ascension |Birds of Paradise |Blood Crypt |Bloodstained Mire |Boros Charm |Boros Signet |Champion of Lambholt |Chromatic Lantern |Command Tower |Commander's Sphere |Cultivate |Demonic Tutor |Domri, Anarch of Bolas |Esper Sentinel |Eternal Witness |Evolving Wilds |Exotic Orchard |Faeburrow Elder |Farseek |Fellwar Stone |Flawless Maneuver |Forest |General Kudro of Drannith |General's Enforcer |Gisela, Blade of Goldnight |Godless Shrine |Gruul Signet |Guardian Project |Halana and Alena, Partners |Heroic Intervention |Heronblade Elite |Ignoble Hierarch |Indatha Triome |Iroas, God of Victory |Isshin, Two Heavens as One |Jetmir's Garden |Jungle Shrine |Karlach, Fury of Avernus |Katilda, Dawnhart Prime |Kodama's Reach |Kyler, Sigardian Emissary |Lightning Greaves |Marisi, Breaker of the Coil |Marsh Flats |Mirari's Wake |Mountain |Nature's Lore |Nomad Outpost |Odric, Lunarch Marshal |Overgrown Tomb |Path to Exile |Plains |Rampant Growth |Rhythm of the Wild |Ruinous Ultimatum |Sacred Foundry |Sakura-Tribe Elder |Samut, Voice of Dissent |Sandsteppe Citadel |Saryth, the Viper's Fang |Saskia the Unyielding |Savage Lands |Savai Triome |Shared Animosity |Sigarda, Champion of Light |Smothering Tithe |Sol Ring |Stomping Ground |Swamp |Swords to Plowshares |Sylvan Library |Temple Garden |Thalia, Heretic Cathar |Three Visits |Torens, Fist of the Angels |Toski, Bearer of Secrets |Tymna the Weaver |Verdant Catacombs |Windswept Heath |Wooded Foothills |Ziatora's Proving Ground";

            // testing end

            var cards = BuildCalculationDeck(deckString);
            var manarockRatio = CalculateManarockRatio(cards);
            var sourceRequirements = CalculateSourceRequirements(cards);

            if (sourceRequirements.Count < 2)
            {
                // mono colored decks
            }

            // create possible land picks
            var landProperties = _csvReader.ReadFile<LandProperty>("lands");
            var categories = _csvReader.ReadFile<Category>("categories");

            var lands = new List<Land>();
            var deckIdentity = sourceRequirements.Keys.ToArray();
            foreach (var p in landProperties)
            {
                var land = p.TransformToLand();
                if (!deckIdentity.CanInclude(land.Identity)) continue;

                // fetch-able sources need to have one intersection with identity otherwise there is nothing to fetch
                if (!deckIdentity.Intersect(p.SingleMatch.ToLower().ToCharArray()).Any())
                    continue;

                var category = categories.First(c => c.Cycle == land.Cycle); // there can only ever be one cycle unless data is faulty

                land.Order = deckIdentity.Length switch
                {
                    2 => category.TwoColor,
                    3 => category.ThreeColor,
                    4 => category.FourColor,
                    5 => category.FiveColor,
                    _ => land.Order
                };

                // ignore from ignore list // todo: user setting
                if (category.Cycle == "True Duals") continue;
                if (category.Cycle == "Fetch Lands") continue;

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

            // first iteration: populate mana rocks
            while (rocksSuggestion.Count < manarockRatio.ManaRocks)
            {
                var n = sourceRequirements.MaxBy(s => s.Value).Key;
                var bestRock = manaRocks.First(r => r.Produces.Contains(n));
                rocksSuggestion.Add(new Manarock(bestRock));

                foreach (var producedMana in bestRock.Produces)
                {
                    if (sourceRequirements.ContainsKey(producedMana))
                    {
                        sourceRequirements[producedMana] -= 1;
                    }
                }
                manaRocks.Remove(bestRock);
            }

            // 1.5 iteration: add one single basic land of each color
            foreach (var req in sourceRequirements)
            {
                landsSuggestion.Add(GetBasic(req.Key));
                sourceRequirements[req.Key] -= 1;

            }


            // second iteration: add lands
            while (sourceRequirements.Values.Sum(v => v) + landsSuggestion.Count > manarockRatio.Lands && lands.Any() && landsSuggestion.Count < manarockRatio.Lands)
            {
                var illegalRequirements = new List<char>();
                foreach (var r in sourceRequirements)
                {
                    if(r.Value <= 0) 
                        illegalRequirements.Add(r.Key);
                }
                var l = lands.First(i => i.DoesNotProduce(illegalRequirements));
                landsSuggestion.Add(l);
                lands.Remove(l);
                foreach (var c in l.Produces)
                {
                    if (sourceRequirements.ContainsKey(c))
                    {
                        sourceRequirements[c] -= 1;
                    }
                }
            }

            // third iteration: equalizer
            

            //while (sourceRequirements.Sum(s => s.Value) >= 0)
            //{
            //    var highReq = sourceRequirements.MaxBy(i => i.Value).Key;
            //    var minReq = sourceRequirements.MinBy(i => i.Value).Key;

            //    var land = lands.FirstOrDefault(l => l.CanProduce(highReq) && l.CanProduce(minReq));
            //    if (land == null) throw new Exception("not enough lands!");
            //    landsSuggestion.Add(land);
            //    lands.Remove(land);
            //    foreach (var c in land.Produces)
            //    {
            //        if (sourceRequirements.ContainsKey(c))
            //        {
            //            sourceRequirements[c] -= 1;
            //        }
            //    }
            //}

            // last step: add basics to fill up
            foreach (var r in sourceRequirements)
            {
                for (var i = 0; i < r.Value; i++)
                {
                    landsSuggestion.Add(GetBasic(r.Key));
                }
            }
  



            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0, 1) } };
        }

        private Dictionary<char, int> CalculateSourceRequirements(List<ScryfallCard> cards)
        {
            // get color requirements based on pips in deck (for example, 1BB requires higher count of sources than 2B)
            var colorPipsRequirements = _csvReader.ReadFile<ColorPipsRequirement>("pips");

            // get requirements
            var sourceRequirements = new Dictionary<char, int>(); // how many sources of each color are needed
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
                    if (sourceRequirements.ContainsKey(color))
                    {
                        // if requirements for this color are already present, update if new value requires higher amount of sources
                        var previousValue = sourceRequirements[color];
                        if (sources > previousValue)
                        {
                            sourceRequirements[color] = sources;
                        }
                    }
                    else
                    {
                        // if requirement for this color is new, add it to list of requirements
                        sourceRequirements[color] = sources;
                    }
                }
            }

            return sourceRequirements;
        }

        private ManarockRatio CalculateManarockRatio(List<ScryfallCard> cards)
        {
            // get required land and manarock count, this will be derived from average mana value
            var manarockRatios = _csvReader.ReadFile<ManarockRatio>("manarockratios");

            var avgMv = cards.Average(c => c.Cmc);
            var manarockRatio = manarockRatios.FirstOrDefault(r => avgMv >= r.MinMv && avgMv <= r.MaxMv);
            if (manarockRatio == null) throw new Exception("can't derive lands and mana rocks requirement");
            Logger.Log($"Average MV: {avgMv}");

            return manarockRatio;
        }

        private List<ScryfallCard> BuildCalculationDeck(string deckString)
        {
            // these cards are strictly excluded from calculation because they have X in costs or are regularly reduced in costs
            var excludedCards = _csvReader.ReadFile<SingleLine>("exclude");

            // get actual cards from deck string
            var cards = new CardsParser().GetCards(deckString, out var missing);
            // report missing cards
            Console.WriteLine($"Missing: {string.Join(",", missing)}");
            Logger.Log("Missing cards", missing); // todo: this information should be reported to the user at some point

            // remove all excluded cards which are not part of the equation
            var excluded = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (excludedCards.Any(x => x.Name == c.Name.ToLower()))
                {
                    excluded.Add(c.Name);
                    cards.RemoveAt(i);
                }
            }
            Console.WriteLine($"Excluded: {string.Join(",", excluded)}");
            Logger.Log("Removed due to X etc.", excluded);  // todo: this information should be reported to the user at some point

            // remove lands (if user gave list with lands in it already)
            var removedLands = new List<string>();
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var c = cards[i];
                if (!c.TypeLine.ToLower().Contains(Const.Card.Land.ToLower())) continue;
                removedLands.Add(c.Name);
                cards.RemoveAt(i);
            }
            Console.WriteLine($"Removed lands: {string.Join(",", removedLands)}");
            Logger.Log("Removed lands (why give lands with calculation?)", removedLands); // todo: this information should be reported to the user at some point

            return cards;
        }

        private Land GetBasic(char k)
        {
            if (k == 'w') return new Land() { Identity = new[] { 'w' }, Name = "Plains", Produces = new[] { 'w' } };
            if (k == 'u') return new Land() { Identity = new[] { 'u' }, Name = "Island", Produces = new[] { 'u' } };
            if (k == 'b') return new Land() { Identity = new[] { 'b' }, Name = "Swamp", Produces = new[] { 'b' } };
            if (k == 'r') return new Land() { Identity = new[] { 'r' }, Name = "Mountain", Produces = new[] { 'r' } };
            if (k == 'g') return new Land() { Identity = new[] { 'g' }, Name = "Forest", Produces = new[] { 'g' } };
            throw new Exception("no basic found with type" + k);
        }
    }
}
