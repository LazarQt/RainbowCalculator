using RainbowCore;
using RainbowCore.Extensions;
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