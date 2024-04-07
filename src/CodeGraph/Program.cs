using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Dotnet.Analyzers;

namespace CodeGraph
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            string solutionPath = args[0];
            string csvFile = args[1];

            try
            {
                AnalysisConfig analysisConfig = new AnalysisConfig(solutionPath, csvFile);
                Analyzer analyzer = new Analyzer(analysisConfig);
                await analyzer.Analyze();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}