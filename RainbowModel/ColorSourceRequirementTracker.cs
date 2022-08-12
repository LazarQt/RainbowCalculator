using System.Drawing;

namespace RainbowModel
{
    public class ColorSourceRequirementTracker
    {
        public List<ColorSourceRequirement> Requirements { get; }

        public ColorSourceRequirementTracker()
        {
            Requirements = new List<ColorSourceRequirement>();
        }

        public int TotalRequirementsCount => Requirements.Sum(r => r.Amount);

        public char[] DeckIdentity => Requirements.Select(r => r.Color).ToArray();

        public char HighestColorRequirement => Requirements.OrderBy(r => r.Amount).First().Color;

        public void ReduceRequirement(char color, int? amount = null)
        {
            //Validate(color);
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

        private void Validate(char color)
        {
            if (Requirements.All(r => r.Color != color)) throw new Exception("Calling non existent color");
        }

        public bool HasColor(char color) => Requirements.Any(r => r.Color == color);

        public int GetColorRequirementCount(char color) => Get(color).Amount;

        private ColorSourceRequirement Get(char color)
        {
            //Validate(color);
            return Requirements.FirstOrDefault(r => r.Color == color);
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
    }
}
