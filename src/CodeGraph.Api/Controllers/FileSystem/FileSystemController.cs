using CodeGraph.Domain.Graph.Database.Repositories.FileSystem;
using CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries;
using CodeGraph.Domain.Graph.Database.Repositories.Results;
using Microsoft.AspNetCore.Mvc;

namespace CodeGraph.Api.Controllers.FileSystem
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSystemController(IFileSystemRepository fileSystemRepository, ILogger<FileSystemController> logger)
        : ControllerBase
    {
        [HttpGet("root", Name = nameof(GetRootFolders))]
        public async Task<ActionResult> GetRootFolders()
        {
            List<FileSystemQueryResult>? rootFolders = await fileSystemRepository.GetRootFolders();

            if (rootFolders == null || !rootFolders.Any()) return NotFound();

            return Ok(rootFolders);
        }

        [HttpGet("path/{path}", Name = nameof(GetFolderByPath))]
        public async Task<ActionResult> GetFolderByPath(string path)
        {
            // Strip final path seperator if present. Windows or real OS
            path = path
                .Trim()
                .TrimEnd('\\')
                .TrimEnd('/');

            FileSystemQueryResult? fileSystemEntry =
                await fileSystemRepository.GetFileSystemEntry(new FileSystemQueryByFullName { FullName = path });

            if (fileSystemEntry == null) return NotFound();

            return Ok(fileSystemEntry);
        }

        [HttpGet("path/{path}/children", Name = nameof(GetChildrenByPath))]
        public async Task<ActionResult> GetChildrenByPath(string path)
        {
            FileSystemQueryResult? fileSystemEntry =
                await fileSystemRepository.GetFileSystemEntry(new FileSystemQueryByFullName { FullName = path });

            if (fileSystemEntry == null) return NotFound();

            List<FileSystemQueryResult>? children =
                await fileSystemRepository.GetChildrenOf(new FileSystemQueryByPk { Pk = fileSystemEntry.Pk });

            if (children == null) return NotFound();

            return Ok(children);
        }

        [HttpGet("pk/{pk}", Name = nameof(GetFolderByPk))]
        public async Task<ActionResult> GetFolderByPk(string pk)
        {
            FileSystemQueryResult? fileSystemEntry =
                await fileSystemRepository.GetFileSystemEntry(new FileSystemQueryByPk { Pk = pk });

            if (fileSystemEntry == null) return NotFound();

            return Ok(fileSystemEntry);
        }

        [HttpGet("pk/{pk}/children", Name = nameof(GetChildrenByPk))]
        public async Task<ActionResult> GetChildrenByPk(string pk)
        {
            List<FileSystemQueryResult>? children =
                await fileSystemRepository.GetChildrenOf(new FileSystemQueryByPk { Pk = pk });

            if (children == null) return NotFound();

            return Ok(children);
        }
    }
}