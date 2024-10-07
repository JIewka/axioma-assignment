namespace AxiomaAssignment.Services
{
    internal interface IQueryParsingService
    {
        INode Parse(string query);
    }
}