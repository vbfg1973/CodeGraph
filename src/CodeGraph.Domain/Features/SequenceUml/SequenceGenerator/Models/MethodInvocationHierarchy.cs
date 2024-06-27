using CodeGraph.Domain.Graph.Database.Repositories.Results;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models
{
    public class MethodInvocationHierarchy
    {
        public MethodInvocationHierarchy(MethodQueryResult methodQueryResult)
        {
            ParentTypeFullname = methodQueryResult.MethodOwnerFullName;
            ParentTypePk = methodQueryResult.MethodOwnerPk;
            MethodFullName = methodQueryResult.MethodFullName;
            MethodPk = methodQueryResult.MethodPk;
            MethodReturnType = methodQueryResult.MethodReturnType;
        }

        public MethodInvocationHierarchy(MethodInvocationQueryResult methodQueryResult)
        {
            ParentTypeFullname = methodQueryResult.InvokedMethodOwnerFullName;
            ParentTypePk = methodQueryResult.InvokedMethodOwnerPk;
            MethodFullName = methodQueryResult.InvokedMethodFullName;
            MethodPk = methodQueryResult.InvokedMethodPk;
            MethodReturnType = methodQueryResult.InvokedMethodReturnType;
        }

        public string ParentTypeFullname { get; }
        public string ParentTypePk { get; }
        public string MethodFullName { get; }
        public string MethodPk { get; }
        public string MethodReturnType { get; }

        public List<MethodInvocationHierarchy> MethodInvocations { get; } = new();
    }
}