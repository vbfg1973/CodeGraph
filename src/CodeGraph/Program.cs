using System.Text.Json;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Dotnet.Analyzers;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            string solutionPath = args[0];

            try
            {
                AnalysisConfig analysisConfig = new(solutionPath);
                Analyzer analyzer = new(analysisConfig);
                IList<Triple> triples = await analyzer.Analyze();

                Console.WriteLine(JsonSerializer.Serialize(triples,
                    new JsonSerializerOptions { WriteIndented = true }));
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}