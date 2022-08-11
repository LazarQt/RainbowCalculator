using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class LandPropertyExtensions
    {
        public static Land TransformToLand(this LandProperty landCategory)
        {
            var land = new Land()
            {
                Name = landCategory.Name,
                Cycle = landCategory.Cycle,
                Identity = landCategory.Identity.ToLower().ToCharArray(),
                Produces = landCategory.Produces.ToLower().ToCharArray()
            };

            return land;
        }

        public static bool CanInclude(this char[] parentIdentity, char[] identity)
        {
            return !identity.Except(parentIdentity).Any();
        }
    }
}
