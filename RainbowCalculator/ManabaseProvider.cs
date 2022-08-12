using RainbowCore;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public ManabaseSuggestion Retrieve(string deckString, string excludedLands)
        {
            return new ManabaseCalculator().Calculate(deckString, excludedLands);
        }
    }
}