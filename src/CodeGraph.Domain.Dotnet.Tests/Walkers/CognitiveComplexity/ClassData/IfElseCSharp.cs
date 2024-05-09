using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class IfElseCSharp : IEnumerable<object[]>
    {
        private const string FileName = "IfElseClass.CSharp";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "BasicMethod", 0, Language };
            yield return new object[] { FileName, "Method_CoalescedIfElse", 1, Language };
            yield return new object[] { FileName, "Method_IfStatement", 1, Language };
            yield return new object[] { FileName, "Method_IfElseStatement", 2, Language };
            yield return new object[] { FileName, "Method_IfElseIfStatement", 3, Language };
            yield return new object[] { FileName, "Method_NestedIfElseStatement", 5, Language };
            yield return new object[] { FileName, "Method_DoublyNestedIfElseStatement", 8, Language };
            yield return new object[] { FileName, "Method_DeeplyNestedIfElseStatement", 12, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}