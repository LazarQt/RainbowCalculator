using RainbowCore;
using RainbowCore.Extensions;
using RainbowCore.Model;
using RainbowModel.Scryfall;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public LandSuggestion[] Retrieve(string deckString)
        {
            return new ManabaseCalculator().Calculate(deckString);
        }
    }
}