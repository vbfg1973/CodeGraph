using CodeGraph.Common.Enums;

namespace CodeGraph.Clients.Dto.Methods
{
    public record MethodResultDto(
        string MethodOwnerFullName,
        string MethodOwnerName,
        int MethodOwnerPk,
        ObjectType MethodOwnerType,
        string MethodFullName,
        string MethodName,
        int MethodPk,
        string MethodReturnType
    );
}