using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public interface ICodeWalker
    {
        IEnumerable<Triple> Walk();
    }
}