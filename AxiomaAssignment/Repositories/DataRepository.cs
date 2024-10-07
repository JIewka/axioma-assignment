using CsvHelper;
using System.Globalization;

namespace AxiomaAssignment.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly string _csvFolderPath;

        public DataRepository(string csvFolderPath)
        {
            _csvFolderPath = csvFolderPath;
        }

        public IList<IDictionary<string, string>> GetRowsFromAllCsvFiles()
        {
            var result = new List<IDictionary<string, string>>();
            var csvFiles = Directory.GetFiles(_csvFolderPath, "*.csv");

            foreach (var csvFile in csvFiles)
            {
                var rows = GetRowsFromSingleCsvFile(csvFile);
                result.AddRange(rows);
            }

            return result;
        }

        private IList<IDictionary<string, string>> GetRowsFromSingleCsvFile(string csvFilePath)
        {
            var result = new List<IDictionary<string, string>>();
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var rowDict = new Dictionary<string, string>();
                    foreach (var header in csv.HeaderRecord)
                    {
                        rowDict[header] = csv.GetField(header);
                    }
                    result.Add(rowDict);
                }
            }

            return result;
        }
    }
}