namespace RainbowCore
{
    public static class Logger
    {
        public static void Log(string title, List<string> items)
        {
            Log($"{title}: {string.Join(',', items)}");
        }

        public static void Log(string text)
        {
            File.WriteAllText("LogFile.txt", $"{text}{Environment.NewLine}");
        }
    }
}
