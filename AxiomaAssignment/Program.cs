using AxiomaAssignment;
using System.Text.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var csvFolderPath = Path.Combine(currentDirectory, "../../../CsvFiles");
var repository = new Repository(csvFolderPath);

var auditDbContext = new AuditDbContext();
var auditRepository = new AuditRepository(auditDbContext);

while (true)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Console.Clear();
    Console.WriteLine("Menu:");
    Console.WriteLine("");
    Console.WriteLine("1. Enter a query");
    Console.WriteLine("2. View audits");
    Console.WriteLine("3. Send notifications");
    Console.Write("Choose an option: ");
    var option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.WriteLine("Possible query operators: '=', '!=', 'or', 'and', '<', '>', 'like'");
            Console.WriteLine("Enter your query:");
            var query = Console.ReadLine();

            var queryParsingService = new ParsingService();
            var rootNode = queryParsingService.Parse(query);

            var queryService = new QueryService(repository);
            var queryResult = queryService.Search(rootNode);

            if (queryResult.Count == 0)
            {
                Console.WriteLine("No results found.");
            }
            else
            {
                var rowCount = 0;

                var output = new Dictionary<string, object>
                {
                    { "searchQuery", query },
                    { "result", new List<Dictionary<string, object>>() }
                };

                foreach (var row in queryResult)
                {
                    var rowDict = new Dictionary<string, object>();

                    foreach (var column in row)
                    {
                        rowDict[column.Key] = column.Value;
                    }

                    ((List<Dictionary<string, object>>)output["result"]).Add(rowDict);

                    rowCount++;
                }
                
                await auditRepository.AddAuditAsync(new Audit
                {
                    DateTime = DateTime.UtcNow, 
                    NumberOfOccurrences = rowCount,
                    Query = query
                });

                var jsonResult = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(jsonResult);
                Console.WriteLine($"Number of rows: {rowCount}");
            }
            break;

        case "2":
            var audits  = await auditRepository.GetAllAuditsAsync();
            if (audits.Count == 0)
            {
                Console.WriteLine("No audits found.");
            }
            else
            {
                foreach (var audit in audits)
                {
                    Console.WriteLine($"Query: {audit.Query}, Number of Occurrences: {audit.NumberOfOccurrences}, DateTime: {audit.DateTime}");
                }
            }
            break;

        case "3":
            Console.WriteLine("Sending notifications...");
            break;

        default:
            Console.WriteLine("Invalid choice, please choose a valid option.");
            break;
    }
}
