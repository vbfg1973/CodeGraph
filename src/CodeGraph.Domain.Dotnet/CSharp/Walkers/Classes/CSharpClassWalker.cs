using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers.Classes
{
    public class CSharpClassWalker : AbstractCSharpWalker
    {
        private readonly SemanticModel? _semanticModel;
        private readonly SyntaxTree? _syntaxTree;

        public CSharpClassWalker(WalkerOptions walkerOptions) :
            base(walkerOptions)
        {
            _syntaxTree = WalkerOptions.Document.GetSyntaxTreeAsync().Result;

            if (_syntaxTree != null) _semanticModel = WalkerOptions.Compilation.GetSemanticModel(_syntaxTree);
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