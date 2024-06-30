using System.Text.Json.Serialization;
using CodeGraph.Common.Enums;

namespace CodeGraph.Domain.Graph.Database.Repositories.Results
{
    public class FileSystemQueryResult
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FileSystemType Type { get; set; }

        public string FullName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Pk { get; init; } = null!;
    }
    
    public class HierarchyFileSystemQueryResult
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FileSystemType ParentType { get; set; }

        public string ParentFullName { get; init; } = null!;
        public string ParentName { get; init; } = null!;
        public string ParentPk { get; init; } = null!;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FileSystemType ChildType { get; set; }

        public string ChildFullName { get; init; } = null!;
        public string ChildName { get; init; } = null!;
        public string ChildPk { get; init; } = null!;
    }
}