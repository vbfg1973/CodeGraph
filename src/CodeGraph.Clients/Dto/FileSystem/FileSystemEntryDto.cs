using CodeGraph.Common.Enums;

namespace CodeGraph.Clients.Dto.FileSystem
{
    public record FileSystemEntryDto(FileSystemType Type, string FullName, string Name, int Pk);

    public class FileSystemHierarchyDto
    {
        public FileSystemType Type { get; init; }
        public string FullName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public int Pk { get; init; }
        public List<FileSystemHierarchyDto> Children { get; init; } = new();
    }
}