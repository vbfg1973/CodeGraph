using System.Globalization;
using System.Text.Json;
using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CsvHelper;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public class Analyzer : IAnalyzer
    {
        private readonly AnalysisConfig _analysisConfig;
        private readonly AnalyzerManager _analyzerManager;

        public Analyzer(AnalysisConfig analysisConfig)
        {
            _analysisConfig = analysisConfig;
            _analyzerManager = new AnalyzerManager(_analysisConfig.Solution);
        }

        public async Task<IList<Triple>> Analyze()
        {
            List<Triple> triples = new();
            IEnumerable<IProjectAnalyzer> projectAnalyzers = _analyzerManager.Projects.Values;

            AdhocWorkspace workspace = new();
            List<(Project, IProjectAnalyzer, IAnalyzerResult)> projects = new();

            foreach (IProjectAnalyzer? projectAnalyzer in projectAnalyzers)
            {
                Project? project = projectAnalyzer.AddToWorkspace(workspace);
                IAnalyzerResult? analyzerResult = projectAnalyzer.Build().First();
                projects.Add((project, projectAnalyzer, analyzerResult));

                ProjectReferenceAnalyzer projectReferenceAnalyzer = new(project, projectAnalyzer, analyzerResult);

                IList<Triple> projectTriples = await projectReferenceAnalyzer.Analyze();
                Console.WriteLine(JsonSerializer.Serialize(projectTriples,
                    new JsonSerializerOptions { WriteIndented = true }));

                triples.AddRange(projectTriples);
            }


            // foreach ((Project, IProjectAnalyzer, IAnalyzerResult) proj in projects.Where(x =>
            //              x.Item1.SupportsCompilation))
            // {
            //     Console.Error.WriteLine($"{proj.Item1.Name}");
            //     Compilation? compilation = await proj.Item1.GetCompilationAsync();
            //
            //     if (compilation == null) continue;
            //
            //     IEnumerable<SyntaxTree> syntaxTrees =
            //         compilation
            //             .SyntaxTrees
            //             .Where(x => !x.FilePath.Contains("obj"));
            //
            //     FileSystemAnalyzer fileSystemAnalyzer = new();
            //     foreach (SyntaxTree st in syntaxTrees)
            //     {
            //         IList<Triple> fileSystemTriples = await fileSystemAnalyzer.GetFileSystemChain(st.FilePath);
            //         TripleIncludedIn? fileTriple = fileSystemTriples.Last() as TripleIncludedIn;
            //         FileNode? fileNode = fileTriple!.NodeA as FileNode;
            //         triples.AddRange(fileSystemTriples);
            //
            //         SemanticModel sem = compilation.GetSemanticModel(st);
            //         CSharpCodeAnalyzer csharpCodeAnalyzer = new(st, sem, fileNode!);
            //         triples.AddRange(await csharpCodeAnalyzer.Analyze());
            //     }
            // }

            return triples;
        }


        private static void WriteCsv<T>(string path, IEnumerable<T> records)
        {
            using StreamWriter writer = new(path);
            using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
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