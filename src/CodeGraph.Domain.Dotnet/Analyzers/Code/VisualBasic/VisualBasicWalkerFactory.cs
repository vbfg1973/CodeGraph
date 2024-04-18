using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.VisualBasic
{
    public class VisualBasicWalkerFactory : ICodeWalkerFactory
    {
        public ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType)
        {
            throw new NotImplementedException();
        }
    }
}