using RainbowModel;

namespace RainbowCore
{
    public class ColorSourceRequirementTracker
    {
        public List<ColorSourceRequirement> Requirements { get; }

        public ColorSourceRequirementTracker()
        {
            Requirements = new List<ColorSourceRequirement>();
        }

        public void ReduceRequirement(char color, int? amount = null)
        {
            var req = Get(color);
            if (req == null) return;
            if (amount != null)
            {
                req.Amount -= (int)amount;
            }
            else
            {
                req.Amount -= 1;
            }
        }

        public void SetColorRequirement(char color, int amount)
        {
            var existingEntry = Get(color);
            if (existingEntry == null)
            {
                Requirements.Add(new ColorSourceRequirement()
                {
                    Color = color,
                    Amount = amount
                });
            }
            else
            {
                existingEntry.Amount = amount;
            }
        }

        public int TotalRequirementsCount => Requirements.Sum(r => r.Amount);

        public char[] DeckIdentity => Requirements.Select(r => r.Color).ToArray();

        public char HighestColorRequirement => Requirements.OrderBy(r => r.Amount).First().Color;

        public bool HasColor(char color) => Requirements.Any(r => r.Color == color);

        public int GetColorRequirementCount(char color) => Get(color).Amount;

        private ColorSourceRequirement Get(char color) => Requirements.FirstOrDefault(r => r.Color == color);
    }
}
