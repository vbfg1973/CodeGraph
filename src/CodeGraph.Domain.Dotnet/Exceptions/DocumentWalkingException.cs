using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Exceptions
{
    public class DocumentWalkingException(Document document)
        : Exception(message: $"Cannot walk document: {document.FilePath}");
}