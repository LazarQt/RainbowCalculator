using RainbowCalculator.Model;
using RainbowCalculator.Util;

namespace RainbowCalculator
{
    public class ManabaseProvider : IManabaseProvider
    {
        public LandSuggestion[] Retrieve(string deckString)
        {
            List<Category> categories = new List<Category>();
            var categoryLines = CsvUtil.ReadLines(@"Categories.csv");
            foreach(var line in categoryLines)
            {
                categories.Add(new Category()
                {
                    Cycle = line[0],
                    Enters = line[1],
                    Order = Convert.ToDouble(line[2]),
                    Cutoff = Convert.ToInt32(line[3])
                });
            }

            List<Model.Land> lands = new List<Model.Land>();
            var landLines = CsvUtil.ReadLines(@"Lands_Filtered.csv");
            foreach (var line in landLines)
            {
                var category = categories.FirstOrDefault(c => c.Cycle == line[4]);
                if (category == null) throw new Exception("unknown land category");
                lands.Add(new Model.Land()
                {
                    Name = line[0],
                    Identity = line[1],
                    Produces = line[2],
                    Order = category.Order,
                    Cutoff = category.Cutoff
                });
            }

            return new[] { new LandSuggestion() { Name = "my name is" + deckString.Substring(0,1) } };
        }
    }
}
