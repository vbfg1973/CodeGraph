using CodeGraph.Domain.Analysis;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.Solution
{
    public class ImportSolutionVerb(ILogger<ImportSolutionVerb> logger)
    {
        private readonly ILogger<ImportSolutionVerb> _logger = logger;

        public async Task Run(ImportSolutionOptions options)
        {
            AnalysisConfig analysisConfig = new(options.Solution);
            Analyzer ana = new(analysisConfig);

            IList<Triple> triples = await ana.Analyze();

            Console.WriteLine(string.Join("\n", triples));
        }
    }
}