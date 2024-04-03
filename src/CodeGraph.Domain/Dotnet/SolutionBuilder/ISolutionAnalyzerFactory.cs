namespace CodeGraph.Domain.Dotnet.SolutionBuilder
{
    public interface ISolutionAnalyzerFactory
    {
        ISolutionAnalyzer CreateSolutionAnalyzer(string solutionPath);
    }
}