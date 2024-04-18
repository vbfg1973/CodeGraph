using CommandLine;

namespace CodeGraph.Domain.Features
{
    public interface ISolutionOptions
    {
        [Option('s', nameof(Solution), Required = true, HelpText = "Path to solution file")]
        public string Solution { get; set; }
    }

    public interface IDatabaseOptions
    {
        [Option('d', nameof(DeleteDatabaseContents), Default = false, HelpText = "Delete existing database contents")] 
        public bool DeleteDatabaseContents { get; set; }
    }
}