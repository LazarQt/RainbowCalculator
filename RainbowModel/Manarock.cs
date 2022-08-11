namespace RainbowModel
{
    public class Manarock
    {
        public Manarock()
        {
            
        }

        public Manarock(Manarock manarock)
        {
            Name = manarock.Name;
            Identity = manarock.Identity;
            Produces = manarock.Produces;
            Order = manarock.Order;
        }

        public string Name { get; set; }
        public char[] Identity { get; set; }
        public char[] Produces { get; set; }
        public float Order { get; set; }
    }
}
