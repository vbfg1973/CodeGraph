using Buildalyzer;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.OriginalImplementation.FileSystem
{
    public class FileSystemAnalyzer
    {
        /// <summary>
        ///     Returns list of triples for entire list of projects
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        public async Task<IList<Triple>> FileSystemTriplesFromProjects(List<(Project, IProjectAnalyzer)> projects)
        {
            List<Triple> triples = new();
            List<string?> allDocumentFilePaths = projects
                .Select(tuple => tuple.Item1)
                .Select(p => p.Documents)
                .SelectMany(documents => documents.Select(document => document.FilePath))
                .Where(filePath => !string.IsNullOrEmpty(filePath))
                .Where(filePath => !filePath!.Contains("obj"))
                .ToList();

            foreach (string? filepath in allDocumentFilePaths)
            {
                triples.AddRange(await GetFileSystemChain(filepath!));
            }

            return triples;
        }

        /// <summary>
        ///     Returns list of triples representing file system path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IList<Triple>> GetFileSystemChain(string filePath)
        {
            List<Triple> triples = new();
            string[] chain = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            string fileName = Path.GetFileName(filePath);

            FolderNode? prevNode = default;
            for (int i = 0; i < chain.Length; i++)
            {
                TripleIncludedIn triple;
                int rangeEnd = i + 1;

                if (i < chain.Length - 1)
                {
                    FolderNode currNode = new(Path.Combine(chain[..rangeEnd]), chain[i]);

                    if (prevNode != default)
                    {
                        triple = new TripleIncludedIn(currNode, prevNode);
                        triples.Add(triple);
                    }

                    prevNode = currNode;
                }

                else if (i == chain.Length - 1 && fileName == chain[i])
                {
                    FileNode currNode = new(Path.Combine(chain[..rangeEnd]), chain[i]);

                    if (prevNode == default) continue;

                    triple = new TripleIncludedIn(currNode, prevNode);
                    triples.Add(triple);
                }

                else
                {
                    await Console.Error.WriteLineAsync($"{i} : {chain[i]} : {filePath}");
                    throw new ArgumentException("Something went wrong figuring out file system chain", filePath);
                }
            }

            return triples;
        }
    }
}