using CodeGraph.Domain.Common;
using FluentAssertions;

namespace CodeGraph.Domain.Tests.Strings
{
    public class StringSplitOnCapitalsTests
    {
        [Theory]
        [InlineData("ThisIsATest", 4)]
        [InlineData("ILiveInTheUKWithMyCat", 8)]
        [InlineData("USAIsBiggerThanTheUK", 6)]
        public void Given_String_With_Capitals_Split_To_Correct_Number(string str, int expectedCount)
        {
            List<string> elements = str.SplitStringOnCapitals().ToList();
            elements.Count.Should().Be(expectedCount);
        } 
    }
}