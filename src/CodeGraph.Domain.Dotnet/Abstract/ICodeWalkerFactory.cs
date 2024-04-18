using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public interface ICodeWalkerFactory
    {
        ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType);
    }
}