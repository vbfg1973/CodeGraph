using CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.VisualBasic.Walkers.Classes
{
    public class VisualBasicClassWalker : VisualBasicSyntaxWalker, ICodeWalker
    {
        public IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }
    }
}