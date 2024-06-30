using System.Text;
using System.Text.Json;
using CodeGraph.Domain.Features.FolderHierarchy.Services;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.FolderHierarchy
{
    public class FolderHierarchyVerb(IFileSystemService fileSystemService, ILogger<FolderHierarchyVerb> logger)
    {
        public async Task Run(FolderHierarchyOptions options)
        {
            FileSystemQueryByFullName queryFullName = new() { FullName = options.FullName };

            var hierarchy = await fileSystemService.GetHierarchy();

            Console.WriteLine(JsonSerializer.Serialize(hierarchy, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}