using RainbowModel;

namespace RainbowCore.Extensions
{
    public static class ManarockPropertyExtensions
    {
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
