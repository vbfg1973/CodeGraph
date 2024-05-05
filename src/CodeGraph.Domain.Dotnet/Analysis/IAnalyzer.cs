using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.Analysis
{
    public interface IAnalyzer
    {
        Task<IList<Triple>> Analyze();
    }
}