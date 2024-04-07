using Buildalyzer;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public class FileSystemAnalyzer
    {
        public IEnumerable<Triple> FileSystemTriplesFromProjects(List<(Project, IProjectAnalyzer)> projects)
        {
            var allDocumentFilePaths = projects
                .Select(tuple => tuple.Item1)
                .Select(p => p.Documents)
                .SelectMany(documents => documents.Select(document => document.FilePath))
                .Where(filePath => !string.IsNullOrEmpty(filePath))
                .Where(filePath => !filePath!.Contains("obj"))
                .ToList();

            foreach (var filepath in allDocumentFilePaths)
            {
                foreach (var triple in FileSystemHelpers.GetFileSystemChain(filepath!).ToList())
                {
                    yield return triple;
                }
            }
        }
    }
}