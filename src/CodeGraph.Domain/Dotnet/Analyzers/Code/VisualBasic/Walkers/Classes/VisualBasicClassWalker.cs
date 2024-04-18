using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.VisualBasic.Walkers.Classes
{
    public class VisualBasicClassWalker : AbstractVisualBasicCSharpWalker
    {
        public VisualBasicClassWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation) : base(codeWalkerFactory, document, compilation)
        {
        }

        public override IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }
    }
}