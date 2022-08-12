using RainbowModel;

namespace RainbowCore
{
    public class ManabaseSuggestion
    {
        public string Report { get; set; }
        public string Name { get; set; }
        public List<string> Lands { get; set; }
        public List<ColorSourceRequirement> ColorRequirements { get; set; }
        public ManarockRatio ManarockRatio { get; set; }
    }
}
