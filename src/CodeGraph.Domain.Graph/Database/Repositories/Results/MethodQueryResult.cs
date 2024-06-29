using System.Text.Json.Serialization;
using CodeGraph.Domain.Common.Enums;

namespace CodeGraph.Domain.Graph.Database.Repositories.Results
{
    public class MethodQueryResult
    {
        public string MethodOwnerFullName { get; init; } = null!;
        public string MethodOwnerName { get; init; } = null!;
        public string MethodOwnerPk { get; init; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType MethodOwnerType { get; init; }

        public string MethodFullName { get; init; } = null!;
        public string MethodName { get; init; } = null!;
        public string MethodPk { get; init; } = null!;
        public string MethodReturnType { get; init; } = null!;
    }
}