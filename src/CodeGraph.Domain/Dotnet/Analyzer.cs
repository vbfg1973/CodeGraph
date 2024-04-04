using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeGraph.Domain.Analysis;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet
{
    public class Analyzer
    {
        private readonly AnalysisConfig _analysisConfig;
        private readonly AnalyzerManager _analyzerManager;

        public Analyzer(AnalysisConfig analysisConfig)
        {
            _analysisConfig = analysisConfig;
            _analyzerManager = new AnalyzerManager(_analysisConfig.Solution);
        }

        public void Analyze()
        {
            var projectAnalyzers = _analyzerManager.Projects.Values;

            var workspace = new AdhocWorkspace();
            var projects = new List<(Project, IProjectAnalyzer)>();

            foreach (var projectAnalyzer in projectAnalyzers)
            {
                var project = projectAnalyzer.AddToWorkspace(workspace);
                projects.Add((project, projectAnalyzer));
            }

            for (var i = 0; i < projects.Count; i++) AnalyzeProject(i + 1, projects[i]);
        }

        private void AnalyzeProject(int index,
            (Microsoft.CodeAnalysis.Project Project, IProjectAnalyzer ProjectAnalyzer) projectTuple)
        {
            var projectName = GetProjectNameFromPath(projectTuple.Project.FilePath);
            Console.Error.WriteLine($"{index} {projectName}");

            var projectBuild = projectTuple.ProjectAnalyzer.Build().FirstOrDefault();

            ProjectReferences(projectBuild);
            PackageReferences(projectBuild);
        }

        private static void ProjectReferences(IAnalyzerResult? projectBuild)
        {
            if (projectBuild == null)
                return;

            foreach (var projectReference in projectBuild.ProjectReferences)
                Console.WriteLine($"\tProjectReference: {GetProjectNameFromPath(projectReference)}");
        }

        private static void PackageReferences(IAnalyzerResult? projectBuild)
        {
            if (projectBuild == null)
                return;

            foreach (var packageReference in projectBuild.PackageReferences)
            {
                var version = packageReference.Value.Values.FirstOrDefault(v => v.Contains('.')) ?? "Unknown";
                var name = packageReference.Key;

                Console.WriteLine($"\tPackageReference: {name} ({version})");
            }
        }

        private static string GetProjectNameFromPath(string? projectPath)
        {
            if (projectPath == null)
                return string.Empty;

            var fileName = projectPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).Last();
            fileName = fileName.Replace(".csproj", "");
            fileName = fileName.Replace(".vbproj", "");

            return fileName;
        }
    }
}