using RainbowCore;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        /// <summary>
        /// Return suggestion how mana base should look like based on provided deck list.
        /// </summary>
        /// <param name="deckString">Entire deck in a single string</param>
        /// <param name="excludedLands">Lands to be manually excluded</param>
        /// <returns>Mana base suggestion and other information regarding deck composition</returns>
        public ManabaseSuggestion Retrieve(string deckString, string excludedLands) => new ManabaseCalculator().Calculate(deckString, excludedLands);
    }
}