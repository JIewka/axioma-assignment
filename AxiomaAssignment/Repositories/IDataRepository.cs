namespace AxiomaAssignment.Repositories
{
    public interface IDataRepository
    {
        IList<IDictionary<string, string>> GetRowsFromAllCsvFiles();
    }
}