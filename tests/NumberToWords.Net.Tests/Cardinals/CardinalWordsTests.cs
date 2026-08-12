using System.Globalization;

namespace NumberToWords.Tests.Cardinals;

public class CardinalWordsTests
{
    [Fact]
    public void ToWords_DefaultStyle_IsAmerican()
    {
        Assert.Equal(NumberToWordsConverter.ToWords(1234), NumberToWordsConverter.ToWords(1234, NumberToWordsStyle.American));
    }

    [Fact]
    public void ToWords_LongMinValue_DoesNotThrowAndProducesNegativeWords()
    {
        string words = NumberToWordsConverter.ToWords(long.MinValue);

        Assert.StartsWith("negative ", words, StringComparison.Ordinal);
        Assert.EndsWith("eight", words, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(21, "twenty-one")]
    [InlineData(99, "ninety-nine")]
    [InlineData(1234, "one thousand two hundred thirty-four")]
    public void ToWords_CompoundTensAndOnes_AreHyphenated(long number, string expected)
    {
        string words = NumberToWordsConverter.ToWords(number);

        Assert.Equal(expected, words);
    }

    [Theory]
    [InlineData(20, "twenty")]
    [InlineData(100, "one hundred")]
    public void ToWords_ExactTensOrHundreds_HaveNoHyphen(long number, string expected)
    {
        string words = NumberToWordsConverter.ToWords(number);

        Assert.Equal(expected, words);
        Assert.DoesNotContain('-', words);
    }

    [Fact]
    public void ToWords_NoHyphenBetweenHundredAndRemainder()
    {
        string words = NumberToWordsConverter.ToWords(1234);

        Assert.DoesNotContain("hundred-", words, StringComparison.Ordinal);
    }

    [Fact]
    public void ToWords_IsCultureInvariant_AcrossThreadCulture()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            string words = NumberToWordsConverter.ToWords(1234567);

            Assert.Equal("one million two hundred thirty-four thousand five hundred sixty-seven", words);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
