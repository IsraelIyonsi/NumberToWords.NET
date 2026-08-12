using NumberToWords.Tests.Support;

namespace NumberToWords.Tests.Ordinals;

public class OrdinalWordsFixtureTests
{
    public static IEnumerable<object[]> OrdinalFixtureRows =>
        FixtureFile.ReadRows("ordinals.tsv", expectedFieldCount: 3);

    [Theory]
    [MemberData(nameof(OrdinalFixtureRows))]
    public void ToOrdinalWords_MatchesKnownFixtureValue(string numberToken, string styleToken, string expected)
    {
        long number = long.Parse(numberToken);
        NumberToWordsStyle style = FixtureFile.ParseStyle(styleToken);

        string actual = NumberToWordsConverter.ToOrdinalWords(number, style);

        Assert.Equal(expected, actual);
    }
}
