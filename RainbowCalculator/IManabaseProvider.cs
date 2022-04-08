namespace RainbowCalculator
{
    public interface IManabaseProvider
    {
        Land[] Retrieve(string deckString);
    }
}