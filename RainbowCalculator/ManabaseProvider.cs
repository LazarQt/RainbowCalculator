﻿using RainbowCore;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        /// <summary>
        /// Return suggestion how mana base should look like based on provided deck list.
        /// </summary>
        /// <param name="deck">Entire deck array</param>
        /// <param name="excludedLands">Lands to be manually excluded</param>
        /// <param name="ignoreAverageMv">Exclude from average mv calculation</param>
        /// <returns>Mana base suggestion and other information regarding deck composition</returns>
        public ManabaseSuggestion Retrieve(string[] deck, string[] excludedLands, string[] ignoreAverageMv) => new ManabaseCalculator().Calculate(deck, excludedLands, ignoreAverageMv);
    }
}