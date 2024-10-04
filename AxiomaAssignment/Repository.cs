namespace AxiomaAssignment
{
    public class Repository : IRepository
    {
        public IList<IDictionary<string, string>> GetRows()
        {
            return new List<IDictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Alex" }, { "Surname", "Bor" }  },
                new Dictionary<string, string> { { "Name", "Max" }, { "Surname", "Bor" } }
            };
        }
    }
}