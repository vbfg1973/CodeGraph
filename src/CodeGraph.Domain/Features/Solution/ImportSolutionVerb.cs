using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.Solution
{
    public class ImportSolutionVerb
    {
        private readonly ILogger<ImportSolutionVerb> _logger;

        public ImportSolutionVerb(ILogger<ImportSolutionVerb> logger)
        {
            _logger = logger;
        }

        public async Task Run(ImportSolutionOptions options)
        {
            throw new NotImplementedException();
        }
    }
}