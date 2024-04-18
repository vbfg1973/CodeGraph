using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.CSharp.Walkers.Classes
{
    public class CSharpClassWalker : CSharpSyntaxWalker, ICodeWalker
    {
        private readonly Compilation _compilation;
        private readonly Document _document;

        public CSharpClassWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation)
        {
            _document = document;
            _compilation = compilation;
        }

        public IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }
    }
}