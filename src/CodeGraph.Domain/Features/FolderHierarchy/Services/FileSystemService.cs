using CodeGraph.Domain.Graph.Database.Repositories.FileSystem;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.FolderHierarchy.Services
{
    
    public interface IFileSystemService
    {
        Task<List<FileSystemQueryResult>> GetRootFolders();
        Task<FileSystemQueryResult?> GetFileSystemEntry(FileSystemQueryByPk fileSystemQueryByPk);
        Task<FileSystemQueryResult?> GetFileSystemEntry(FileSystemQueryByFullName fileSystemQueryByFullName);
        Task<List<FileSystemQueryResult>> GetChildrenOf(FileSystemQueryByPk fileSystemQueryByPk);
        Task<List<FileSystemHierarchy>> GetHierarchy();
    }
    
    public class FileSystemService(IFileSystemRepository fileSystemRepository, ILogger<FileSystemService> logger) : IFileSystemService
    {
        private readonly FileSystemHierarchyBuilder _fileSystemHierarchyBuilder = new();

        public async Task<List<FileSystemQueryResult>> GetRootFolders()
        {
            return await fileSystemRepository.GetRootFolders();
        }

        public async Task<FileSystemQueryResult?> GetFileSystemEntry(FileSystemQueryByPk fileSystemQueryByPk)
        {
            return await fileSystemRepository.GetFileSystemEntry(fileSystemQueryByPk);
        }

        public async Task<FileSystemQueryResult?> GetFileSystemEntry(
            FileSystemQueryByFullName fileSystemQueryByFullName)
        {
            return await fileSystemRepository.GetFileSystemEntry(fileSystemQueryByFullName);
        }

        public async Task<List<FileSystemQueryResult>> GetChildrenOf(FileSystemQueryByPk fileSystemQueryByPk)
        {
            return await fileSystemRepository.GetChildrenOf(fileSystemQueryByPk);
        }

        public async Task<List<FileSystemHierarchy>> GetHierarchy()
        {
            List<HierarchyFileSystemQueryResult> fullHierarchy = await fileSystemRepository.GetFullHierarchy();

            return _fileSystemHierarchyBuilder.FileSystemHierarchy(fullHierarchy);
        }
    }
}