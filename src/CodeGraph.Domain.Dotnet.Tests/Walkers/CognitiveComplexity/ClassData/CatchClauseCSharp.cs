using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class CatchClauseCSharp : IEnumerable<object[]>
    {
        private const string FileName = "CatchClass.CSharp";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SingleCatch", 1, Language };
            yield return new object[] { FileName, "DoubleCatch", 2, Language };
            yield return new object[] { FileName, "CatchWithFinally", 1, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}