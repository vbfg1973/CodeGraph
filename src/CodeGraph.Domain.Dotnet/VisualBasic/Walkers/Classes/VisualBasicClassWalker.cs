using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.VisualBasic.Walkers.Classes
{
    public class VisualBasicClassWalker : AbstractVisualBasicCSharpWalker
    {
        public VisualBasicClassWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation)
            : base(codeWalkerFactory, document, compilation)
        {
        }

        public override IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }
    }
}