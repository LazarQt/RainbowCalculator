using System.Globalization;
using System.Reflection;
using RainbowModel;

namespace RainbowCore
{
    public class CsvReader
    {
        public List<T> ReadFile<T>(string file) where T : ICsvProperty
        {
            var workPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (workPath == null) throw new Exception("Execution path cannot be determined"); // this should be impossible to happen

            var filename = Path.Combine(workPath, "CalcFiles", $"{file}.csv");
            using var reader = new StreamReader(filename);
            using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }
    }
}
