using CodeGraph.Domain.Graph.QueryModels.Queries;
using CodeGraph.Domain.Graph.QueryModels.Results;

namespace CodeGraph.Domain.Graph.Database.Repositories
{
    public interface IMethodRepository
    {
        Task<MethodQueryResult?> LookupMethodByFullName(string fullName);
        Task<MethodQueryResult?> LookupMethodByPk(string pk);

        Task<List<InterfaceMethodImplementationQueryResult>> InterfaceMethodImplementations(
            InterfaceImplementationQuery? interfaceImplementationQuery = null!);

        Task<List<MethodInvocationQueryResult>> MethodInvocations(MethodInvocationQuery? methodInvocationQuery = null);
    }
}