using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Analysis
{
    public interface IAnalyzer
    {
        Task<IList<Triple>> Analyze();
    }
}