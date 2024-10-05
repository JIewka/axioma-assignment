namespace AxiomaAssignment
{
    public class Node: INode
    {
        public OperatorEnum? Operator { get; set; } = null;
        public string? Value { get; set; } = null;
        public string? ColumnName { get; set; } = null;
        public INode? LeftNode { get; set; }
        public INode? RightNode { get; set; }

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
    }
}