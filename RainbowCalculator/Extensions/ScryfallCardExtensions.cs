using RainbowCalculator.Model;

namespace RainbowCalculator.Extensions
{
    public static class ScryfallCardExtensions
    {
        public static string GetManaCost(this ScryfallCard card)
        {
            var manaCost = card.ManaCost;
            if (manaCost == null)
            {
                manaCost = card.CardFaces.First().ManaCost;
            }
            manaCost = manaCost.ToLower();

            // remove eldrain split card thingies
            if (manaCost.Contains("//"))
            {
                manaCost = manaCost.Substring(0, manaCost.IndexOf("//"));
            }
            // todo: might have to deal with this later and include in calculation eldraine cards

            return manaCost;
        }
    }
}
