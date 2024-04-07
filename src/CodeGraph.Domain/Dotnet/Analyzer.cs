using System.Globalization;
using System.Text.Json;
using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeGraph.Domain.Dotnet.Analyzers;
using CodeGraph.Domain.Graph.Triples.Abstract;
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
            IEnumerable<IProjectAnalyzer> projectAnalyzers = _analyzerManager.Projects.Values;

            AdhocWorkspace workspace = new AdhocWorkspace();
            List<(Project, IProjectAnalyzer)> projects = new List<(Project, IProjectAnalyzer)>();

            foreach (IProjectAnalyzer? projectAnalyzer in projectAnalyzers)
            {
                Project? project = projectAnalyzer.AddToWorkspace(workspace);
                projects.Add((project, projectAnalyzer));
            }

            // List<DataDto> dataDtos = new();
            // for (var i = 0; i < projects.Count; i++)
            // {
            //     var list = AnalyzeProject(i + 1, projects[i]);
            //     dataDtos.AddRange(list);
            // }
            // WriteCsv(_analysisConfig.CsvFile, dataDtos);

            FileSystemAnalyzer fileSystemAnalyzer = new FileSystemAnalyzer();
            List<Triple> triples = fileSystemAnalyzer.FileSystemTriplesFromProjects(projects).ToList();

            foreach (Triple triple in triples)
            {
                Console.WriteLine(JsonSerializer.Serialize(triple, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        private IList<DataDto> AnalyzeProject(int index,
            (Microsoft.CodeAnalysis.Project Project, IProjectAnalyzer ProjectAnalyzer) projectTuple)
        {
            string projectName = GetProjectNameFromPath(projectTuple.Project.FilePath);
            Console.Error.WriteLine($"{index} {projectName}");

            IAnalyzerResult? projectBuild = projectTuple.ProjectAnalyzer.Build().FirstOrDefault();

            List<DataDto> dataDtos = new();
            // dataDtos.AddRange(ProjectReferences(projectBuild));
            dataDtos.AddRange(PackageReferences(projectBuild));

            return dataDtos;
        }

        private IEnumerable<DataDto> ProjectReferences(IAnalyzerResult? projectBuild)
        {
            if (projectBuild == null)
            {
                yield break;
            }

            foreach (string? projectReference in projectBuild.ProjectReferences)
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
            {
                yield break;
            }

            foreach (KeyValuePair<string, IReadOnlyDictionary<string, string>> packageReference in projectBuild
                         .PackageReferences)
            {
                string version = packageReference.Value.Values.FirstOrDefault(v => v.Contains('.')) ?? "Unknown";
                string? name = packageReference.Key;

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
            {
                return string.Empty;
            }

            string fileName = projectPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                .Last();
            fileName = fileName.Replace(".csproj", "");
            fileName = fileName.Replace(".vbproj", "");

            return fileName;
        }

        private static void WriteCsv<T>(string path, IEnumerable<T> records)
        {
            using StreamWriter writer = new StreamWriter(path);
            using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
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