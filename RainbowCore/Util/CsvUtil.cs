using System.Reflection;

namespace RainbowCore.Util
{
    public static class CsvUtil
    {
        public static List<string[]> ReadLines(string path)
        {
            List<string[]> lines = new List<string[]>();
            var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var reader = new StreamReader(Path.Combine(executableLocation, path)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine()?.Split(','));
                }
            }
            return lines;
        }
    }
}