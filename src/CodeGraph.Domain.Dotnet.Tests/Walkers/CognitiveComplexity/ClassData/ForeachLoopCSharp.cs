using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class ForeachLoopCSharp : IEnumerable<object[]>
    {
        private const string FileName = "ForeachClass.CSharp";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SingleForeachLoop", 1, Language };
            yield return new object[] { FileName, "DoubleForeachLoop", 3, Language };
            yield return new object[] { FileName, "TripleForeachLoop", 6, Language };
            yield return new object[] { FileName, "QuadrupleForeachLoop", 10, Language };
            yield return new object[] { FileName, "QuintupleForeachLoop", 15, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}