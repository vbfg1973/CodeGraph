using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public class FileSystemAnalyzer
    {
    }

    public static class FileSystemHelpers
    {

        /// <summary>
        /// Returns project root path from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetRootPath(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies path to begin at project root
        /// </summary>
        /// <param name="path"></param>
        /// <param name="projectRoot"></param>
        /// <returns></returns>
        public static string ModifyFullPathToStartAtProjectRoot(string path, string projectRoot)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns list of triples representing file system path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<Triple> GetFileSystemChain(string filePath)
        {
            var chain = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            var fileName = Path.GetFileName(filePath);

            FolderNode? prevNode = null;
            for (var i = 0; i < chain.Length; i++)
            {
                if (i < chain.Length - 1)
                {
                    var currNode = new FolderNode(Path.Combine(chain[..i]), chain[i]);
                    if (prevNode != null) yield return new TripleIncludedIn(prevNode, currNode);
                }

                else if (i == chain.Length - 1 && fileName == chain[i])
                {
                    var currNode = new FolderNode(Path.Combine(chain[..i]), chain[i]);
                    yield return new TripleIncludedIn(prevNode, currNode);
                }

                else
                {
                    Console.Error.WriteLine($"{i} : {chain[i]} : {filePath}");
                    throw new ArgumentException("Something went wrong figuring out file system chain", filePath);
                }
            }
        }
    }
}