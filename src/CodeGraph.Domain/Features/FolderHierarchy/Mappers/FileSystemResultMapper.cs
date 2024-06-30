using AutoMapper;
using CodeGraph.Clients.Dto.FileSystem;
using CodeGraph.Domain.Features.FolderHierarchy.Services;
using CodeGraph.Domain.Graph.Database.Repositories.Results;

namespace CodeGraph.Domain.Features.FolderHierarchy.Mappers
{
    public class FileSystemResultMapper : Profile
    {
        public FileSystemResultMapper()
        {
            CreateMap<FileSystemQueryResult, FileSystemEntryDto>();
            CreateMap<FileSystemHierarchy, FileSystemHierarchyDto>();
        }
    }
}