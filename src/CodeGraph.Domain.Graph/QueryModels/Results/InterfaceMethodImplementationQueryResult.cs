namespace CodeGraph.Domain.Graph.QueryModels.Results
{
    public class InterfaceMethodImplementationQueryResult
    {
        public string InterfaceFullName { get; set; } = null!;
        public string InterfaceName { get; set; } = null!;
        public string InterfacePk { get; set; } = null!;
        
        public string InterfaceMethodFullName { get; set; } = null!;
        public string InterfaceMethodName { get; set; } = null!;
        public string InterfaceMethodPk { get; set; } = null!;
        
        public string ClassFullName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string ClassPk { get; set; } = null!;
        
        public string ClassMethodFukkName { get; set; } = null!;
        public string ClassMethodName { get; set; } = null!;
        public string ClassMethodPk { get; set; } = null!;
    }
}