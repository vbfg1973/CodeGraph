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

       
    }
}