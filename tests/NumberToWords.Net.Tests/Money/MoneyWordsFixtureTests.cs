using System.Globalization;
using NumberToWords.Tests.Support;

namespace NumberToWords.Tests.Money;

public class MoneyWordsFixtureTests
{
    public static IEnumerable<object[]> MoneyFixtureRows =>
        FixtureFile.ReadRows("money.tsv", expectedFieldCount: 4);

    [Theory]
    [MemberData(nameof(MoneyFixtureRows))]
    public void ToWords_MatchesKnownFixtureValue(string amountToken, string currencyToken, string styleToken, string expected)
    {
        decimal amount = decimal.Parse(amountToken, CultureInfo.InvariantCulture);
        CurrencyWords currency = ResolveCurrency(currencyToken);
        NumberToWordsStyle style = FixtureFile.ParseStyle(styleToken);

        string actual = MoneyToWordsConverter.ToWords(amount, currency, style);

        Assert.Equal(expected, actual);
    }

    private static CurrencyWords ResolveCurrency(string token) => token switch
    {
        "NGN" => CurrencyWords.NigerianNaira,
        "USD" => CurrencyWords.UnitedStatesDollar,
        _ => throw new ArgumentOutOfRangeException(nameof(token), token, "Unknown fixture currency token."),
    };
}
