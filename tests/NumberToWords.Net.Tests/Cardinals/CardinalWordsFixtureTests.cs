using NumberToWords.Tests.Support;

namespace NumberToWords.Tests.Cardinals;

public class CardinalWordsFixtureTests
{
    public static IEnumerable<object[]> CardinalFixtureRows =>
        FixtureFile.ReadRows("cardinals.tsv", expectedFieldCount: 3);

    [Theory]
    [MemberData(nameof(CardinalFixtureRows))]
    public void ToWords_MatchesKnownFixtureValue(string numberToken, string styleToken, string expected)
    {
        long number = long.Parse(numberToken);
        NumberToWordsStyle style = FixtureFile.ParseStyle(styleToken);

        string actual = NumberToWordsConverter.ToWords(number, style);

        Assert.Equal(expected, actual);
    }
}
