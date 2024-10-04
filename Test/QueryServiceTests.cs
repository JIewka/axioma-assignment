using AxiomaAssignment;
using Moq;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace Test
{
    public class QueryServiceTests
    {

        [Fact]
        public void TestSingleResultReturned()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRows())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Alex" }, { "Surname", "Bor" } },
                    new Dictionary<string, string> { { "Name", "Max" }, { "Surname", "Bor" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = new Node
            {
                Operator = OperatorEnum.EQ,
                LeftNode = new Node
                {
                    Value = "Max"
                },
                RightNode = new Node
                {
                    ColumnName = "Name"
                }
            };

            // Act
            var result = service.Search(searchNode);

            // Assert
            Assert.Single(result);
            Assert.Equal("Max", result[0]["Name"]);
            Assert.Equal("Bor", result[0]["Surname"]);
        }

        [Fact]
        public void TestMultipleResultsReturned()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRows())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Alex" }, { "Surname", "Bor" } },
                    new Dictionary<string, string> { { "Name", "Max" }, { "Surname", "Bor" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = new Node
            {
                Operator = OperatorEnum.EQ,
                LeftNode = new Node
                {
                    ColumnName = "Surname"
                },
                RightNode = new Node
                {
                    Value = "Bor"
                }
            };

            // Act
            var result = service.Search(searchNode);

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal("Alex", result[0]["Name"]);
            Assert.Equal("Bor", result[0]["Surname"]);

            Assert.Equal("Max", result[1]["Name"]);
            Assert.Equal("Bor", result[1]["Surname"]);
        }

        
    }
}
