using RainbowModel;

namespace RainbowCore
{
    public class ManabaseSuggestion
    {
        public string Error { get; set; }
        public string AverageManaValue { get; set; }
        public List<string> RelevantCardList { get; set; }
        public List<string> CardsNotFound { get; set; }
        public List<string> ExcludedCards { get; set; }
        public List<string> RemovedLands { get; set; }
        public List<string> Sources { get; set; }
        public int SourcesCount { get; set; }
        public List<string> ColorRequirementsErrors { get; set; }
        public List<ColorSourceRequirement> ColorRequirements { get; set; }
        public ManarockRatio ManarockRatio { get; set; }
    }
}
