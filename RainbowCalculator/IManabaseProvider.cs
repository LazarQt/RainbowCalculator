using RainbowCore;

namespace RainbowCalculator
{
    public interface IManabaseProvider
    {
        ManabaseSuggestion Retrieve(string deckString, string excludedLands);
    }
}