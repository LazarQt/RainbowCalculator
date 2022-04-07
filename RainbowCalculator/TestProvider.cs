namespace RainbowCalculator
{
    public class TestProvider : ITestProvider
    {
        public User[] Get()
        {
            return new[] { new User() { Id = 1, Name = "asdfxd" } };
        }
    }
}
