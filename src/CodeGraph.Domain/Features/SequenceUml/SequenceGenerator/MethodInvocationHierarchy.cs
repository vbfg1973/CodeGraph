using CodeGraph.Domain.Graph.QueryModels.Results;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator
{
    public class MethodInvocationHierarchy
    {
        public MethodInvocationHierarchy(
            string parentTypeFullname,
            string parentTypePk,
            string methodFullName,
            string methodPk)
        {
            ParentTypeFullname = parentTypeFullname;
            ParentTypePk = parentTypePk;
            MethodFullName = methodFullName;
            MethodPk = methodPk;
        }

        public MethodInvocationHierarchy(MethodQueryResult methodQueryResult)
        {
            ParentTypeFullname = methodQueryResult.MethodOwnerFullName;
            ParentTypePk = methodQueryResult.MethodOwnerPk;
            MethodFullName = methodQueryResult.MethodFullName;
            MethodPk = methodQueryResult.MethodPk;
        }
        
        public MethodInvocationHierarchy(MethodInvocationQueryResult methodQueryResult)
        {
            ParentTypeFullname = methodQueryResult.InvokedMethodOwnerFullName;
            ParentTypePk = methodQueryResult.InvokedMethodOwnerPk;
            MethodFullName = methodQueryResult.InvokedMethodFullName;
            MethodPk = methodQueryResult.InvokedMethodPk;
        }
        
        public string ParentTypeFullname { get; }
        public string ParentTypePk { get; }
        public string MethodFullName { get; }
        public string MethodPk { get; }

        public List<MethodInvocationHierarchy> MethodInvocations { get; } = new();
    }
}