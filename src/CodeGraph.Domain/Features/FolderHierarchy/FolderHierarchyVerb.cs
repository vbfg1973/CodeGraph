using System.Runtime.InteropServices.JavaScript;
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
            FolderQueryByFullName queryFullName = new FolderQueryByFullName() { FullName = options.FullName };

            FolderQueryResult result = await fileSystemRepository.GetFolder(queryFullName);

            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions() {WriteIndented = true}));

            var sb = new StringBuilder();

            sb.AppendLine(result.FolderName);
            
            await GetHierarchy(result.FolderPk, sb);

            Console.WriteLine(sb.ToString());
        }

        private async Task GetHierarchy(string pkParent, StringBuilder sb, int depth = 0)
        {
            List<FolderQueryResult> children = await fileSystemRepository.GetChildFoldersOf(new FolderQueryByPk() { Pk = pkParent });

            foreach (var child in children)
            {
                sb.AppendLine($"{new String('\t', depth + 1)}{child.FolderName}");
                await GetHierarchy(child.FolderPk, sb, depth + 1);
            }
        }
    }
}