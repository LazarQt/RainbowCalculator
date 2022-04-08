namespace RainbowCalculator.Util
{
    public static class CsvUtil
    {
        public static List<string[]> ReadLines(string path)
        {
            List<string[]> lines = new List<string[]>();
            using (var reader = new StreamReader(@path))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine().Split(','));
                }
            }
            return lines;
        }
    }
}
