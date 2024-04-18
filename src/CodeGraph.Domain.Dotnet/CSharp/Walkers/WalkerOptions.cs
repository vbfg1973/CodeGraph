using CodeGraph.Domain.Dotnet.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class WalkerOptions(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation)
    {
        public ICodeWalkerFactory CodeWalkerFactory { get; } = codeWalkerFactory;
        public Document Document { get; } = document;
        public Compilation Compilation { get; } = compilation;
    }
}