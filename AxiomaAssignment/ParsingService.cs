using System;
using System.Collections.Generic;

namespace AxiomaAssignment
{
    public class ParsingService : IParsingService
    {
        public INode Parse(string query)
        {
            var tokens = Tokenize(query);
            return ParseExpression(tokens);
        }

        // Tokenizes the input query into a list of tokens
        private List<string> Tokenize(string query)
        {
            return new List<string>(query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        // Recursive descent parser for expression
        private INode ParseExpression(List<string> tokens)
        {
            var root = ParseOr(tokens);
            return root;
        }

        private INode ParseOr(List<string> tokens)
        {
            var left = ParseAnd(tokens);

            while (tokens.Count > 0 && tokens[0].ToLower() == "or")
            {
                tokens.RemoveAt(0);  // consume "or"
                var right = ParseAnd(tokens);

                left = new Node
                {
                    Operator = OperatorEnum.OR,
                    LeftNode = left,
                    RightNode = right
                };
            }

            return left;
        }

        private INode ParseAnd(List<string> tokens)
        {
            var left = ParseComparison(tokens);

            while (tokens.Count > 0 && tokens[0].ToLower() == "and")
            {
                tokens.RemoveAt(0);  // consume "and"
                var right = ParseComparison(tokens);

                left = new Node
                {
                    Operator = OperatorEnum.AND,
                    LeftNode = left,
                    RightNode = right
                };
            }

            return left;
        }

        private INode ParseComparison(List<string> tokens)
        {
            if (tokens.Count < 3)
            {
                throw new ArgumentException("Invalid query format. It should contain at least 3 nodes");
            }

            // Create node for the left operand (column name or value)
            var leftNode = new Node
            {
                ColumnName = tokens[0]
            };
            tokens.RemoveAt(0); // consume column name or value

            // Operator (can be =, !=, LIKE, >, or <)
            var operatorToken = tokens[0];
            OperatorEnum operatorEnum;

            switch (operatorToken.ToLower())
            {
                case "=":
                    operatorEnum = OperatorEnum.EQ;
                    break;
                case "!=":
                    operatorEnum = OperatorEnum.NE;
                    break;
                case "like":
                    operatorEnum = OperatorEnum.LIKE;
                    break;
                case ">":
                    operatorEnum = OperatorEnum.GT;
                    break;
                case "<":
                    operatorEnum = OperatorEnum.LT;
                    break;
                default:
                    throw new ArgumentException($"Invalid query format. Unsupported operator '{operatorToken}'");
            }
            tokens.RemoveAt(0); // consume operator

            // Create node for the right operand (value or column name)
            var rightNode = new Node
            {
                Value = tokens[0]
            };
            tokens.RemoveAt(0); // consume value or column name

            // Return a node representing the comparison with operator and two sub-nodes
            return new Node
            {
                Operator = operatorEnum,
                LeftNode = leftNode,  // Column name node
                RightNode = rightNode // Value node
            };
        }
    }
}
