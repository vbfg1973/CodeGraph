using CodeGraph.Domain.Dotnet;

namespace CodeGraph
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string solutionPath = args[0];
            string csvFile = args[1];

            AnalysisConfig analysisConfig = new AnalysisConfig(solutionPath, csvFile);
            Analyzer analyzer = new Analyzer(analysisConfig);
            analyzer.Analyze();
        }
    }
}