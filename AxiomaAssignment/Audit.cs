namespace AxiomaAssignment
{
    public class Audit : IAudit
    {
        public int Id { get; set; }
        public string Query { get; set; }
        public int NumberOfOccurrences { get; set; }
        public DateTime DateTime { get; set; }
    }
}
