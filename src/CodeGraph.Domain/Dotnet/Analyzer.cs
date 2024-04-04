using System.Collections;
using System.Globalization;
using Buildalyzer;
using Buildalyzer.Workspaces;
using CsvHelper;
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

            List<DataDto> dataDtos = new();
            for (var i = 0; i < projects.Count; i++)
            {
                var list = AnalyzeProject(i + 1, projects[i]);
                dataDtos.AddRange(list);
            }

            WriteCsv(_analysisConfig.CsvFile, dataDtos);
        }

        private IList<DataDto> AnalyzeProject(int index,
            (Microsoft.CodeAnalysis.Project Project, IProjectAnalyzer ProjectAnalyzer) projectTuple)
        {
            var projectName = GetProjectNameFromPath(projectTuple.Project.FilePath);
            Console.Error.WriteLine($"{index} {projectName}");

            var projectBuild = projectTuple.ProjectAnalyzer.Build().FirstOrDefault();

            List<DataDto> dataDtos = new();
            // dataDtos.AddRange(ProjectReferences(projectBuild));
            dataDtos.AddRange(PackageReferences(projectBuild));

            return dataDtos;
        }

        private IEnumerable<DataDto> ProjectReferences(IAnalyzerResult? projectBuild)
        {
            if (projectBuild == null)
                yield break;

            foreach (var projectReference in projectBuild.ProjectReferences)
            {
                Console.WriteLine($"\tProjectReference: {GetProjectNameFromPath(projectReference)}");
                yield return new DataDto(
                    Path.GetFileName(_analysisConfig.Solution),
                    GetProjectNameFromPath(projectBuild.ProjectFilePath),
                    GetProjectNameFromPath(projectReference),
                    "Package");
            }
        }

        private IEnumerable<DataDto> PackageReferences(IAnalyzerResult? projectBuild)
        {
            if (projectBuild == null)
                yield break;

            foreach (var packageReference in projectBuild.PackageReferences)
            {
                var version = packageReference.Value.Values.FirstOrDefault(v => v.Contains('.')) ?? "Unknown";
                var name = packageReference.Key;

                Console.WriteLine($"\tPackageReference: {name} ({version})");

                yield return new DataDto(
                    Path.GetFileName(_analysisConfig.Solution),
                    GetProjectNameFromPath(projectBuild.ProjectFilePath),
                    packageReference.Key,
                    "Package",
                    version);
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

        private static void WriteCsv<T>(string path, IEnumerable<T> records)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public record DataDto(
            string Solution,
            string Project,
            string RefersTo,
            string ReferenceType,
            string Version = "");
    }
}