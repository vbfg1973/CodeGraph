using Buildalyzer;
using CodeGraph.Domain.Dotnet.Analyzers.FileSystem;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public class ProjectReferenceAnalyzer(
        Project project,
        IProjectAnalyzer projectAnalyzer,
        IAnalyzerResult analyzerResult)
        : IAnalyzer
    {
        private readonly IProjectAnalyzer _projectAnalyzer = projectAnalyzer;

        public async Task<IList<Triple>> Analyze()
        {
            List<Triple> list = new();

            string projectName = GetProjectNameFromPath(project.Name);
            FileSystemAnalyzer fileSystemAnalyzer = new();
            IList<Triple> fileSystemTriples = await fileSystemAnalyzer.GetFileSystemChain(project.FilePath!);

            FolderNode? folderNode = fileSystemTriples.Last().NodeB as FolderNode;
            ProjectNode projectNode = new(projectName);

            list.Add(new TripleIncludedIn(projectNode, folderNode!));
            list.AddRange(GetPackageDependencies(projectNode));
            list.AddRange(GetProjectDependencies(projectNode));

            return list;
        }

        private IEnumerable<TripleDependsOnProject> GetProjectDependencies(ProjectNode projectNode)
        {
            foreach (string analyzerResultProjectReference in analyzerResult.ProjectReferences)
            {
                var node = new ProjectNode(GetProjectNameFromPath(analyzerResultProjectReference));
                yield return new TripleDependsOnProject(projectNode, node);
            }
        }

        private IEnumerable<TripleDependsOnPackage> GetPackageDependencies(ProjectNode projectNode)
        {
            foreach (KeyValuePair<string, IReadOnlyDictionary<string, string>> x in analyzerResult.PackageReferences
                         .ToList())
            {
                string version = x.Value.Values.FirstOrDefault(x => x.Contains('.')) ?? "none";
                PackageNode packageNode = new PackageNode(x.Key, x.Key, version);
                yield return new TripleDependsOnPackage(projectNode, packageNode);
            }
        }

        private static string GetProjectNameFromPath(string? projectPath)
        {
            if (projectPath == null) return string.Empty;

            string fileName = projectPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                .Last();
            fileName = fileName.Replace(".csproj", "");
            fileName = fileName.Replace(".vbproj", "");

            return fileName;
        }
    }
}