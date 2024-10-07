namespace AxiomaAssignment.Services
{
    public class QueryParsingService : IQueryParsingService
    {
        public INode Parse(string query)
        {
            var tokens = Tokenize(query);
            return ParseExpression(tokens);
        }

        private List<string> Tokenize(string query)
        {
            return new List<string>(query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

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
                tokens.RemoveAt(0);
                var right = ParseAnd(tokens);

                left = Node.CreateOperatorNode(OperatorEnum.OR, left, right);
            }

            return left;
        }

        private INode ParseAnd(List<string> tokens)
        {
            var left = ParseComparison(tokens);

            while (tokens.Count > 0 && tokens[0].ToLower() == "and")
            {
                tokens.RemoveAt(0);
                var right = ParseComparison(tokens);

                left = Node.CreateOperatorNode(OperatorEnum.AND, left, right);
            }

            return left;
        }

        private INode ParseComparison(List<string> tokens)
        {
            if (tokens.Count < 3)
            {
                throw new ArgumentException("Invalid query format. It should contain at least 3 nodes");
            }

            var leftNode = Node.CreateColumnNameNode(tokens[0]);
            tokens.RemoveAt(0);

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
            tokens.RemoveAt(0);

            var rightNode = Node.CreateValueNode(tokens[0]);
            tokens.RemoveAt(0);

            return Node.CreateOperatorNode(operatorEnum, leftNode, rightNode);
        }
    }
}
