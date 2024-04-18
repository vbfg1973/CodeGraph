using CodeGraph.Domain.Dotnet.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.VisualBasic
{
    public class VisualBasicWalkerFactory : ICodeWalkerFactory
    {
        public ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType)
        {
            throw new NotImplementedException();
        }
    }
}