using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract
{
    public interface ICodeWalker
    {
        IEnumerable<Triple> Walk();
    }
}