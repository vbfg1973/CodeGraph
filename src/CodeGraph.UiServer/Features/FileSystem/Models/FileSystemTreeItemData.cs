using CodeGraph.Clients.Dto.FileSystem;
using CodeGraph.Common.Enums;
using MudBlazor;

namespace CodeGraph.UiServer.Features.FileSystem.Models
{
    public class FileSystemTreeItemData : TreeItemData<string>
    {
        public FileSystemTreeItemData(FileSystemHierarchyDto fileSystemHierarchyDto, int? number = null) : base(
            fileSystemHierarchyDto.Name)
        {
            Text = fileSystemHierarchyDto.Name;
            Name = fileSystemHierarchyDto.Name;
            FullName = fileSystemHierarchyDto.FullName;
            Pk = fileSystemHierarchyDto.Pk;
            Icon = GetIcon(fileSystemHierarchyDto.Type);
            Number = number;
        }

        public int? Number { get; init; }
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