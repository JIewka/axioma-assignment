namespace AxiomaAssignment
{
    public class Repository : IRepository
    {
        private readonly string _csvFolderPath;

        public Repository(string csvFolderPath)
        {
            _csvFolderPath = csvFolderPath;
        }
        public IList<IDictionary<string, string>> GetRowsFromSingleCsvFile(string csvFilePath)
        {
            var result = new List<IDictionary<string, string>>();
            var rows = File.ReadAllLines(csvFilePath);
            var columnNames = rows[0].Split(',');

           
            for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
            {
                var rowValues = rows[rowIndex].Split(',');
                var rowDict = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
                {
                    rowDict[columnNames[columnIndex]] = rowValues[columnIndex];
                }

                result.Add(rowDict);
            }

            return result;
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

        //private string GetCsvFolderPathFromConfig()
        //{
        //    string configFilePath = "config.txt";

        //    if (!File.Exists(configFilePath))
        //    {
        //        throw new FileNotFoundException("The configuration file was not found.", configFilePath);
        //    }

        //    string csvFolderPath = File.ReadAllText(configFilePath).Trim();

        //    if (string.IsNullOrEmpty(csvFolderPath))
        //    {
        //        throw new InvalidOperationException("The configuration file contains an empty CSV folder path.");
        //    }

        //    return csvFolderPath;
        //}
    }
}