namespace AxiomaAssignment
{
    public class Repository : IRepository
    {
        public IList<IDictionary<string, string>> GetRowsFromSingleCsvFile(string csvFilePath)
        {
            var result = new List<IDictionary<string, string>>();

            // Read all lines from the CSV file
            var rows = File.ReadAllLines(csvFilePath);

            // Assuming the first line contains the headers
            var columnNames = rows[0].Split(',');

            // Process each line (row) after the headers
            for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
            {
                var rowValues = rows[rowIndex].Split(',');
                var rowDict = new Dictionary<string, string>();

                // Map each value to the corresponding header (column name)
                for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
                {
                    rowDict[columnNames[columnIndex]] = rowValues[columnIndex];
                }

                // Add the row dictionary to the result list
                result.Add(rowDict);
            }

            return result;
        }

        public IList<IDictionary<string, string>> GetRowsFromAllCsvFile(string csvFolderPath)
        {
            var result = new List<IDictionary<string, string>>();

            // Get all CSV files from the directory
            var csvFiles = Directory.GetFiles(csvFolderPath, "*.csv");

            foreach (var csvFile in csvFiles)
            {
                // Call the GetRowsFromSingleCsvFile method to get the rows from each file
                var rows = GetRowsFromSingleCsvFile(csvFile);

                // Add the rows to the overall result list
                result.AddRange(rows);
            }

            return result;
        }
    }
}