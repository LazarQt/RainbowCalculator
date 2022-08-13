using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class ManarockPropertyExtensions
    {
        /// <summary>
        /// Create a mana rock based on mana rock property class
        /// </summary>
        /// <param name="manarockProperty">Mana rock property</param>
        /// <returns>Mana rock</returns>
        public static Manarock TransformToManarock(this ManarockProperty manarockProperty)
        {
            var manarock = new Manarock()
            {
                Name = manarockProperty.Name,
                Order = manarockProperty.Order,
                Identity = manarockProperty.Identity.ToLower().ToCharArray(),
                Produces = manarockProperty.Produces.ToLower().ToCharArray()
            };

            return manarock;
        }
    }
}
