namespace RainbowModel
{
    public class LandProperty : ICsvProperty
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Produces { get; set; }
        public string SingleMatch { get; set; }
        public string Set { get; set; }
        public string Cycle { get; set; }
    }
}
