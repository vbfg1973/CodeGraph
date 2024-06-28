using System.Text.Json.Serialization;
using CodeGraph.Domain.Graph.QueryModels.Enums;

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
}