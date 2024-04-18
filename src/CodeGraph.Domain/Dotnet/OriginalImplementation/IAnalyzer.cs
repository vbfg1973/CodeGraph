using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Dotnet.OriginalImplementation
{
    public interface IAnalyzer
    {
        Task<IList<Triple>> Analyze();
    }
}