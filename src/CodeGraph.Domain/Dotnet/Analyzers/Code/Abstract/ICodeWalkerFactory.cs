using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract
{
    public interface ICodeWalkerFactory
    {
        ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType);
    }
}