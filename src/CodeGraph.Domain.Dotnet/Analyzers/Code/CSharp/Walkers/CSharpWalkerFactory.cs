using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.CSharp.Walkers
{
    public class CSharpWalkerFactory : ICSharpCodeWalkerFactory
    {
        public ICodeWalker GetWalker(Document document, Compilation compilation, CodeWalkerType codeWalkerType)
        {
            throw new NotImplementedException();
        }
    }
}