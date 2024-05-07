using System.Text.Json.Serialization;
using CodeGraph.Domain.Graph.QueryModels.Enums;

namespace CodeGraph.Domain.Graph.QueryModels.Results
{
    public class MethodQueryResult
    {
        public string MethodOwnerFullName { get; set; } = null!;
        public string MethodOwnerName { get; set; } = null!;
        public string MethodOwnerPk { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType MethodOwnerType { get; set; }

        public string MethodFullName { get; set; } = null!;
        public string MethodName { get; set; } = null!;
        public string MethodPk { get; set; } = null!;
        public string MethodReturnType { get; set; } = null!;
    }
}