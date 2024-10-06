using AxiomaAssignment;
using System.Text.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var csvFolderPath = Path.Combine(currentDirectory, "../../../CsvFiles");
var repository = new Repository(csvFolderPath);

Console.WriteLine("Possible query operators: '=', '!=', 'or', 'and', '<', '>', 'like'");
Console.WriteLine("Enter 'exit' if you want to quit");


while (true) 
{
    Console.WriteLine("Enter your query:");
    var query = Console.ReadLine();

    var queryParser = new ParsingService();
    var rootNode = queryParser.Parse(query);

    var queryService = new QueryService(repository);
    var queryResult = queryService.Search(rootNode);

    if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("");
        break;
    }
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


        var jsonResult = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(jsonResult);
        Console.WriteLine($"Total rows printed: {rowCount}");
    }
}

