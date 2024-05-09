﻿using System.Collections;
using CodeGraph.Domain.Dotnet.Extensions;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData
{
    public class WhileLoopCSharp : IEnumerable<object[]>
    {
        private const string FileName = "WhileClass.CSharp";
        private const Language Language = CodeGraph.Domain.Dotnet.Extensions.Language.CSharp;

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { FileName, "SingleWhileLoop", 1, Language };
            yield return new object[] { FileName, "DoubleWhileLoop", 3, Language };
            yield return new object[] { FileName, "TripleWhileLoop", 6, Language };
            yield return new object[] { FileName, "QuadrupleWhileLoop", 10, Language };
            yield return new object[] { FileName, "QuintupleWhileLoop", 15, Language };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}