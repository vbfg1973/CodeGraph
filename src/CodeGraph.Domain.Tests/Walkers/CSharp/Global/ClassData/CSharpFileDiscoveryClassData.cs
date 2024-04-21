using System.Collections;

namespace CodeGraph.Domain.Tests.Walkers.CSharp.Global.ClassData
{
    public class CSharpFileDiscoveryClassData : IEnumerable<object[]>
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp"
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            IEnumerable<string> paths =
                Directory.EnumerateFiles(Path.Combine(_path), "*.csharp", SearchOption.AllDirectories);
            //.Select(path => path.Remove(path.IndexOf(_path.First(), StringComparison.InvariantCulture) - 1));

            foreach (string path in paths)
            {
                yield return new object[] { path };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}