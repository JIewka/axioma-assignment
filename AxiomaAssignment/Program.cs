using System.Text.Json;
using AxiomaAssignment.Repositories;
using AxiomaAssignment.Services;

var currentDirectory = Directory.GetCurrentDirectory();
var csvFolderPath = Path.Combine(currentDirectory, "../../../CsvFiles");
var repository = new DataRepository(csvFolderPath);

var auditDbContext = new AuditDbContext();
var auditRepository = new AuditRepository(auditDbContext);

var queryParsingService = new QueryParsingService();
var queryService = new QueryService(repository);

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

            if (!IsQueryValid(query))
            {
                Console.WriteLine("Invalid query. Make sure that you indicate operator, column name and value");
                break;
            }

            
            var rootNode = queryParsingService.Parse(query);

            
            var queryResult = queryService.Search(rootNode);

            if (queryResult.Count == 0)
            {
                Console.WriteLine("No results found");
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
                Console.WriteLine("No audits found");
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
            Console.WriteLine("Enter severity operator (>, <, =):");
            var stringSeverityOperator = Console.ReadLine();

            if (stringSeverityOperator != ">" && stringSeverityOperator != "<" && stringSeverityOperator != "=")
            {
                Console.WriteLine("Invalid operator. Please use '>', '<', or '='");
                break;
            }

            Console.WriteLine("Enter severity number:");
            if (!int.TryParse(Console.ReadLine(), out int severityNumber))
            {
                Console.WriteLine("Invalid number. Please enter a valid integer.");
                break;
            }

            OperatorEnum severityOperator;

            if (stringSeverityOperator == ">")
            {
                severityOperator = OperatorEnum.GT;
            }
            else if (stringSeverityOperator == "<")
            {
                severityOperator = OperatorEnum.LT;
            }
            else
            {
                severityOperator = OperatorEnum.EQ;
            }

            var severityNode = Node.CreateOperatorNode(
                severityOperator, 
                Node.CreateColumnNameNode("severity"), 
                Node.CreateValueNode(severityNumber.ToString()));

            var queryResultLogs = queryService.Search(severityNode);

            var notificationService = new NotificationService();
            notificationService.SendNotifications(queryResultLogs);

            Console.WriteLine("Notifications sent based on severity.");
            break;

        default:
            Console.WriteLine("Invalid option");
            break;
    }
}

bool IsQueryValid(string query)
{
    string[] validOperators = { "=", "!=", "or", "and", "<", ">", "like" };
    var parts = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length < 3)
    {
        return false;
    }

    foreach (var part in parts)
    {
        if (validOperators.Contains(part))
        {
            return true;
        }
    }

    return false;
}