using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.SolutionBuilder
{
    public class SolutionAnalyzerFactory : ISolutionAnalyzerFactory
    {

        public SolutionAnalyzerFactory()
        {
        }

        public ISolutionAnalyzer CreateSolutionAnalyzer(string solutionPath)
        {
            return new SolutionAnalyzer(solutionPath);
        }
    }
}