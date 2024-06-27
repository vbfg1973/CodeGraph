using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class LambdaCSharp : IEnumerable<object[]>
    {
        private const string FileName = "LambdaClass.CSharp";
        private const Language Language = Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SimpleLambda", 0, Language };
            yield return new object[] { FileName, "ParenthesizedLambda", 2, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}