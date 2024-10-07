namespace AxiomaAssignment.Services
{
    internal interface IQueryService
    {
        IList<IDictionary<string, string>> Search(INode searchNode);
    }
}
