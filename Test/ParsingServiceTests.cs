﻿using AxiomaAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ParsingServiceTests
    {
        [Fact]
        public void TestParseEqualsInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex";
            var parsingService = new ParsingService();

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
            var parsingService = new ParsingService();

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            var expectedNode = new Node
            {
                Operator = OperatorEnum.OR,
                LeftNode = new Node
                {
                    Operator = OperatorEnum.AND,
                    LeftNode = new Node
                    {
                        Operator = OperatorEnum.EQ,
                        LeftNode = new Node
                        {
                            ColumnName = "Name",
                        },
                        RightNode = new Node
                        {
                            Value = "Alex"
                        }
                    },
                    RightNode = new Node
                    {
                        Operator = OperatorEnum.EQ,
                        LeftNode = new Node
                        {
                            ColumnName = "name",
                        },
                        RightNode = new Node
                        {
                            Value = "john"
                        }
                    }
                },
                RightNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "name",
                    },
                    RightNode = new Node
                    {
                        Value = "PETE"
                    }
                }
            };

            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestInvalidInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND OR name = john";
            var parsingService = new ParsingService();

            // act & assert
            var exception = Assert.Throws<ArgumentException>(() => parsingService.Parse(searchQuery));
            Assert.Equal($"Invalid query format. Unsupported operator 'name'", exception.Message);
        }

        [Fact]
        public void TestParseWithMultipleAndOrOperatorsInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex AND name = john OR name = PETE AND Age = 30 OR Department = Sales";
            var parsingService = new ParsingService();

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            var expectedNode = new Node
            {
                Operator = OperatorEnum.OR,
                LeftNode = new Node
                {
                    Operator = OperatorEnum.OR,
                    LeftNode = new Node
                    {
                        Operator = OperatorEnum.AND,
                        LeftNode = new Node
                        {
                            Operator = OperatorEnum.EQ,
                            LeftNode = new Node
                            {
                                ColumnName = "Name",
                            },
                            RightNode = new Node
                            {
                                Value = "Alex"
                            }
                        },
                        RightNode = new Node
                        {
                            Operator = OperatorEnum.EQ,
                            LeftNode = new Node
                            {
                                ColumnName = "name",
                            },
                            RightNode = new Node
                            {
                                Value = "john"
                            }
                        }
                    },
                    RightNode = new Node
                    {
                        Operator = OperatorEnum.AND,
                        LeftNode = new Node
                        {
                            Operator = OperatorEnum.EQ,
                            LeftNode = new Node
                            {
                                ColumnName = "name",
                            },
                            RightNode = new Node
                            {
                                Value = "PETE" 
                            }
                        },
                        RightNode = new Node
                        {
                            Operator = OperatorEnum.EQ,
                            LeftNode = new Node
                            {
                                ColumnName = "Age",
                            },
                            RightNode = new Node
                            {
                                Value = "30"
                            }
                        }
                    }
                },
                RightNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "Department",
                    },
                    RightNode = new Node
                    {
                        Value = "Sales"
                    }
                }
            };

            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }

        [Fact]
        public void TestParseWithOrOperatorInputQuery()
        {
            // arrange
            var searchQuery = "Name = Alex OR name = john";
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.OR,
                LeftNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "Name",
                    },
                    RightNode = new Node
                    {
                        Value = "Alex"
                    }
                },
                RightNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "name",
                    },
                    RightNode = new Node
                    {
                        Value = "john"
                    }
                }
            };

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
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.AND,
                LeftNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "Name",
                    },
                    RightNode = new Node
                    {
                        Value = "Alex"
                    }
                },
                RightNode = new Node
                {
                    Operator = OperatorEnum.EQ,
                    LeftNode = new Node
                    {
                        ColumnName = "name",
                    },
                    RightNode = new Node
                    {
                        Value = "john"
                    }
                }
            };

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
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.LIKE,
                LeftNode = new Node
                {
                    ColumnName = "Description",
                },
                RightNode = new Node
                {
                    Value = "developer"
                }
            };

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
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.LT,
                LeftNode = new Node
                {
                    ColumnName = "Age",
                },
                RightNode = new Node
                {
                    Value = "30"
                }
            };

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
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.GT,
                LeftNode = new Node
                {
                    ColumnName = "Age",
                },
                RightNode = new Node
                {
                    Value = "18"
                }
            };

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
            var parsingService = new ParsingService();
            var expectedNode = new Node
            {
                Operator = OperatorEnum.NE,
                LeftNode = new Node
                {
                    ColumnName = "Name",
                },
                RightNode = new Node
                {
                    Value = "Alex"
                }
            };

            // act
            INode actualNode = parsingService.Parse(searchQuery);

            // assert
            Assert.Equal(JsonSerializer.Serialize(expectedNode), JsonSerializer.Serialize(actualNode));
        }


    }
}
