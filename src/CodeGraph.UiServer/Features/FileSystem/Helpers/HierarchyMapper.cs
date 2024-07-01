using CodeGraph.Clients.Dto.FileSystem;
using CodeGraph.Common.Enums;
using CodeGraph.UiServer.Features.FileSystem.Models;

namespace CodeGraph.UiServer.Features.FileSystem.Helpers
{
    public static class HierarchyMapper
    {
        public static async Task<List<FileSystemTreeItemData>> Map(IEnumerable<FileSystemHierarchyDto> hierarchies)
        {
            List<FileSystemTreeItemData> treeItems = new();

            foreach (FileSystemHierarchyDto hierarchy in hierarchies.OrderBy(x => x.Name))
            {
                FileSystemTreeItemData treeItemData = new(hierarchy);
                treeItems.Add(treeItemData);
                await MapChildrenToTreeItems(treeItemData, hierarchy);
            }

            return treeItems;
        }

        private static async Task MapChildrenToTreeItems(FileSystemTreeItemData currentTreeItem,
            FileSystemHierarchyDto currentHierarchy)
        {
            IOrderedEnumerable<FileSystemHierarchyDto> childFolders = currentHierarchy
                .Children
                .Where(x => x.Type == FileSystemType.Folder)
                .OrderBy(x => x.Name);

            IOrderedEnumerable<FileSystemHierarchyDto> childFiles = currentHierarchy
                .Children
                .Where(x => x.Type == FileSystemType.File)
                .OrderBy(x => x.Name);

            foreach (FileSystemHierarchyDto child in childFolders)
            {
                FileSystemTreeItemData treeItemData = new(child);
                currentTreeItem.Children ??= [];
                currentTreeItem.Children.Add(treeItemData);
                await MapChildrenToTreeItems(treeItemData, child);
            }

            foreach (FileSystemHierarchyDto child in childFiles)
            {
                FileSystemTreeItemData treeItemData = new(child);
                currentTreeItem.Children ??= [];
                currentTreeItem.Children.Add(treeItemData);
            }
        }
    }
}