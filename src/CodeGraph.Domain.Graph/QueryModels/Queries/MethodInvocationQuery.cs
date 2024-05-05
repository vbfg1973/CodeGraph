namespace CodeGraph.Domain.Graph.QueryModels.Queries
{
    public class MethodInvocationQuery
    {
        public string ClassFullName { get; set; } = null!;
        public string MethodFullName { get; set; } = null!;
    }
}