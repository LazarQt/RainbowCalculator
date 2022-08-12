using RainbowCore;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public ManabaseSuggestion Retrieve(string deckString)
        {
            return new ManabaseCalculator().Calculate(deckString);
        }
    }
}