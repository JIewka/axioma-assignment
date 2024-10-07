namespace AxiomaAssignment.Repositories
{
    public interface IAudit
    {
        int Id { get; set; }
        string Query { get; set; }
        int NumberOfOccurrences { get; set; }
        DateTime DateTime { get; set; }
    }
}
