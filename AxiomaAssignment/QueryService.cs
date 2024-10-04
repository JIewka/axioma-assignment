namespace AxiomaAssignment
{
    public class QueryService(IRepository repository) : IQueryService
    {
        public IList<IDictionary<string, string>> Search(INode searchNode)
        {
            // Get all rows from the repository
            var rows = repository.GetRows();

            // Filter the rows based on the searchNode
            return FilterRows(searchNode, rows);
        }

        private IList<IDictionary<string, string>> FilterRows(INode node, IList<IDictionary<string, string>> rows)
        {
            // Base case: leaf node or single condition
            if (node.Operator == OperatorEnum.EQ)
            {
                return rows.Where(row => EvaluateCondition(node.LeftNode, node.RightNode, row)).ToList();
            }

            // For future use: extend for other operators like AND, OR, etc.
            throw new NotImplementedException($"Operator {node.Operator} is not supported.");
        }

        private bool EvaluateCondition(INode leftNode, INode rightNode, IDictionary<string, string> row)
        {
            // Get the column value from the row
            INode columnNode;
            if (!string.IsNullOrEmpty(leftNode.ColumnName))
            {
                columnNode = leftNode;
            }
            else if (!string.IsNullOrEmpty(rightNode.ColumnName))
            {
                columnNode = rightNode;
            }
            else
            {
                throw new Exception("Unexpected node config detected.");
            }

            INode valueNode;
            if (!string.IsNullOrEmpty(leftNode.Value))
            {
                valueNode = leftNode;
            }
            else if (!string.IsNullOrEmpty(rightNode.Value))
            {
                valueNode = rightNode;
            }
            else
            {
                throw new Exception("Unexpected node config detected.");
            }

            if (!row.ContainsKey(columnNode.ColumnName)) return false;

            var columnValue = row[columnNode.ColumnName];

            // Apply the EQUALS operation (EQ)
            return columnValue == valueNode.Value;
        }
    }


}
