using CodeGraph.Common;
using FluentAssertions;

namespace CodeGraph.Domain.Dotnet.Tests.Strings
{
    public class StringSplitOnCapitalsTests
    {
        [Theory]
        [InlineData("ThisIsATest", 1)]
        [InlineData("ILiveInTheUKWithMyCat", 3)]
        [InlineData("USAIsBiggerThanTheUK", 3)]
        public void Given_String_With_Capitals_Split_To_Correct_Number(string str, int expectedCount)
        {
            List<string> elements = str.SplitStringOnCapitals().ToList();
            elements.Count.Should().Be(expectedCount);
        }
    }
}