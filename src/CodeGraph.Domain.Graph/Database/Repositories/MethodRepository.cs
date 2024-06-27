using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels.Queries;
using CodeGraph.Domain.Graph.QueryModels.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Graph.Database.Repositories

{
    public class MethodRepository : IMethodRepository
    {
        private readonly INeo4jDataAccess _dataAccess;
        private readonly ILogger<MethodRepository> _logger;

        public MethodRepository(INeo4jDataAccess dataAccess, ILoggerFactory loggerFactory)
        {
            _dataAccess = dataAccess;
            _logger = loggerFactory.CreateLogger<MethodRepository>();
        }

        public async Task<MethodQueryResult> LookupMethodByFullName(string fullName)
        {
            string query = $"""
                            MATCH (t)-[:HAS]-(m:Method{FullName(fullName)})
                             RETURN t.fullName AS MethodOwnerFullName,
                                    t.name AS MethodOwnerName,
                            		t.pk AS MethodOwnerPk,
                            		labels(t)[0] AS MethodOwnerType,
                            		
                            		m.fullName AS MethodFullName,
                            		m.name AS MethodName,
                            		m.pk AS MethodPk
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(LookupMethodByFullName), query);

            return await _dataAccess.ExecuteReadScalarAsync<MethodQueryResult>(query, parameters);
        }

        public async Task<MethodQueryResult> LookupMethodByPk(string pk)
        {
            string query = $"""
                            MATCH (t)-[:HAS]-(m:Method{Pk(pk)})
                             RETURN t.fullName AS MethodOwnerFullName,
                                    t.name AS MethodOwnerName,
                            		t.pk AS MethodOwnerPk,
                            		labels(t)[0] AS MethodOwnerType,
                            		
                            		m.fullName AS MethodFullName,
                            		m.name AS MethodName,
                            		m.pk AS MethodPk,
                            		m.returnType AS MethodReturnType
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(LookupMethodByPk), query);

            return await _dataAccess.ExecuteReadScalarAsync<MethodQueryResult>(query, parameters);
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
                           classMethod.pk AS ClassMethodPk,
                           classMethod.returnType AS ClassMethodReturnType
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
                          classMethod.pk AS ClassMethodPk,
                          classMethod.returnType AS ClassMethodReturnType
                     ORDER BY InterfaceFullName, InterfaceMethodName
                   """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(InterfaceMethodImplementations), query);

            return await _dataAccess.ExecuteReadDictionaryAsync<InterfaceMethodImplementationQueryResult>(query, "p",
                parameters);
        }

        public async Task<List<MethodInvocationQueryResult>> MethodInvocations(
            MethodInvocationQuery? methodInvocationQuery = null)
        {
            string query = methodInvocationQuery == null
                ? """
                  MATCH (c)-[:HAS]-(cm:Method)-[:INVOKES]-(i:Invocation)
                  MATCH (i)-[:INVOCATION_OF]-(im)-[:HAS]-(pt)
                  RETURN c.fullName AS CallingOwnerFullName,
                         c.name AS CallingOwnerName,
                         c.pk AS CallingOwnerPk,
                         
                         cm.fullName AS CallingOwnerMethodFullName,
                         cm.name AS CallingOwnerMethodName,
                         cm.pk AS CallingOwnerMethodPk,
                         
                  	     pt.fullName AS InvokedMethodOwnerFullName,
                  	     pt.name AS InvokedMethodOwnerName,
                  	     pt.pk AS InvokedMethodOwnerPk,
                  	     
                         im.fullName AS InvokedMethodFullName,
                         im.name AS InvokedMethodName,
                         im.pk AS InvokedMethodPk,
                         im.returnType AS InvokedMethodReturnType,
                         
                  	     labels(pt)[0] AS InvokedMethodOwnerType,
                  	     
                  	     i.location AS Location
                  ORDER BY cm.fullName
                  """
                : $"""
                   MATCH (c)-[:HAS]-(cm:Method{Pk(methodInvocationQuery.MethodPk)})-[:INVOKES]-(i:Invocation)
                   MATCH (i)-[:INVOCATION_OF]-(im)-[:HAS]-(pt)
                   RETURN c.fullName AS CallingOwnerFullName,
                          c.name AS CallingOwnerName,
                          c.pk AS CallingOwnerPk,
                          
                          cm.fullName AS CallingOwnerMethodFullName,
                          cm.name AS CallingOwnerMethodName,
                          cm.pk AS CallingOwnerMethodPk,
                          
                   	     pt.fullName AS InvokedMethodOwnerFullName,
                   	     pt.name AS InvokedMethodOwnerName,
                   	     pt.pk AS InvokedMethodOwnerPk,
                   	     
                          im.fullName AS InvokedMethodFullName,
                          im.name AS InvokedMethodName,
                          im.pk AS InvokedMethodPk,
                          im.returnType AS InvokedMethodReturnType,
                          
                   	     labels(pt)[0] AS InvokedMethodOwnerType,
                   	     
                   	     i.location AS Location
                   ORDER BY cm.fullName
                   """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(MethodInvocations), query);

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