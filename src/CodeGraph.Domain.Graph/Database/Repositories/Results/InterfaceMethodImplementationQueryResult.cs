namespace CodeGraph.Domain.Graph.Database.Repositories.Results
{
    public class InterfaceMethodImplementationQueryResult
    {
        public string InterfaceFullName { get; init; } = null!;
        public string InterfaceName { get; init; } = null!;
        public string InterfacePk { get; init; } = null!;

        public string InterfaceMethodFullName { get; init; } = null!;
        public string InterfaceMethodName { get; init; } = null!;
        public string InterfaceMethodPk { get; init; } = null!;

        public string ClassFullName { get; init; } = null!;
        public string ClassName { get; init; } = null!;
        public string ClassPk { get; init; } = null!;

        public string ClassMethodFullName { get; init; } = null!;
        public string ClassMethodName { get; init; } = null!;
        public string ClassMethodPk { get; init; } = null!;
        public string ClassMethodReturnType { get; init; } = null!;
    }
}