using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class LandExtensions
    {
        //public static List<char> ProduceColors(this Land land)
        //{
        //    return land.Produces.ToLower().Replace("c", "").ToArray().ToList();
        //}

        public static bool CanProduce(this Land land, char color)
        {
            return land.CanProduce(new [] { color });
        }

        public static bool CanProduce(this Land land, IEnumerable<char> colors)
        {
            return (land.Produces.Any(colors.Contains));
        }

        //public static bool ProducesAll(this Land land, IEnumerable<char> colors)
        //{
        //    return (land.ProduceColors().All(colors.Contains));
        //}

        public static bool DoesNotProduce(this Land land, IEnumerable<char> colors)
        {
            return (land.Produces.All(c => !colors.Contains(c)));
        }
    }
}
