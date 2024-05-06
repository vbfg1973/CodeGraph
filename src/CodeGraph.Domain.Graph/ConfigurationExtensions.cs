using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.Database.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGraph.Domain.Graph
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INeo4jDataAccess, Neo4jDataAccess>();
            serviceCollection.AddTransient<IMethodRepository, MethodRepository>();

            return serviceCollection;
        }
    }
}