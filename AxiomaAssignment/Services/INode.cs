namespace AxiomaAssignment.Services
{
    public interface INode
    {
        OperatorEnum? Operator { get; }
        string? Value { get; }
        string? ColumnName { get; }
        INode? LeftNode { get; }
        INode? RightNode { get; }

        bool IsColumnNode();
        bool IsValueNode();
        bool IsOperatorNode();
    }
}
