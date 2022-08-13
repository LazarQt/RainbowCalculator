using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class LandPropertyExtensions
    {
        /// <summary>
        /// Transform land property to land class
        /// </summary>
        /// <param name="landCategory">Land category</param>
        /// <returns>Land object</returns>
        public static Land TransformToLand(this LandProperty landCategory)
        {
            var land = new Land
            {
                Name = landCategory.Name,
                Cycle = landCategory.Cycle,
                Identity = landCategory.Identity.ToLower().ToCharArray(),
                Produces = landCategory.Produces.ToLower().ToCharArray()
            };

            return land;
        }

        /// <summary>
        /// Checks if card can be included in deck given the deck identity
        /// </summary>
        /// <param name="parentIdentity">Deck identity</param>
        /// <param name="identity">Card identity</param>
        /// <returns>True or false depending on whether card can be included or not</returns>
        public static bool CanInclude(this char[] parentIdentity, char[] identity) => !identity.Except(parentIdentity).Any();
    }
}
