using System.Text;
using System.Text.Json;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.FolderHierarchy
{
    [Verb("folders", HelpText = "Folder hierarchy")]
    public class FolderHierarchyOptions
    {
        [Option('f', nameof(FullName), HelpText = "Reveal folder Hierarchy starting at", Required = true)]
        public string FullName { get; set; } = null!;
    }

    public class FolderHierarchyVerb(IFileSystemRepository fileSystemRepository, ILogger<FolderHierarchyVerb> logger)
    {
        public async Task Run(FolderHierarchyOptions options)
        {
            FileSystemQueryByFullName queryFullName = new() { FullName = options.FullName };

            FileSystemQueryResult result = await fileSystemRepository.GetFileSystemEntry(queryFullName);

            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));

            StringBuilder sb = new();

            sb.AppendLine(result.Name);

            await GetHierarchy(result.Pk, sb);

            Console.WriteLine(sb.ToString());
        }

        private async Task GetHierarchy(string pkParent, StringBuilder sb, int depth = 0)
        {
            List<FileSystemQueryResult> children =
                await fileSystemRepository.GetChildrenOf(new FileSystemQueryByPk { Pk = pkParent });

            foreach (FileSystemQueryResult child in children)
            {
                sb.AppendLine($"{new string('\t', depth + 1)}{child.Name}\t{child.Type}");
                await GetHierarchy(child.Pk, sb, depth + 1);
            }
        }
    }
}