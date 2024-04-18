using CommandLine;

namespace CodeGraph.Domain.Features.Solution
{
    [Verb("solution", HelpText = "Import solution")]
    public class ImportSolutionOptions : ISolutionOptions, IDatabaseOptions
    {
        public string Solution { get; set; } = null!;
        public bool DeleteDatabaseContents { get; set; }
    }
}