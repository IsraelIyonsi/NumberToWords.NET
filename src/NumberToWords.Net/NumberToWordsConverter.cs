using NumberToWords.Internal;

namespace NumberToWords;

/// <summary>
/// Converts 64-bit integers into English cardinal and ordinal words.
/// All output is culture-invariant American English spelling; pass
/// <see cref="NumberToWordsStyle.British"/> to each method to switch to British hundreds "and" grouping.
/// </summary>
public static class NumberToWordsConverter
{
    /// <summary>
    /// Converts <paramref name="number"/> to cardinal English words, for example
    /// 1234 becomes "one thousand two hundred thirty-four" in <see cref="NumberToWordsStyle.American"/>
    /// or "one thousand two hundred and thirty-four" in <see cref="NumberToWordsStyle.British"/>.
    /// </summary>
    /// <param name="number">The integer to convert. Any <see cref="long"/> value is supported, including <see cref="long.MinValue"/>.</param>
    /// <param name="style">The hundreds and-joining convention to use.</param>
    /// <returns>The lowercase, hyphenated cardinal word form of <paramref name="number"/>.</returns>
    public static string ToWords(long number, NumberToWordsStyle style = NumberToWordsStyle.American)
    {
        return CardinalWordFormatter.Format(number, style);
    }

    /// <summary>
    /// Converts <paramref name="number"/> to ordinal English words, for example
    /// 21 becomes "twenty-first" and 100 becomes "one hundredth".
    /// </summary>
    /// <param name="number">The integer to convert. Any <see cref="long"/> value is supported, including <see cref="long.MinValue"/>.</param>
    /// <param name="style">The hundreds and-joining convention to use.</param>
    /// <returns>The lowercase, hyphenated ordinal word form of <paramref name="number"/>.</returns>
    public static string ToOrdinalWords(long number, NumberToWordsStyle style = NumberToWordsStyle.American)
    {
        return OrdinalWordFormatter.Format(number, style);
    }
}
