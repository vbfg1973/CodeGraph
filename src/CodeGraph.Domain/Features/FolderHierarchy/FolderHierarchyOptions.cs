using CommandLine;

namespace CodeGraph.Domain.Features.FolderHierarchy
{
    [Verb("folders", HelpText = "Folder hierarchy")]
    public class FolderHierarchyOptions
    {
        [Option('f', nameof(FullName), HelpText = "Reveal folder Hierarchy starting at", Required = true)]
        public string FullName { get; set; } = null!;
    }
}