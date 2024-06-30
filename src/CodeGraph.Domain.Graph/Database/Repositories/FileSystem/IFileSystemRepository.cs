using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;

namespace CodeGraph.Domain.Graph.Database.Repositories.FileSystem
{
    public interface IFileSystemRepository
    {
        Task<List<FileSystemQueryResult>> GetRootFolders();
        Task<FileSystemQueryResult?> GetFileSystemEntry(FileSystemQueryByPk fileSystemQueryByPk);
        Task<FileSystemQueryResult?> GetFileSystemEntry(FileSystemQueryByFullName fileSystemQueryByFullName);
        Task<List<FileSystemQueryResult>> GetChildrenOf(FileSystemQueryByPk fileSystemQueryByPk);
        Task<List<HierarchyFileSystemQueryResult>> GetFullHierarchy();
    }
}