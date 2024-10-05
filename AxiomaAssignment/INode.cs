namespace AxiomaAssignment
{
    public interface INode
    {
        OperatorEnum? Operator { get; set; }
        string? Value { get; set; }
        string? ColumnName { get; set; }
        INode? LeftNode { get; set; }
        INode? RightNode { get; set; }

        bool IsColumnNode();
        bool IsValueNode();
        bool IsOperatorNode();
    }
}
