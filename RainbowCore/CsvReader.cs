using System.Globalization;
using System.Reflection;
using RainbowModel;

namespace RainbowCore
{
    public class CsvReader
    {
        /// <summary>
        /// Reads csv file and create appropriate class objects
        /// </summary>
        /// <typeparam name="T">Class object that represents CSV data</typeparam>
        /// <param name="file">File name</param>
        /// <returns>Returns a list of class objects</returns>
        public List<T> ReadFile<T>(string file) where T : ICsvProperty
        {
            var workPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filename = Path.Combine(workPath, "CalcFiles", $"{file}.csv");
            using var reader = new StreamReader(filename);
            using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }
    }
}
