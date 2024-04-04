using CodeGraph.Domain.Analysis;
using CodeGraph.Domain.Dotnet;

namespace CodeGraph
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var solutionPath = args[0];

            var analysisConfig = new AnalysisConfig(solutionPath);
            var analyzer = new Analyzer(analysisConfig);
            analyzer.Analyze();
        }
    }
}