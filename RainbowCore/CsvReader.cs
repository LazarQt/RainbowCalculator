using System.Globalization;
using System.Reflection;
using RainbowModel;

namespace RainbowCore
{
    public class CsvReader
    {
        public List<T> ReadFile<T>(string file) where T : ICsvProperty
        {
            var strWorkPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (strWorkPath == null)
            {
                throw new Exception("Execution path cannot be determined"); // this should be impossible to happen
            }

            string filename = Path.Combine(strWorkPath, "CalcFiles", $"{file}.csv");
            using var reader = new StreamReader(filename);
            using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }
    }
}
