using RainbowModel;

namespace RainbowCore
{
    public class ManabaseSuggestion
    {
        public List<string> CardsNotFound { get; set; }
        public List<string> ExcludedCards { get; set; }
        public List<string> RemovedLands { get; set; }
        public List<string> Lands { get; set; }
        public List<ColorSourceRequirement> ColorRequirements { get; set; }
        public ManarockRatio ManarockRatio { get; set; }
    }
}
