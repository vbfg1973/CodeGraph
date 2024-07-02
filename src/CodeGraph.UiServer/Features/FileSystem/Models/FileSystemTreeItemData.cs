using CodeGraph.Clients.Dto.FileSystem;
using CodeGraph.Common.Enums;
using MudBlazor;

namespace CodeGraph.UiServer.Features.FileSystem.Models
{
    public class FileSystemTreeItemData : TreeItemData<string>
    {
        public FileSystemTreeItemData(FileSystemHierarchyDto fileSystemHierarchyDto, int complexity = 0) : base(
            fileSystemHierarchyDto.Name)
        {
            Text = fileSystemHierarchyDto.Name;
            Name = fileSystemHierarchyDto.Name;
            FullName = fileSystemHierarchyDto.FullName;
            Pk = fileSystemHierarchyDto.Pk;
            Icon = GetIcon(fileSystemHierarchyDto.Type);
            Complexity = complexity;
        }

        public int Complexity { get; init; } = 0;
        public string FullName { get; init; }
        public string Name { get; init; }
        public int Pk { get; init; }

        private static string GetIcon(FileSystemType fileSystemType)
        {
            return fileSystemType switch
            {
                FileSystemType.File => Icons.Custom.FileFormats.FileCode,
                FileSystemType.Folder => Icons.Material.Filled.Folder,
                _ => Icons.Custom.FileFormats.FileCode
            };
        }
    }
}