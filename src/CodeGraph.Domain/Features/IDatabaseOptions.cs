using CommandLine;

namespace CodeGraph.Domain.Features
{
    public interface IDatabaseOptions
    {
        [Option('d', nameof(DeleteDatabaseContents), Default = true, HelpText = "Delete existing database contents")]
        public bool DeleteDatabaseContents { get; set; }
    }
}