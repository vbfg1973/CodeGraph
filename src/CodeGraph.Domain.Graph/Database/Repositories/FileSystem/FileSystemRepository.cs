using CodeGraph.Domain.Graph.Database.Repositories.Common;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Graph.Database.Repositories.FileSystem
{
    public class FileSystemRepository(INeo4jDataAccess dataAccess, ILoggerFactory loggerFactory)
        : BaseRepository, IFileSystemRepository
    {
        private readonly ILogger<FileSystemRepository> _logger = loggerFactory.CreateLogger<FileSystemRepository>();

        public async Task<List<FileSystemQueryResult>> GetRootFolders()
        {
            string query = """
                           MATCH (f:Folder) 
                           WHERE NOT (f)-[:INCLUDED_IN]->() 
                           RETURN 
                               labels(f)[0] AS Type,
                               f.fullName AS FullName,
                               f.name AS Name, 
                               f.pk AS Pk
                           """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetRootFolders), query);

            return await dataAccess.ExecuteReadDictionaryAsync<FileSystemQueryResult>(query, "p", parameters);
        }

        public async Task<FileSystemQueryResult> GetFileSystemEntry(FileSystemQueryByPk fileSystemQueryByPk)
        {
            string query = $"""
                            MATCH (f:Folder|File {Pk(fileSystemQueryByPk.Pk)}) 
                            RETURN 
                                labels(f)[0] AS Type,
                                f.fullName AS FullName,
                                f.name AS Name, 
                                f.pk AS Pk
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetFileSystemEntry), query);

            return await dataAccess.ExecuteReadScalarAsync<FileSystemQueryResult>(query, parameters);
        }

        public async Task<FileSystemQueryResult> GetFileSystemEntry(FileSystemQueryByFullName fileSystemQueryByFullName)
        {
            string query = $"""
                            MATCH (f:Folder|File {FullName(fileSystemQueryByFullName.FullName)}) 
                            RETURN 
                                labels(f)[0] AS Type,
                                f.fullName AS FullName,
                                f.name AS Name, 
                                f.pk AS Pk
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetFileSystemEntry), query);

            return await dataAccess.ExecuteReadScalarAsync<FileSystemQueryResult>(query, parameters);
        }

        public async Task<List<FileSystemQueryResult>> GetChildrenOf(FileSystemQueryByPk fileSystemQueryByPk)
        {
            string query = $"""
                            MATCH (parent:Folder {Pk(fileSystemQueryByPk.Pk)})<-[:INCLUDED_IN]-(child:Folder|File) 
                            RETURN 
                                labels(child)[0] AS Type,
                                child.fullName AS FullName,
                                child.name AS Name, 
                                child.pk AS Pk
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetChildrenOf), query);

            return await dataAccess.ExecuteReadDictionaryAsync<FileSystemQueryResult>(query, "p", parameters);
        }
        
        public async Task<List<HierarchyFileSystemQueryResult>> GetFullHierarchy()
        {
            string query = $"""
                            MATCH (parent:Folder)<-[INCLUDED_IN]-(child:Folder|File) 
                            RETURN 
                                LABELS(parent)[0] AS ParentType, 
                                parent.fullName AS ParentFullName, 
                                parent.name AS ParentName, 
                                parent.pk AS ParentPk, 
                                
                                LABELS(child)[0] AS ChildType , 
                                child.fullName AS ChildFullName, 
                                child.name AS ChildName, 
                                child.pk AS ChildPk 
                                
                                ORDER BY parent.fullName, child.fullName
                            """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetFullHierarchy), query);

            return await dataAccess.ExecuteReadDictionaryAsync<HierarchyFileSystemQueryResult>(query, "p", parameters);
        }
    }
}