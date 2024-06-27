using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.Database.Repositories.Common;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem;
using CodeGraph.Domain.Graph.Database.Repositories.Methods;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGraph.Domain.Graph
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INeo4jDataAccess, Neo4jDataAccess>();
            serviceCollection.AddTransient<IMethodRepository, MethodRepository>();
            serviceCollection.AddTransient<IFileSystemRepository, FileSystemRepository>();

            return serviceCollection;
        }
    }
}