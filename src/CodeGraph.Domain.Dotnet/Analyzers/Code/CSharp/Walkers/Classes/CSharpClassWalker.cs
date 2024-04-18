using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.CSharp.Walkers.Classes
{
    public class CSharpClassWalker : AbstractCSharpWalker
    {
        private readonly ICodeWalkerFactory _codeWalkerFactory;
        private readonly Document _document;
        private readonly Compilation _compilation;

        private readonly SyntaxTree? _syntaxTree;
        private readonly SemanticModel? _semanticModel;

        public CSharpClassWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation) :
            base(codeWalkerFactory, document, compilation)
        {
            _codeWalkerFactory = codeWalkerFactory;
            _document = document;
            _compilation = compilation;

            _syntaxTree = _document.GetSyntaxTreeAsync().Result;

            if (_syntaxTree != null)
            {
                _semanticModel = _compilation.GetSemanticModel(_syntaxTree);
            }
            
            
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