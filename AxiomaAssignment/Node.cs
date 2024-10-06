namespace AxiomaAssignment
{
    public class Node: INode
    {
        private Node(
            OperatorEnum? @operator, 
            string? value, 
            string? columnName, 
            INode? leftNode, 
            INode? rightNode)
        {
            Operator = @operator;
            Value = value;
            ColumnName = columnName;
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public OperatorEnum? Operator { get; }

        public string? Value { get; }

        public string? ColumnName { get; }

        public INode? LeftNode { get; }

        public INode? RightNode { get; }

        public bool IsColumnNode()
        {
            return ColumnName != null && Operator == null && Value == null;
        }

        public bool IsValueNode()
        {
            return ColumnName == null && Operator == null && Value != null;
        }

        public bool IsOperatorNode()
        {
            return ColumnName == null && Operator != null && Value == null;
        }

        public static Node CreateColumnNameNode(string columnName)
        {
            return new Node(null, null, columnName, null, null);
        }

        public static Node CreateValueNode(string value)
        {
            return new Node(null, value, null, null, null);
        }

        public static Node CreateOperatorNode(OperatorEnum @operator, INode leftNode, INode rightNode)
        {
            return new Node(@operator, null, null, leftNode, rightNode);
        }
    }
}