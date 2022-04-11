using RainbowCalculator.Model;

namespace RainbowCalculator.Extensions
{
    public static class LandExtensions
    {
        public static List<char> ProduceColors(this Land land)
        {
            var res = new List<char>();

            foreach (var c in land.Produces.ToLower().Replace("c", "").ToArray())
            {
                res.Add(c);
            }

            return res;
        }

        public static bool CanProduce(this Land land, IEnumerable<char> colors)
        {
            return (land.ProduceColors().Any(c => colors.Contains(c)));
        }

        public static bool ProducesAll(this Land land, IEnumerable<char> colors)
        {
            return (land.ProduceColors().All(c => colors.Contains(c)));
        }

        public static bool DoesNotProduce(this Land land, IEnumerable<char> colors)
        {
            return (land.ProduceColors().All(c => !colors.Contains(c)));
        }
    }
}
