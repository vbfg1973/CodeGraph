using System.Text.Json;
using CodeGraph.Domain.Database;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Dotnet.Analyzers;
using CodeGraph.Domain.Dotnet.OriginalImplementation;
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

                // Console.WriteLine(JsonSerializer.Serialize(triples,
                //     new JsonSerializerOptions { WriteIndented = true }));

                CredentialsConfig creds = new("neo4j:neo4j:AdminPassword");
                await DbManager.InsertData(triples, creds, true);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}