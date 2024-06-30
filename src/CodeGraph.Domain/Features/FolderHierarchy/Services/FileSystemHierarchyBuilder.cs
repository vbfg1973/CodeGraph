using CodeGraph.Domain.Graph.Database.Repositories.Results;

namespace CodeGraph.Domain.Features.FolderHierarchy.Services
{
    public class FileSystemHierarchyBuilder
    {
        public List<FileSystemHierarchy> FileSystemHierarchy(
            List<HierarchyFileSystemQueryResult> hierarchyFileSystemResults)
        {
            List<FileSystemHierarchy> hierarchyRoots = hierarchyFileSystemResults
                .Where(hierarchyFileSystemQueryResult => string.Equals(hierarchyFileSystemQueryResult.ParentFullName,
                    hierarchyFileSystemQueryResult.ParentName))
                .Select(FileSystemHierarchyFromParent)
                .ToList();

            Dictionary<string, List<HierarchyFileSystemQueryResult>> parentChildDictionary =
                hierarchyFileSystemResults
                    .GroupBy(result => result.ParentFullName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.ToList());

            foreach (var root in hierarchyRoots)
            {
                BuildHierarchy(root, parentChildDictionary);
            }
            
            return hierarchyRoots;
        }

        private void BuildHierarchy(FileSystemHierarchy hierarchy,
            Dictionary<string, List<HierarchyFileSystemQueryResult>> dictionary)
        {
            if (!dictionary.ContainsKey(hierarchy.FullName)) return;
            
            List<FileSystemHierarchy> children =
                dictionary[hierarchy.FullName].Select(FileSystemHierarchyFromChild).ToList();
            hierarchy.Children.AddRange(children);

            foreach (var child in children)
            {
                BuildHierarchy(child, dictionary);
            }
        }

        private static FileSystemHierarchy FileSystemHierarchyFromParent(
            HierarchyFileSystemQueryResult hierarchyFileSystemQueryResult)
        {
            return new FileSystemHierarchy
            {
                FullName = hierarchyFileSystemQueryResult.ParentFullName,
                Name = hierarchyFileSystemQueryResult.ParentName,
                Pk = hierarchyFileSystemQueryResult.ParentPk,
                Type = hierarchyFileSystemQueryResult.ParentType
            };
        }

        private static FileSystemHierarchy FileSystemHierarchyFromChild(
            HierarchyFileSystemQueryResult hierarchyFileSystemQueryResult)
        {
            return new FileSystemHierarchy
            {
                FullName = hierarchyFileSystemQueryResult.ChildFullName,
                Name = hierarchyFileSystemQueryResult.ChildName,
                Pk = hierarchyFileSystemQueryResult.ChildPk,
                Type = hierarchyFileSystemQueryResult.ChildType
            };
        }
    }
}