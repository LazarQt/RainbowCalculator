namespace RainbowCalculator
{
    public interface IManabaseProvider
    {
        LandSuggestion[] Retrieve(string deckString);
    }
}