// ReSharper disable UnusedMember.Global
namespace RainbowModel
{
    public class ManarockRatio : ICsvProperty
    {
        public int LandsWithoutRocks { get; set; }
        public double MinMv { get; set; }
        public double MaxMv { get; set; }
        public int ManaRocks { get; set; }
        public int Lands { get; set; }
    }
}
