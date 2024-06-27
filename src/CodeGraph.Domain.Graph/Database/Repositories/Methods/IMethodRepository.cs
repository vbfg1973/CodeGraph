using CodeGraph.Domain.Graph.Database.Repositories.Methods.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Methods.Results;

namespace CodeGraph.Domain.Graph.Database.Repositories.Methods
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