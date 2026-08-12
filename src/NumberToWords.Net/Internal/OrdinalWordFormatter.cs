namespace NumberToWords.Internal;

/// <summary>
/// Converts a signed 64-bit integer into its English ordinal word form by
/// reusing <see cref="CardinalWordFormatter"/> and transforming only the final word.
/// </summary>
internal static class OrdinalWordFormatter
{
    private const string YSuffix = "y";

    /// <summary>Formats <paramref name="value"/> as ordinal words in the given style.</summary>
    internal static string Format(long value, NumberToWordsStyle style)
    {
        string cardinal = CardinalWordFormatter.Format(value, style);

        int lastSpaceIndex = cardinal.LastIndexOf(NumberWordConstants.Space, StringComparison.Ordinal);
        string prefixThroughLastSpace = lastSpaceIndex < 0 ? string.Empty : cardinal[..(lastSpaceIndex + 1)];
        string lastWord = lastSpaceIndex < 0 ? cardinal : cardinal[(lastSpaceIndex + 1)..];

        int hyphenIndex = lastWord.LastIndexOf(NumberWordConstants.Hyphen, StringComparison.Ordinal);
        string lastWordPrefixThroughHyphen = hyphenIndex < 0 ? string.Empty : lastWord[..(hyphenIndex + 1)];
        string wordToConvert = hyphenIndex < 0 ? lastWord : lastWord[(hyphenIndex + 1)..];

        return prefixThroughLastSpace + lastWordPrefixThroughHyphen + ToOrdinalWord(wordToConvert);
    }

    private static string ToOrdinalWord(string cardinalWord)
    {
        if (NumberWordConstants.IrregularOrdinals.TryGetValue(cardinalWord, out string? irregularOrdinal))
        {
            return irregularOrdinal;
        }

        if (cardinalWord.Length > 1 && cardinalWord.EndsWith(YSuffix, StringComparison.Ordinal))
        {
            return cardinalWord[..^1] + NumberWordConstants.TensOrdinalSuffix;
        }

        return cardinalWord + NumberWordConstants.RegularOrdinalSuffix;
    }
}
