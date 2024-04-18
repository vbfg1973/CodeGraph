using CodeGraph.Domain.Dotnet.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpWalkerFactory : ICSharpCodeWalkerFactory
    {
        public ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType)
        {
            throw new NotImplementedException();
        }
    }
}