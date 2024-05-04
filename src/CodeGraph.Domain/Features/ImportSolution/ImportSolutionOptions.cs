using CodeGraph.Domain.Features.Abstract;
using CommandLine;

namespace CodeGraph.Domain.Features.ImportSolution
{
    [Verb("solution", HelpText = "Import solution")]
    public class ImportSolutionOptions : ISolutionOptions, IDatabaseOptions
    {
        public bool DeleteDatabaseContents { get; set; }
        public string Solution { get; set; } = null!;
    }
}