using CodeGraph.Domain.Dotnet;

namespace CodeGraph
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var solutionPath = args[0];
            var csvFile = args[1];

            var analysisConfig = new AnalysisConfig(solutionPath, csvFile);
            var analyzer = new Analyzer(analysisConfig);
            analyzer.Analyze();
        }
    }
}