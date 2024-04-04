namespace CodeGraph.Domain.Dotnet.SolutionBuilder
{
    public class SolutionAnalyzerFactory : ISolutionAnalyzerFactory
    {
        public ISolutionAnalyzer CreateSolutionAnalyzer(string solutionPath)
        {
            return new SolutionAnalyzer(solutionPath);
        }
    }
}