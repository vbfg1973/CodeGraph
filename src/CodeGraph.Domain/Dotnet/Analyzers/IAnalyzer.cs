using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Analyzers
{
    public interface IAnalyzer
    {
        Task<IList<Triple>> Analyze();
    }
}