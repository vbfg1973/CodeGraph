using CodeGraph.Domain.Common.Enums;

namespace CodeGraph.Clients.Dto.FileSystem
{
    public record FileSystemEntryDto(FileSystemType Type, string FullName, string Name, int Pk);
}