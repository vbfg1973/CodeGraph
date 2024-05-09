using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class DoWhileLoopCSharp : IEnumerable<object[]>
    {
        private const string FileName = "DoWhileClass.CSharp";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SingleDoWhileLoop", 1, Language };
            yield return new object[] { FileName, "DoubleDoWhileLoop", 3, Language };
            yield return new object[] { FileName, "TripleDoWhileLoop", 6, Language };
            yield return new object[] { FileName, "QuadrupleDoWhileLoop", 10, Language };
            yield return new object[] { FileName, "QuintupleDoWhileLoop", 15, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}