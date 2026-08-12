using System.Text;

namespace NumberToWords.Internal;

/// <summary>
/// Converts a signed 64-bit integer into its English cardinal word form.
/// </summary>
internal static class CardinalWordFormatter
{
    /// <summary>Formats <paramref name="value"/> as cardinal words in the given style.</summary>
    internal static string Format(long value, NumberToWordsStyle style)
    {
        if (value == 0)
        {
            return NumberWordConstants.Zero;
        }

        bool isNegative = value < 0;
        ulong magnitude = ToAbsoluteMagnitude(value);
        string words = FormatMagnitude(magnitude, style);

        return isNegative
            ? NumberWordConstants.NegativePrefix + NumberWordConstants.Space + words
            : words;
    }

    /// <summary>
    /// Converts a signed value to its unsigned magnitude without overflow,
    /// including the asymmetric case where <paramref name="value"/> equals <see cref="long.MinValue"/>.
    /// </summary>
    internal static ulong ToAbsoluteMagnitude(long value)
    {
        if (value == long.MinValue)
        {
            return (ulong)long.MaxValue + 1UL;
        }

        return value < 0 ? (ulong)(-value) : (ulong)value;
    }

    /// <summary>Formats an unsigned magnitude (no sign handling) as cardinal words.</summary>
    internal static string FormatMagnitude(ulong magnitude, NumberToWordsStyle style)
    {
        if (magnitude == 0)
        {
            return NumberWordConstants.Zero;
        }

        List<(int Value, int ScaleIndex)> groups = SplitIntoGroups(magnitude);
        List<string> segments = new(groups.Count);

        foreach ((int groupValue, int scaleIndex) in groups)
        {
            string groupWords = FormatGroupUnderThousand(groupValue, style);
            string scaleWord = NumberWordConstants.Scales[scaleIndex];
            segments.Add(scaleWord.Length == 0
                ? groupWords
                : groupWords + NumberWordConstants.Space + scaleWord);
        }

        return JoinSegments(segments, groups, style);
    }

    private static List<(int Value, int ScaleIndex)> SplitIntoGroups(ulong magnitude)
    {
        List<(int Value, int ScaleIndex)> groups = new();
        ulong remaining = magnitude;
        int scaleIndex = 0;

        while (remaining > 0)
        {
            int groupValue = (int)(remaining % NumberWordConstants.GroupDivisor);
            remaining /= NumberWordConstants.GroupDivisor;

            if (groupValue != 0)
            {
                groups.Add((groupValue, scaleIndex));
            }

            scaleIndex++;
        }

        groups.Reverse();
        return groups;
    }

    private static string JoinSegments(List<string> segments, List<(int Value, int ScaleIndex)> groups, NumberToWordsStyle style)
    {
        if (segments.Count == 1)
        {
            return segments[0];
        }

        (int lastValue, int lastScaleIndex) = groups[^1];
        bool lastGroupIsSubHundredUnits = lastScaleIndex == 0 && lastValue < NumberWordConstants.HundredThreshold;

        if (style == NumberToWordsStyle.British && lastGroupIsSubHundredUnits)
        {
            StringBuilder builder = new();
            for (int i = 0; i < segments.Count - 1; i++)
            {
                if (i > 0)
                {
                    builder.Append(NumberWordConstants.Space);
                }

                builder.Append(segments[i]);
            }

            builder.Append(NumberWordConstants.Space)
                .Append(NumberWordConstants.And)
                .Append(NumberWordConstants.Space)
                .Append(segments[^1]);

            return builder.ToString();
        }

        return string.Join(NumberWordConstants.Space, segments);
    }

    private static string FormatGroupUnderThousand(int value, NumberToWordsStyle style)
    {
        int hundredsDigit = value / NumberWordConstants.HundredDivisor;
        int remainder = value % NumberWordConstants.HundredDivisor;

        if (hundredsDigit == 0)
        {
            return FormatUnderHundred(remainder);
        }

        string hundredsPart = NumberWordConstants.OnesAndTeens[hundredsDigit] + NumberWordConstants.Space + NumberWordConstants.Hundred;

        if (remainder == 0)
        {
            return hundredsPart;
        }

        string remainderWords = FormatUnderHundred(remainder);
        string joiner = style == NumberToWordsStyle.British
            ? NumberWordConstants.Space + NumberWordConstants.And + NumberWordConstants.Space
            : NumberWordConstants.Space;

        return hundredsPart + joiner + remainderWords;
    }

    private static string FormatUnderHundred(int value)
    {
        if (value < NumberWordConstants.TeenRangeExclusiveUpperBound)
        {
            return NumberWordConstants.OnesAndTeens[value];
        }

        int tensDigit = value / NumberWordConstants.TenDivisor;
        int onesDigit = value % NumberWordConstants.TenDivisor;
        string tensWord = NumberWordConstants.Tens[tensDigit];

        return onesDigit == 0
            ? tensWord
            : tensWord + NumberWordConstants.Hyphen + NumberWordConstants.OnesAndTeens[onesDigit];
    }
}
