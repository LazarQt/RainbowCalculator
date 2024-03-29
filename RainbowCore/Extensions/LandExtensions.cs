﻿using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class LandExtensions
    {
        /// <summary>
        /// Check if land is not able to produce a set of colors
        /// </summary>
        /// <param name="land">Land to be checked</param>
        /// <param name="colors">Colors that may or may not be produced</param>
        /// <returns>True or false depending on whether color can be generated by any means</returns>
        public static bool DoesNotProduce(this Land land, IEnumerable<char> colors) => land.Produces.All(c => !colors.Contains(c));
    }
}
