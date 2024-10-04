namespace AxiomaAssignment
{
    public interface IRepository
    {
        IList<IDictionary<string, string>> GetRows();
    }
}