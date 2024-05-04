using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public interface ICodeWalker
    {
        IEnumerable<Triple> Walk();
    }
}