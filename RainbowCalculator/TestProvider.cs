namespace RainbowCalculator
{
    public class TestProvider : ITestProvider
    {
        public User[] Get(string x)
        {
            return new[] { new User() { Id = 1, Name = "my name is" + x } };
        }
    }
}
