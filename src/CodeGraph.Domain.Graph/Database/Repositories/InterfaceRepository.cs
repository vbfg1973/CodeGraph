using System.Text.Json;
using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database.Repositories
{
    public class InterfaceRepository
    {
        private readonly INeo4jDataAccess _dataAccess;
        private readonly ILoggerFactory _loggerFactory;

        public InterfaceRepository(INeo4jDataAccess dataAccess, ILoggerFactory loggerFactory)
        {
            _dataAccess = dataAccess;
            _loggerFactory = loggerFactory;
        }

        public async Task<List<InterfaceMethodImplementation>> Query()
        {
            string query = """
                           MATCH (c)-[:HAS]-(classMethod:Method)-[:IMPLEMENTS]->(interfaceMethod:Method)<-[:HAS]-(i:Interface)
                             RETURN i.fullName AS InterfaceFullName,
                                    i.name AS InterfaceName,
                                    interfaceMethod.name AS InterfaceMethodName,
                                    c.fullName AS ClassFullName,
                                    c.name AS ClassName,
                                    classMethod.name AS ClassMethodName
                             ORDER BY InterfaceFullName, InterfaceMethodName
                           """;
            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };
            return await _dataAccess.ExecuteReadDictionaryAsync<InterfaceMethodImplementation>(query, "p", parameters);

            
        }
    }
}