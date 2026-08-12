namespace NumberToWords.Tests.Money;

public class MoneyWordsTests
{
    [Fact]
    public void ToWords_NullCurrency_DefaultsToNigerianNaira()
    {
        string withNull = MoneyToWordsConverter.ToWords(1234.50m, currency: null);
        string withExplicitNaira = MoneyToWordsConverter.ToWords(1234.50m, CurrencyWords.NigerianNaira);

        Assert.Equal(withExplicitNaira, withNull);
        Assert.EndsWith("kobo", withNull, StringComparison.Ordinal);
    }

    [Fact]
    public void ToWords_DefaultStyle_IsAmerican()
    {
        string withDefault = MoneyToWordsConverter.ToWords(1234.50m);
        string withExplicitAmerican = MoneyToWordsConverter.ToWords(1234.50m, CurrencyWords.NigerianNaira, NumberToWordsStyle.American);

        Assert.Equal(withExplicitAmerican, withDefault);
    }

    [Fact]
    public void ToWords_ZeroMinorUnits_OmitsMinorClause()
    {
        string words = MoneyToWordsConverter.ToWords(1234.00m);

        Assert.DoesNotContain("kobo", words, StringComparison.Ordinal);
        Assert.DoesNotContain(" and ", words, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("1.994", 1, "ninety-nine")]
    [InlineData("1.995", 2, null)]
    public void ToWords_RoundsFractionalPartToTwoPlaces_WithCarryOnMidpoint(string amountText, long expectedMajorUnits, string? expectedMinorWords)
    {
        decimal amount = decimal.Parse(amountText, System.Globalization.CultureInfo.InvariantCulture);
        string words = MoneyToWordsConverter.ToWords(amount, CurrencyWords.UnitedStatesDollar);

        string expectedMajorWords = NumberToWordsConverter.ToWords(expectedMajorUnits);
        Assert.StartsWith(expectedMajorWords, words, StringComparison.Ordinal);

        if (expectedMinorWords is null)
        {
            Assert.DoesNotContain("cent", words, StringComparison.Ordinal);
        }
        else
        {
            Assert.Contains(expectedMinorWords, words, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void ToWords_NegativeAmountThatRoundsToZero_HasNoNegativePrefix()
    {
        string words = MoneyToWordsConverter.ToWords(-0.001m);

        Assert.Equal("zero naira", words);
    }

    [Fact]
    public void ToWords_NegativeAmountThatRoundsToNonzero_HasNegativePrefix()
    {
        string words = MoneyToWordsConverter.ToWords(-0.005m);

        Assert.StartsWith("negative ", words, StringComparison.Ordinal);
    }

    [Fact]
    public void ToWords_AmountBeyondLongRangeInMinorUnits_ThrowsOverflowException()
    {
        decimal enormousAmount = 100_000_000_000_000_000_000m;

        Assert.Throws<OverflowException>(() => MoneyToWordsConverter.ToWords(enormousAmount));
    }

    [Fact]
    public void ToWords_SingleUnitAmounts_UseSingularCurrencyNoun()
    {
        Assert.Equal("one dollar", MoneyToWordsConverter.ToWords(1.00m, CurrencyWords.UnitedStatesDollar));
        Assert.Equal("one dollar and one cent", MoneyToWordsConverter.ToWords(1.01m, CurrencyWords.UnitedStatesDollar));
    }
}
