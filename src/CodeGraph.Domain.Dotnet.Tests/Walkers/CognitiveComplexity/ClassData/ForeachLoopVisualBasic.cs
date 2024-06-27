﻿using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class ForeachLoopVisualBasic : IEnumerable<object[]>
    {
        private const string FileName = "ForeachClass.VisualBasic";
        private const Language Language = Extensions.Language.VisualBasic;

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