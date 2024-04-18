using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.CSharp.Walkers.Classes
{
    public class CSharpClassWalker : AbstractCSharpWalker
    {
        public CSharpClassWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation) :
            base(codeWalkerFactory, document, compilation)
        {
        }

        public override IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            base.VisitClassDeclaration(node);
        }
    }
}