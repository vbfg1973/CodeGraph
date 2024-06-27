using CodeGraph.Domain.Graph.Database.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Graph.Database.Repositories.FileSystem
{
    public class FileSystemRepository
    {
        private readonly INeo4jDataAccess _neo4JDataAccess;
        private readonly ILogger<FileSystemRepository> _logger;


        public FileSystemRepository(INeo4jDataAccess neo4JDataAccess, ILoggerFactory loggerFactory)
        {
            _neo4JDataAccess = neo4JDataAccess;
            _logger = loggerFactory.CreateLogger<FileSystemRepository>();
        }
    }
}