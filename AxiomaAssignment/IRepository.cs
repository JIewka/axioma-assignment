namespace AxiomaAssignment
{
    public interface IRepository
    {
        IList<IDictionary<string, string>> GetRowsFromSingleCsvFile(string csvFilePath);
        IList<IDictionary<string, string>> GetRowsFromAllCsvFiles();
    }
}