using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class LandExtensions
    {
        public static bool DoesNotProduce(this Land land, IEnumerable<char> colors)
        {
            return (land.Produces.All(c => !colors.Contains(c)));
        }
    }
}
