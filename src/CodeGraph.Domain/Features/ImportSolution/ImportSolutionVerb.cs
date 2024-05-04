using CodeGraph.Domain.Analysis;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.ImportSolution
{
    public class ImportSolutionVerb(ILogger<ImportSolutionVerb> logger)
    {
        private readonly ILogger<ImportSolutionVerb> _logger = logger;

        public async Task Run(ImportSolutionOptions options)
        {
            try
            {
                AnalysisConfig analysisConfig = new(options.Solution);
                Analyzer ana = new(analysisConfig);

                IList<Triple> triples = await ana.Analyze();

                CredentialsConfig creds = new("neo4j://localhost:7687;neo4j;neo4j;AdminPassword");
                await DbManager.InsertData(triples, creds, options.DeleteDatabaseContents);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}