using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeGraph.Domain.Analysis.FileSystem;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Analysis
{
    public class Analyzer : IAnalyzer
    {
        private readonly AnalysisConfig _analysisConfig;
        private readonly AnalyzerManager _analyzerManager;
        private readonly List<Triple> _triples = new();

        public Analyzer(AnalysisConfig analysisConfig)
        {
            _analysisConfig = analysisConfig;
            _analyzerManager = new AnalyzerManager(_analysisConfig.Solution);
        }

        public async Task<IList<Triple>> Analyze()
        {
            IEnumerable<IProjectAnalyzer> projectAnalyzers = _analyzerManager.Projects.Values;

            AdhocWorkspace workspace = new();
            List<(Project, IProjectAnalyzer, IAnalyzerResult)> projects = new();

            foreach (IProjectAnalyzer? projectAnalyzer in projectAnalyzers)
            {
                await ProjectAnalysis(projectAnalyzer, workspace, projects);
            }

            return _triples;
        }

        private async Task ProjectAnalysis(IProjectAnalyzer projectAnalyzer, AdhocWorkspace workspace,
            List<(Project, IProjectAnalyzer, IAnalyzerResult)> projects)
        {
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
                CSharpTypeDiscoveryWalker walker = new(fileNode!, walkerOptions);
                _triples.AddRange(walker.Walk());
            }
        }
    }
}