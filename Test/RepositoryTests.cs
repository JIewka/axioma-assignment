using AxiomaAssignment;
using Moq;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Test;

public class RepositoryTests
{
    [Fact]
    public void TestSingleResultReturned()
    {
        // arrange
        var currentDirectory = Directory.GetCurrentDirectory();
        var csvFolderPath = Path.Combine(currentDirectory, "../../../TestCsvFiles");
        var repository = new Repository(csvFolderPath);
        var expectedResult = new List<IDictionary<string, string>>
        {
            new Dictionary<string, string> { { "name", "Aleksej" }, { "surname", "Borisov" }, {"age", "27"}, {"email", "bla@gmail.com"} },
            new Dictionary<string, string> { { "name", "John" }, { "surname", "Doe" }, {"age", "34"}, {"email", "john@gmail.com"} },
            new Dictionary<string, string> { { "name", "Bruce" }, { "surname", "Willis" }, {"age", "56"}, {"email", "bruce@gmail.com"} },

            new Dictionary<string, string> { { "name", "Jim" }, { "surname", "Carrey" }, {"age", "54"}, {"height", "180"} },
            new Dictionary<string, string> { { "name", "Phil" }, { "surname", "Johnson" }, {"age", "22"}, {"height", "175"} },
            new Dictionary<string, string> { { "name", "Bob" }, { "surname", "Marley" }, {"age", "21"}, {"height", "160"} },
        };

        // act
        var actualResult = repository.GetRowsFromAllCsvFiles();

        // assert
        Assert.Equal(JsonSerializer.Serialize(expectedResult), JsonSerializer.Serialize(actualResult));
    }
}