namespace RainbowCore.Model
{
    public class Land
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Produces { get; set; }
        public double Order { get; set; }
        public int Cutoff { get; set; }
        public bool Exclude { get; set; }
    }

    public class LandProperty
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Produces { get; set; }
        public string Set { get; set; }
        public string Cycle { get; set; }
    }
}
