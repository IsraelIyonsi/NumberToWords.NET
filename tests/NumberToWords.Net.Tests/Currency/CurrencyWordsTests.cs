namespace NumberToWords.Tests.Currency;

public class CurrencyWordsTests
{
    [Fact]
    public void NigerianNaira_HasInvariantSingularAndPluralNouns()
    {
        CurrencyWords naira = CurrencyWords.NigerianNaira;

        Assert.Equal("naira", naira.MajorUnitSingularName);
        Assert.Equal("naira", naira.MajorUnitPluralName);
        Assert.Equal("kobo", naira.MinorUnitSingularName);
        Assert.Equal("kobo", naira.MinorUnitPluralName);
    }

    [Fact]
    public void UnitedStatesDollar_HasDistinctSingularAndPluralNouns()
    {
        CurrencyWords dollar = CurrencyWords.UnitedStatesDollar;

        Assert.Equal("dollar", dollar.MajorUnitSingularName);
        Assert.Equal("dollars", dollar.MajorUnitPluralName);
        Assert.Equal("cent", dollar.MinorUnitSingularName);
        Assert.Equal("cents", dollar.MinorUnitPluralName);
    }

    [Theory]
    [InlineData(null, "dollars", "cent", "cents")]
    [InlineData("", "dollars", "cent", "cents")]
    [InlineData("   ", "dollars", "cent", "cents")]
    [InlineData("dollar", null, "cent", "cents")]
    [InlineData("dollar", "dollars", null, "cents")]
    [InlineData("dollar", "dollars", "cent", null)]
    public void Constructor_RejectsNullOrWhitespaceNames(string? majorSingular, string? majorPlural, string? minorSingular, string? minorPlural)
    {
        Assert.ThrowsAny<ArgumentException>(() => new CurrencyWords(majorSingular!, majorPlural!, minorSingular!, minorPlural!));
    }

    [Fact]
    public void Constructor_CustomCurrency_RoundTripsThroughMoneyToWords()
    {
        var poundsAndPence = new CurrencyWords("pound", "pounds", "penny", "pence");

        string words = MoneyToWordsConverter.ToWords(2.05m, poundsAndPence);

        Assert.Equal("two pounds and five pence", words);
    }
}
