namespace AxiomaAssignment
{
    public interface INode
    {
        OperatorEnum? Operator { get; set; }
        public string? Value { get; set; }
        public string? ColumnName { get; set; }
        public INode? LeftNode { get; set; }
        public INode? RightNode { get; set; }
    }
}
