using RainbowModel.Scryfall;
using System.Text.RegularExpressions;

namespace RainbowCore.Extensions
{
    public static class ScryfallCardExtensions
    {
        /// <summary>
        /// Get string representation of mana cost
        /// </summary>
        /// <param name="card">Card to derive mana cost from</param>
        /// <returns>Mana cost as string</returns>
        public static string GetManaCost(this ScryfallCard card)
        {
            var manaCost = card.ManaCost;
            if (manaCost == null)
            {
                manaCost = card.CardFaces.First().ManaCost;
            }
            manaCost = manaCost.ToLower();

            // remove split cards 
            if (manaCost.Contains("//"))
            {
                manaCost = manaCost.Substring(0, manaCost.IndexOf("//", StringComparison.Ordinal));
            }
            // todo: might have to deal with this later and include the split card thingies

            return manaCost;
        }

        public static bool IsThis(this ScryfallCard card, string cardName)
        {
            var removeNonAlphanumerics = new Regex("[^a-zA-Z0-9 -]");

            var originalCardName = removeNonAlphanumerics.Replace(card.Name.ToLower().Trim(), string.Empty);
            var searchTerm = removeNonAlphanumerics.Replace(cardName.ToLower().Trim(), string.Empty);

            return originalCardName == searchTerm;
        }

        public static bool IsThis(this string cardName, ScryfallCard card) => card.IsThis(cardName);

        public static bool IsLand(this ScryfallCard card) => card.TypeLine.ToLower().Contains(Const.Card.Land.ToLower());
    }
}
