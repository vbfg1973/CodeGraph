namespace CodeGraph.Domain.Graph.QueryModels
{
    public class InterfaceMethodImplementation
    {
        public string InterfaceFullName { get; set; } = null!;
        public string InterfaceName { get; set; } = null!;
        public string InterfaceMethodName { get; set; } = null!;
        public string ClassFullName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string ClassMethodName { get; set; } = null!;
    }
}