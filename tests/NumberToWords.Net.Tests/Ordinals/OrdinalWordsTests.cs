namespace NumberToWords.Tests.Ordinals;

public class OrdinalWordsTests
{
    [Theory]
    [InlineData(1, "first")]
    [InlineData(2, "second")]
    [InlineData(3, "third")]
    [InlineData(5, "fifth")]
    [InlineData(8, "eighth")]
    [InlineData(9, "ninth")]
    [InlineData(12, "twelfth")]
    public void ToOrdinalWords_IrregularOnes_UseIrregularSuffix(long number, string expected)
    {
        Assert.Equal(expected, NumberToWordsConverter.ToOrdinalWords(number));
    }

    [Theory]
    [InlineData(20, "twentieth")]
    [InlineData(30, "thirtieth")]
    [InlineData(90, "ninetieth")]
    public void ToOrdinalWords_ExactTens_ReplaceTrailingYWithIeth(long number, string expected)
    {
        Assert.Equal(expected, NumberToWordsConverter.ToOrdinalWords(number));
    }

    [Fact]
    public void ToOrdinalWords_CompoundNumber_OnlyLastWordBecomesOrdinal()
    {
        string ordinal = NumberToWordsConverter.ToOrdinalWords(21);

        Assert.Equal("twenty-first", ordinal);
    }

    [Fact]
    public void ToOrdinalWords_DefaultStyle_IsAmerican()
    {
        Assert.Equal(NumberToWordsConverter.ToOrdinalWords(121), NumberToWordsConverter.ToOrdinalWords(121, NumberToWordsStyle.American));
    }
}
