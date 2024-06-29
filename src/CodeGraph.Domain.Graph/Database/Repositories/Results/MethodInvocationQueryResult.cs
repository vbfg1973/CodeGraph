using System.Text.Json.Serialization;
using CodeGraph.Common.Enums;

namespace CodeGraph.Domain.Graph.Database.Repositories.Results
{
    public class MethodInvocationQueryResult
    {
        public string CallingOwnerFullName { get; init; } = null!;
        public string CallingOwnerName { get; init; } = null!;
        public string CallingOwnerPk { get; init; } = null!;

        public string CallingOwnerMethodFullName { get; init; } = null!;
        public string CallingOwnerMethodName { get; init; } = null!;
        public string CallingOwnerMethodPk { get; init; } = null!;

        public string InvokedMethodOwnerFullName { get; init; } = null!;
        public string InvokedMethodOwnerName { get; init; } = null!;
        public string InvokedMethodOwnerPk { get; init; } = null!;

        public string InvokedMethodFullName { get; init; } = null!;
        public string InvokedMethodName { get; init; } = null!;
        public string InvokedMethodPk { get; init; } = null!;
        public string InvokedMethodReturnType { get; init; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectType InvokedMethodOwnerType { get; init; }

        public string Location { get; init; } = null!;
    }
}