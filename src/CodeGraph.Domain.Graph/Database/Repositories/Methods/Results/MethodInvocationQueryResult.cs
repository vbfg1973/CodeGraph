using System.Text.Json.Serialization;
using CodeGraph.Domain.Graph.QueryModels.Enums;

namespace CodeGraph.Domain.Graph.Database.Repositories.Methods.Results
{
    public class MethodInvocationQueryResult
    {
        public string CallingOwnerFullName { get; set; } = null!;
        public string CallingOwnerName { get; set; } = null!;
        public string CallingOwnerPk { get; set; } = null!;

        public string CallingOwnerMethodFullName { get; set; } = null!;
        public string CallingOwnerMethodName { get; set; } = null!;
        public string CallingOwnerMethodPk { get; set; } = null!;

        public string InvokedMethodOwnerFullName { get; set; } = null!;
        public string InvokedMethodOwnerName { get; set; } = null!;
        public string InvokedMethodOwnerPk { get; set; } = null!;

        public string InvokedMethodFullName { get; set; } = null!;
        public string InvokedMethodName { get; set; } = null!;
        public string InvokedMethodPk { get; set; } = null!;
        public string InvokedMethodReturnType { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType InvokedMethodOwnerType { get; set; }

        public string Location { get; set; } = null!;
    }
}