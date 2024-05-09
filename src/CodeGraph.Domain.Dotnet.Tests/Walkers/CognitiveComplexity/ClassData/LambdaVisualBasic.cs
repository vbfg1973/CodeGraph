using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class LambdaVisualBasic : IEnumerable<object[]>
    {
        private const string FileName = "LambdaClass.VisualBasic";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.VisualBasic;

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