using CodeGraph.Domain.Dotnet.Analysis;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.ImportSolution
{
    public class ImportSolutionVerb(ILoggerFactory loggerFactory)
    {
        private readonly ILogger<ImportSolutionVerb> _logger = loggerFactory.CreateLogger<ImportSolutionVerb>();

        public async Task Run(ImportSolutionOptions options)
        {
            try
            {
                AnalysisConfig analysisConfig = new(options.Solution);
                Analyzer analyzer = new(analysisConfig, loggerFactory);

                IList<Triple> triples = await analyzer.Analyze();

                CredentialsConfig creds = new("neo4j://localhost:7687;neo4j;neo4j;AdminPassword");
                await DbManager.InsertData(triples, creds, options.DeleteDatabaseContents);
            }

            catch (Exception e)
            {
                _logger.LogError("{Exception}", e);
            }
        }
    }
}