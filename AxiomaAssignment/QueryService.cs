namespace AxiomaAssignment
{
    public class QueryService(IRepository repository) : IQueryService
    {
        public IList<IDictionary<string, string>> Search(INode searchNode)
        {
            var rows = repository.GetRowsFromAllCsvFiles();

            return FilterRows(searchNode, rows);
        }

        private IList<IDictionary<string, string>> FilterRows(INode node, IList<IDictionary<string, string>> rows)
        {
            if (node.Operator == OperatorEnum.EQ || node.Operator == OperatorEnum.NE ||
                node.Operator == OperatorEnum.LT || node.Operator == OperatorEnum.GT ||
                node.Operator == OperatorEnum.LIKE)
            {
                return rows.Where(row => EvaluateCondition(node.LeftNode, node.RightNode, node.Operator.Value, row)).ToList();
            }
            if (node.Operator == OperatorEnum.AND)
            {
                var leftFilteredRows = FilterRows(node.LeftNode!, rows);
                var rightFilteredRows = FilterRows(node.RightNode!, leftFilteredRows);
                return rightFilteredRows;
            }
            if (node.Operator == OperatorEnum.OR)
            {
                var leftFilteredRows = FilterRows(node.LeftNode!, rows);
                var rightFilteredRows = FilterRows(node.RightNode!, rows);
                return leftFilteredRows.Union(rightFilteredRows).ToList();
            }

            throw new NotImplementedException($"Operator {node.Operator} is not supported.");
        } 

        private bool EvaluateCondition(INode leftNode, INode rightNode, OperatorEnum operatorType, IDictionary<string, string> row)
        {
            INode columnNode;
            INode valueNode;

            if (leftNode.IsColumnNode())
            {
                columnNode = leftNode;
            }
            else if (rightNode.IsColumnNode())
            {
                columnNode = rightNode;
            }
            else
            {
                throw new Exception("Unexpected node config detected.");
            }

           
            if (leftNode.IsValueNode())
            {
                valueNode = leftNode;
            }
            else if (rightNode.IsValueNode())
            {
                valueNode = rightNode;
            }
            else
            {
                throw new Exception("Unexpected node config detected.");
            }

            if (!row.ContainsKey(columnNode.ColumnName)) return false;

            var columnValue = row[columnNode.ColumnName];
            var comparisonValue = valueNode.Value;

            // Try parsing as integers for numeric comparison (LT, GT)
            if (operatorType == OperatorEnum.LT || operatorType == OperatorEnum.GT)
            {
                if (int.TryParse(columnValue, out int columnIntValue) && int.TryParse(comparisonValue, out int comparisonIntValue))
                {
                    // Perform integer comparison
                    return operatorType == OperatorEnum.LT ? columnIntValue < comparisonIntValue : columnIntValue > comparisonIntValue;
                }
                else
                {
                    throw new ArgumentException("Both column value and comparison value must be integers for LT or GT operators.");
                }
            }

            if (operatorType == OperatorEnum.LIKE)
            {
                return columnValue.Contains(comparisonValue, StringComparison.InvariantCultureIgnoreCase);
            }

            // Handle equality and not equal comparisons
            switch (operatorType)
            {
                case OperatorEnum.EQ:
                    return columnValue == comparisonValue;

                case OperatorEnum.NE:
                    return columnValue != comparisonValue;

                default:
                    throw new ArgumentException($"Unsupported operator {operatorType}");
            }
        }
    }


}
