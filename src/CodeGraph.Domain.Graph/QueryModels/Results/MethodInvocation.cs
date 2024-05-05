using System.Text.Json.Serialization;
using CodeGraph.Domain.Graph.QueryModels.Enums;

namespace CodeGraph.Domain.Graph.QueryModels.Results
{
    public class MethodInvocation
    {
        public string ClassFullName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string ClassPk { get; set; } = null!;
        
        public string ClassMethodFullName { get; set; } = null!;
        public string ClassMethodName { get; set; } = null!;
        public string ClassMethodPk { get; set; } = null!;

        public string InvokedMethodOwnerFullName { get; set; } = null!;
        public string InvokedMethodOwnerName { get; set; } = null!;
        public string InvokedMethodOwnerPk { get; set; } = null!;
        
        public string InvokedMethodFullName { get; set; } = null!;
        public string InvokedMethodName { get; set; } = null!;
        public string InvokedMethodPk { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType InvokedMethodOwnerType { get; set; }

        public string Location { get; set; } = null!;
    }
}