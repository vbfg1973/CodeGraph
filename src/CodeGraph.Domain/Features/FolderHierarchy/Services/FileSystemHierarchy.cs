using CodeGraph.Common.Enums;

namespace CodeGraph.Domain.Features.FolderHierarchy.Services
{
    public class FileSystemHierarchy
    {
        public string FullName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Pk { get; init; } = null!;
        public FileSystemType Type { get; init; }
        public List<FileSystemHierarchy> Children { get; init; } = new();
    }
}