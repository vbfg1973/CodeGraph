using System.Text.Json.Serialization;
using CodeGraph.Domain.Graph.QueryModels.Enums;

namespace CodeGraph.Domain.Graph.QueryModels
{
    public class MethodInvocation
    {
        public string ClassName { get; set; } = null!;
        public string MethodName { get; set; } = null!;
        public string InvokedMethodParent { get; set; } = null!;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType InvokedMethodParentType { get; set; }
        public string InvokedMethod { get; set; } = null!;
    }
}