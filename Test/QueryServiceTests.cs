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
        public void TestEqualsSingleResultReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Doe" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.EQ,
                Node.CreateValueNode("John"),
                Node.CreateColumnNameNode("Name")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Single(result);
            Assert.Equal("John", result[0]["Name"]);
            Assert.Equal("Doe", result[0]["Surname"]);
        }

        [Fact]
        public void TestEqualsMultipleResultsReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Borisov" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.EQ,
                Node.CreateColumnNameNode("Surname"),
                Node.CreateValueNode("Borisov")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Equal(2, result.Count);

            Assert.Equal("Aleksej", result[0]["Name"]);
            Assert.Equal("Borisov", result[0]["Surname"]);

            Assert.Equal("John", result[1]["Name"]);
            Assert.Equal("Borisov", result[1]["Surname"]);
        }

        [Fact]
        public void TestOrMultipleResultsReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "Jane" }, { "Surname", "Doe" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.OR,
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Name"),
                    Node.CreateValueNode("Aleksej")
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Name"),
                    Node.CreateValueNode("John")
                )
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Equal(2, result.Count);

            Assert.Equal("Aleksej", result[0]["Name"]);
            Assert.Equal("Borisov", result[0]["Surname"]);

            Assert.Equal("John", result[1]["Name"]);
            Assert.Equal("Borisov", result[1]["Surname"]);
        }

        [Fact]
        public void TestAndSingleResultReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Doe" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.AND,
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Surname"),
                    Node.CreateValueNode("Borisov")
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Name"),
                    Node.CreateValueNode("John")
                )
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Equal(1, result.Count);
            Assert.Equal("John", result[0]["Name"]);
            Assert.Equal("Borisov", result[0]["Surname"]);
        }

        [Fact]
        public void TestNotEqualsSingleResultReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Doe" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.NE,
                Node.CreateColumnNameNode("Surname"),
                Node.CreateValueNode("Doe")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Single(result);
            Assert.Equal("Aleksej", result[0]["Name"]);
            Assert.Equal("Borisov", result[0]["Surname"]);
        }

        [Fact]
        public void TestLessThanSingleResultReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Age", "27" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Age", "35" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.LT,
                Node.CreateColumnNameNode("Age"),
                Node.CreateValueNode("30")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Single(result);
            Assert.Equal("Aleksej", result[0]["Name"]);
            Assert.Equal("27", result[0]["Age"]);
        }

        [Fact]
        public void TestGreaterThanSingleResultReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Age", "27" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Age", "35" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.GT,
                Node.CreateColumnNameNode("Age"),
                Node.CreateValueNode("30")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Single(result);
            Assert.Equal("John", result[0]["Name"]);
            Assert.Equal("35", result[0]["Age"]);
        }

        [Fact]
        public void TestLikeMultipleResultsReturned()
        {
            // arrange
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(r => r.GetRowsFromAllCsvFiles())
                .Returns(new List<IDictionary<string, string>>
                {
                    new Dictionary<string, string> { { "Name", "Aleksej" }, { "Surname", "Borisov" } },
                    new Dictionary<string, string> { { "Name", "John" }, { "Surname", "Doe" } },
                    new Dictionary<string, string> { { "Name", "ALEKSANDER" }, { "Surname", "Willis" } }
                });

            var service = new QueryService(repositoryMock.Object);

            var searchNode = Node.CreateOperatorNode(OperatorEnum.LIKE,
                Node.CreateColumnNameNode("Name"),
                Node.CreateValueNode("Leks")
            );

            // act
            var result = service.Search(searchNode);

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Aleksej", result[0]["Name"]);
            Assert.Equal("Borisov", result[0]["Surname"]);

            Assert.Equal("ALEKSANDER", result[1]["Name"]);
            Assert.Equal("Willis", result[1]["Surname"]);
        }
    }
}
