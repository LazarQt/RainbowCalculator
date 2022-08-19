namespace RainbowModel
{
    public class ColorSourceRequirement
    {
        public char Color { get; set; }
        public int Amount { get; set; }
        public int AmountFulfilled { get; set; }
        public bool IsFulfilled => AmountFulfilled >= Amount;
        public string CardResponsible { get; set; }
    }
}
