using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.SolutionBuilder
{
    public interface ISolutionAnalyzer
    {
        string SolutionPath { get; }
        Solution Solution { get; }
        IEnumerable<Project> Projects { get; }
        IEnumerable<Compilation> Compilations { get; }
        IEnumerable<Document> Documents { get; }

        /// <summary>
        ///     Get compilation for project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        bool TryGetCompilation(string projectName, out Compilation compilation);

        /// <summary>
        ///     Get documents from project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        bool GetDocuments(string projectName, IEnumerable<Document> documents);
    }
}