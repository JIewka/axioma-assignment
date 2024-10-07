using AxiomaAssignment.Services;
using System.Text.Json;
using Xunit;

namespace Test
{
    public class QueryParsingServiceTests
    {
        [Fact]
        public void TestParseEqualsInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex";
            var parsingService = new QueryParsingService();

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(OperatorEnum.EQ, actualNode.Operator);
            Assert.Equal("Name", actualNode.LeftNode!.ColumnName);
            Assert.Equal("Alex", actualNode.RightNode!.Value);
        }

        [Fact]
        public void TestParseWithAndOrOperatorsInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND name = john or name = PETE";
            var parsingService = new QueryParsingService();

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.OR,
                Node.CreateOperatorNode(OperatorEnum.AND,
                    Node.CreateOperatorNode(OperatorEnum.EQ,
                        Node.CreateColumnNameNode("Name"),
                        Node.CreateValueNode("Alex")
                    ),
                    Node.CreateOperatorNode(OperatorEnum.EQ,
                        Node.CreateColumnNameNode("name"),
                        Node.CreateValueNode("john")
                    )
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("name"),
                    Node.CreateValueNode("PETE")
                )
            );

            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestInvalidInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND OR name = john";
            var parsingService = new QueryParsingService();

            // act & assert
            var exception = Assert.Throws<ArgumentException>(() => parsingService.Parse(searchQuery));
            Assert.Equal($"Invalid query format. Unsupported operator 'name'", exception.Message);
        }

        [Fact]
        public void TestParseWithMultipleAndOrOperatorsInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND name = john OR name = PETE AND Age = 30 OR Department = Sales";
            var parsingService = new QueryParsingService();

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.OR,
                Node.CreateOperatorNode(OperatorEnum.OR,
                    Node.CreateOperatorNode(OperatorEnum.AND,
                        Node.CreateOperatorNode(OperatorEnum.EQ,
                            Node.CreateColumnNameNode("Name"),
                            Node.CreateValueNode("Alex")
                        ),
                        Node.CreateOperatorNode(OperatorEnum.EQ,
                            Node.CreateColumnNameNode("name"),
                            Node.CreateValueNode("john")
                        )
                    ),
                    Node.CreateOperatorNode(OperatorEnum.AND,
                        Node.CreateOperatorNode(OperatorEnum.EQ,
                            Node.CreateColumnNameNode("name"),
                            Node.CreateValueNode("PETE")
                        ),
                        Node.CreateOperatorNode(OperatorEnum.EQ,
                            Node.CreateColumnNameNode("Age"),
                            Node.CreateValueNode("30")
                        )
                    )
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Department"),
                    Node.CreateValueNode("Sales")
                )
            );

            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithOrOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex OR name = john";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.OR,
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Name"),
                    Node.CreateValueNode("Alex")
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("name"),
                    Node.CreateValueNode("john")
                )
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithAndOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND name = john";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.AND,
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("Name"),
                    Node.CreateValueNode("Alex")
                ),
                Node.CreateOperatorNode(OperatorEnum.EQ,
                    Node.CreateColumnNameNode("name"),
                    Node.CreateValueNode("john")
                )
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Theory]
        [InlineData("like")]
        [InlineData("LIKE")]
        public void TestParseWithLikeOperatorInputQuery(string likeOperator)
        {
            // arrange
            var searchQuery = $"Description {likeOperator} developer";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.LIKE,
                Node.CreateColumnNameNode("Description"),
                Node.CreateValueNode("developer")
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithLessThanOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Age < 30";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.LT,
                Node.CreateColumnNameNode("Age"),
                Node.CreateValueNode("30")
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithGreaterThanOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Age > 18";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.GT,
                Node.CreateColumnNameNode("Age"),
                Node.CreateValueNode("18")
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithNotEqualsOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Name != Alex";
            var parsingService = new QueryParsingService();
            var expectedNode = Node.CreateOperatorNode(OperatorEnum.NE,
                Node.CreateColumnNameNode("Name"),
                Node.CreateValueNode("Alex")
            );

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }
    }
}
