using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Analysis.FileSystem;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Analysis
{
    public class Analyzer : IAnalyzer
    {
        private readonly AnalysisConfig _analysisConfig;
        private readonly ILoggerFactory _loggerFactory;
        private readonly AnalyzerManager _analyzerManager;
        private readonly ILogger<Analyzer> _logger;
        
        private readonly List<Triple> _triples = new();

        public Analyzer(AnalysisConfig analysisConfig, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Analyzer>();
            
            _analysisConfig = analysisConfig;
            _loggerFactory = loggerFactory;
            
            _analyzerManager = new AnalyzerManager(_analysisConfig.Solution);
        }

        public async Task<IList<Triple>> Analyze()
        {
            _logger.LogTrace("{Method}", nameof(Analyze));
            
            IEnumerable<IProjectAnalyzer> projectAnalyzers = _analyzerManager.Projects.Values;

            AdhocWorkspace workspace = new();
            List<(Project, IProjectAnalyzer, IAnalyzerResult)> projects = new();

            foreach (IProjectAnalyzer? projectAnalyzer in projectAnalyzers)
            {
                await ProjectAnalysis(projectAnalyzer, workspace, projects);
            }

            await RelationshipStatistics();
            await ReportNamespaces();
            
            return _triples;
        }

        private async Task ProjectAnalysis(IProjectAnalyzer projectAnalyzer, AdhocWorkspace workspace,
            List<(Project, IProjectAnalyzer, IAnalyzerResult)> projects)
        {
            _logger.LogTrace("{Method} {ProjectName}", nameof(ProjectAnalysis), projectAnalyzer.ProjectInSolution.ProjectName);

            Project? project = projectAnalyzer.AddToWorkspace(workspace);
            IAnalyzerResult? analyzerResult = projectAnalyzer.Build().First();
            projects.Add((project, projectAnalyzer, analyzerResult));

            await Console.Error.WriteLineAsync($"Project analysis: {project.Name}");

            ProjectReferenceAnalyzer projectReferenceAnalyzer = new(project, projectAnalyzer, analyzerResult);

            _triples.AddRange(await projectReferenceAnalyzer.Analyze());

            await CodeAnalysis(project, projectAnalyzer, analyzerResult);
        }

        private async Task CodeAnalysis(Project project, IProjectAnalyzer projectAnalyzer,
            IAnalyzerResult analyzerResult)
        {
            _logger.LogTrace("{Method} {ProjectName}", nameof(CodeAnalysis), project.Name);
            
            await Console.Error.WriteLineAsync($"Code analysis: {project.Name}");
            Compilation? compilation = await project.GetCompilationAsync();

            if (compilation == null) return;

            IEnumerable<SyntaxTree> syntaxTrees =
                compilation
                    .SyntaxTrees
                    .Where(x => !x.FilePath.Contains("obj"));

            FileSystemAnalyzer fileSystemAnalyzer = new();
            foreach (SyntaxTree syntaxTree in syntaxTrees)
            {
                IList<Triple> fileSystemTriples = await fileSystemAnalyzer.GetFileSystemChain(syntaxTree.FilePath);
                FileNode fileNode = fileSystemTriples
                    .OfType<TripleIncludedIn>()
                    .Where(x => x.NodeA is FileNode)
                    .Select(x => x.NodeA as FileNode)
                    .First()!;
                _triples.AddRange(fileSystemTriples);

                SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

                WalkerOptions walkerOptions = new(new DotnetOptions(syntaxTree, semanticModel, project), true);
                CSharpTypeDiscoveryWalker walker = new(fileNode!, new ProjectNode(project.Name), walkerOptions, _loggerFactory);
                _triples.AddRange(walker.Walk());
            }
        }
        
        private async Task RelationshipStatistics()
        {
            Dictionary<string, List<Triple>> dictionary =
                _triples.GroupBy(x => x.Relationship.Type).ToDictionary(x => x.Key, x => x.ToList());

            await Console.Error.WriteLineAsync();
            foreach (KeyValuePair<string, List<Triple>> kvp in dictionary.OrderByDescending(x => x.Value.Count()))
            {
                await Console.Error.WriteLineAsync($"{kvp.Key}: {kvp.Value.Count}");
            }

            await Console.Error.WriteLineAsync();
        }

        private async Task ReportNamespaces()
        {
            await Console.Error.WriteLineAsync();

            IEnumerable<string> invokedNamespaces = _triples
                .OfType<TripleInvocationOf>()
                .Where(x => x.NodeB is MethodNode)
                .Select(x => x.NodeB.FullName)
                .Select(x => string.Join(".", x.Split('.').SkipLast(2)))
                .GroupBy(x => x)
                .Select(grouping => new { Namespace = grouping.Key, Count = grouping.Count() })
                .OrderBy(x => x.Namespace)
                .Select(x => $"ns: {x.Namespace} - {x.Count}");

            await Console.Error.WriteLineAsync("Invoked namespaces:");
            await Console.Error.WriteLineAsync("\t" + string.Join("\n\t", invokedNamespaces));
            await Console.Error.WriteLineAsync();
        }
    }
}