using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeGraph.Domain.Dotnet.SolutionBuilder
{
    public class SolutionAnalyzer : ISolutionAnalyzer
    {
        private readonly Dictionary<string, Compilation> _compilations = new();
        private readonly Dictionary<string, Project> _projects = new();

        public SolutionAnalyzer(string solutionPath)
        {
            SolutionPath = solutionPath;

            using var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (o, e) =>
            {
                Console.Error.WriteLine($"MSBuild {e.Diagnostic.Kind} {e.Diagnostic.Message}");
            };

            Solution = workspace.OpenSolutionAsync(solutionPath).Result;
            BuildIt().Wait();
        }

        public string SolutionPath { get; }
        public Solution Solution { get; }
        public IEnumerable<Project> Projects => _projects.Values;
        public IEnumerable<Compilation> Compilations => _compilations.Values;
        public IEnumerable<Document> Documents => Projects.SelectMany(project => project.Documents);

        /// <summary>
        ///     Get compilation for project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public bool TryGetCompilation(string projectName, out Compilation compilation)
        {
            compilation = null!;
            if (!_compilations.TryGetValue(projectName, out var value)) return false;

            compilation = value;
            return true;
        }

        /// <summary>
        ///     Get documents from project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public bool GetDocuments(string projectName, IEnumerable<Document> documents)
        {
            documents = ArraySegment<Document>.Empty;

            if (!_projects.TryGetValue(projectName, out var enumerable)) return false;

            documents = enumerable.Documents;
            return true;
        }

        private async Task BuildIt()
        {
            foreach (var project in Solution.Projects)
            {
                await Console.Error.WriteLineAsync($"Building: {project.Name}");
                var compilation = await project.GetCompilationAsync();

                if (compilation == null) continue;

                _compilations[project.Name] = compilation;
                _projects[project.Name] = project;
            }
        }
    }
}