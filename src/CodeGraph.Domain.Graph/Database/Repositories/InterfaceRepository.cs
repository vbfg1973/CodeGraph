using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels;
using CodeGraph.Domain.Graph.QueryModels.Queries;
using CodeGraph.Domain.Graph.QueryModels.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Graph.Database.Repositories
{
    public interface IInterfaceRepository
    {
        Task<List<InterfaceMethodImplementationQueryResult>> InterfaceMethodImplementations(InterfaceImplementationQuery? interfaceImplementationQuery = null!);

        Task<List<MethodInvocationQueryResult>> MethodInvocations(MethodInvocationQuery? methodInvocationQuery = null);
    }

    public class InterfaceRepository : IInterfaceRepository
    {
        private readonly INeo4jDataAccess _dataAccess;
        private readonly ILoggerFactory _loggerFactory;

        public InterfaceRepository(INeo4jDataAccess dataAccess, ILoggerFactory loggerFactory)
        {
            _dataAccess = dataAccess;
            _loggerFactory = loggerFactory;
        }

        public async Task<List<InterfaceMethodImplementationQueryResult>> InterfaceMethodImplementations(
            InterfaceImplementationQuery? interfaceImplementationQuery = null!)
        {
            string query = interfaceImplementationQuery == null
                ? """
                  MATCH (c)-[:HAS]-(classMethod:Method)-[:IMPLEMENTS]->(interfaceMethod:Method)<-[:HAS]-(i:Interface)
                    RETURN i.fullName AS InterfaceFullName,
                           i.name AS InterfaceName,
                           i.pk AS InterfacePk,
                           
                           interfaceMethod.fullName AS InterfaceMethodFullName,
                           interfaceMethod.name AS InterfaceMethodName,
                           interfaceMethod.pk AS InterfaceMethodPk,
                           
                           c.fullName AS ClassFullName,
                           c.name AS ClassName,
                           c.pk AS ClassPk,
                           
                           classMethod.fullName AS ClassMethodFullName,
                           classMethod.name AS ClassMethodName,
                           classMethod.pk AS ClassMethodPk
                    ORDER BY InterfaceFullName, InterfaceMethodName
                  """
                : $"""
                   MATCH (c)-[:HAS]-(classMethod:Method)-[:IMPLEMENTS]->(interfaceMethod:Method{Pk(interfaceImplementationQuery.InterfaceMethodPk)})<-[:HAS]-(i:Interface)
                   RETURN i.fullName AS InterfaceFullName,
                          i.name AS InterfaceName,
                          i.pk AS InterfacePk,
                          
                          interfaceMethod.fullName AS InterfaceMethodFullName,
                          interfaceMethod.name AS InterfaceMethodName,
                          interfaceMethod.pk AS InterfaceMethodPk,
                          
                          c.fullName AS ClassFullName,
                          c.name AS ClassName,
                          c.pk AS ClassPk,
                          
                          classMethod.fullName AS ClassMethodFullName,
                          classMethod.name AS ClassMethodName,
                          classMethod.pk AS ClassMethodPk
                     ORDER BY InterfaceFullName, InterfaceMethodName
                   """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };
            return await _dataAccess.ExecuteReadDictionaryAsync<InterfaceMethodImplementationQueryResult>(query, "p", parameters);
        }

        public async Task<List<MethodInvocationQueryResult>> MethodInvocations(MethodInvocationQuery? methodInvocationQuery = null)
        {
            string query = methodInvocationQuery == null
                ? """
                  MATCH (c)-[:HAS]-(cm:Method)-[:INVOKES]-(i:Invocation)-[:INVOKED_AT]-(at:InvocationLocation)
                  MATCH (i)-[:INVOCATION_OF]-(im)-[:HAS]-(pt)
                  RETURN c.fullName AS ClassFullName,
                         c.name AS ClassName,
                         c.pk AS ClassPk,
                         
                         cm.fullName AS ClassMethodFullName,
                         cm.name AS ClassMethodName,
                         cm.pk AS ClassMethodPk,
                         
                  	     pt.fullName AS InvokedMethodOwnerFullName,
                  	     pt.name AS InvokedMethodOwnerName,
                  	     pt.pk AS InvokedMethodOwnerPk,
                  	     
                         im.fullName AS InvokedMethodFullName,
                         im.name AS InvokedMethodName,
                         im.pk AS InvokedMethodPk,
                         
                  	     labels(pt)[1] AS InvokedMethodOwnerType,
                  	     
                  	     at.fullName AS Location
                  ORDER BY cm.fullName,
                           at.fullName
                  """
                : $"""
                   MATCH (c{FullName(methodInvocationQuery.ClassFullName)})-[:HAS]-(cm:Method{FullName(methodInvocationQuery.MethodFullName)})-[:INVOKES]-(i:Invocation)-[:INVOKED_AT]-(at:InvocationLocation)
                   MATCH (i)-[:INVOCATION_OF]-(im)-[:HAS]-(pt)
                   RETURN c.fullName AS ClassFullName,
                          c.name AS ClassName,
                          c.pk AS ClassPk,
                          
                          cm.fullName AS ClassMethodFullName,
                          cm.name AS ClassMethodName,
                          cm.pk AS ClassMethodPk,
                          
                   	      pt.fullName AS InvokedMethodOwnerFullName,
                   	      pt.name AS InvokedMethodOwnerName,
                   	      pt.pk AS InvokedMethodOwnerPk,
                   	     
                          im.fullName AS InvokedMethodFullName,
                          im.name AS InvokedMethodName,
                          im.pk AS InvokedMethodPk,
                          
                   	     labels(pt)[1] AS InvokedMethodOwnerType,
                   	     
                   	     at.fullName AS Location
                   ORDER BY cm.fullName,
                            at.fullName
                   """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };
            return await _dataAccess.ExecuteReadDictionaryAsync<MethodInvocationQueryResult>(query, "p", parameters);
        }

        private string FullName(string fullName)
        {
            return string.IsNullOrEmpty(fullName) ? string.Empty : $" {{fullName: \"{fullName}\"}}";
        }

        private string Name(string name)
        {
            return string.IsNullOrEmpty(name) ? string.Empty : $" {{name: \"{name}\"}}";
        }
        
        private string Pk(string pk)
        {
            return string.IsNullOrEmpty(pk) ? string.Empty : $" {{pk: \"{pk}\"}}";
        }
    }
}