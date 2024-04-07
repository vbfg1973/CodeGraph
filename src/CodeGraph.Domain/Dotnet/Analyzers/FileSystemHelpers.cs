using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public static class FileSystemHelpers
    {
        /// <summary>
        ///     Returns project root path from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetRootPath(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Modifies path to begin at project root
        /// </summary>
        /// <param name="path"></param>
        /// <param name="projectRoot"></param>
        /// <returns></returns>
        public static string ModifyFullPathToStartAtProjectRoot(string path, string projectRoot)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns list of triples representing file system path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<Triple> GetFileSystemChain(string filePath)
        {
            var chain = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            var fileName = Path.GetFileName(filePath);

            FolderNode? prevNode = default;
            for (var i = 0; i < chain.Length; i++)
            {
                TripleIncludedIn triple;
                var rangeEnd = i + 1;

                if (i < chain.Length - 1)
                {
                    var currNode = new FolderNode(Path.Combine(chain[..rangeEnd]), chain[i]);

                    if (prevNode != default)
                    {
                        triple = new TripleIncludedIn(currNode, prevNode);
                        yield return triple;
                    }

                    prevNode = currNode;
                }

                else if (i == chain.Length - 1 && fileName == chain[i])
                {
                    var currNode = new FileNode(Path.Combine(chain[..rangeEnd]), chain[i]);

                    if (prevNode == default) continue;
                    
                    triple = new TripleIncludedIn(currNode, prevNode);
                    yield return triple;
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