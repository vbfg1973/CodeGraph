using CodeGraph.Domain.Graph.Database.Repositories.Common;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Methods;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Graph.Database.Repositories.FileSystem
{
    public interface IFileSystemRepository
    {
        Task<List<FolderQueryResult>> GetRootFolders();
        Task<FolderQueryResult> GetFolder(FolderQueryByPk folderQueryByPk);
        Task<FolderQueryResult> GetFolder(FolderQueryByFullName folderQueryByFullName);
        Task<List<FolderQueryResult>> GetChildFoldersOf(FolderQueryByPk folderQueryByPk);
    }

    public class FileSystemRepository(INeo4jDataAccess dataAccess, ILoggerFactory loggerFactory) : BaseRepository, IFileSystemRepository
    {
        private readonly ILogger<FileSystemRepository> _logger = loggerFactory.CreateLogger<FileSystemRepository>();

        public async Task<List<FolderQueryResult>> GetRootFolders()
        {
            string query = """
                           MATCH (f:Folder) 
                           WHERE NOT (f)-[:INCLUDED_IN]->() 
                           RETURN 
                              f.fullName AS FolderFullName,
                              f.name AS FolderName, 
                              f.pk AS FolderPk
                           """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetRootFolders), query);

            return await dataAccess.ExecuteReadDictionaryAsync<FolderQueryResult>(query, "p", parameters);
        }
        
        public async Task<FolderQueryResult> GetFolder(FolderQueryByPk folderQueryByPk)
        {
            string query = $"""
                           MATCH (f:Folder {Pk(folderQueryByPk.Pk)}) 
                           RETURN 
                              f.fullName AS FolderFullName,
                              f.name AS FolderName, 
                              f.pk AS FolderPk
                           """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetFolder), query);

            return await dataAccess.ExecuteReadScalarAsync<FolderQueryResult>(query, parameters);
        }
        
        public async Task<FolderQueryResult> GetFolder(FolderQueryByFullName folderQueryByFullName)
        {
            string query = $"""
                           MATCH (f:Folder {FullName(folderQueryByFullName.FullName)}) 
                           RETURN 
                              f.fullName AS FolderFullName,
                              f.name AS FolderName, 
                              f.pk AS FolderPk
                           """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetFolder), query);

            return await dataAccess.ExecuteReadScalarAsync<FolderQueryResult>(query, parameters);
        }
        
        public async Task<List<FolderQueryResult>> GetChildFoldersOf(FolderQueryByPk folderQueryByPk)
        {
            string query = $"""
                           MATCH (parent:Folder {Pk(folderQueryByPk.Pk)})<-[:INCLUDED_IN]-(child:Folder) 
                           RETURN 
                           child.fullName AS FolderFullName,
                           child.name AS FolderName, 
                           child.pk AS FolderPk
                           """;

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", "data" } };

            _logger.LogTrace("{Method} {Query}", nameof(GetChildFoldersOf), query);

            return await dataAccess.ExecuteReadDictionaryAsync<FolderQueryResult>(query, "p", parameters);
        }
    }
}