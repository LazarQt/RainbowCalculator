using RainbowCore;

namespace RainbowCalculator
{
    public interface IManabaseProvider
    {
        ManabaseSuggestion Retrieve(string[] deck, string[] excludedLands, string[] ignoreAverageMv);
    }
}