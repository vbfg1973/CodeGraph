using CodeGraph.Common.Enums;

namespace CodeGraph.Clients.Dto.FileSystem
{
    public record FileSystemEntryDto(FileSystemType Type, string FullName, string Name, int Pk);

    public record FileSystemHierarchyDto(FileSystemType Type, string FullName, string Name, int Pk, List<FileSystemHierarchyDto> Children);
}