using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class MethodCSharp : IEnumerable<object[]>
    {
        private const string FileName = "MethodClass.CSharp";
        private const Language Language = Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SimpleMethod", 0, Language };
            yield return new object[] { FileName, "MethodWithInvocation", 0, Language };
            yield return new object[] { FileName, "MethodWithRecursiveInvocation", 1, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}