namespace NumberToWords.Internal;

/// <summary>
/// Word tables and literals shared by the cardinal and ordinal formatters.
/// All values are culture-invariant American English spellings.
/// </summary>
internal static class NumberWordConstants
{
    /// <summary>Number of digits in one thousands group.</summary>
    internal const int GroupDigitCount = 3;

    /// <summary>Divisor that splits a magnitude into thousands groups.</summary>
    internal const ulong GroupDivisor = 1000UL;

    /// <summary>Upper bound (exclusive) of the "teen" word range.</summary>
    internal const int TeenRangeExclusiveUpperBound = 20;

    /// <summary>Value at and above which a group has a hundreds digit.</summary>
    internal const int HundredThreshold = 100;

    /// <summary>Divisor used to split a value into its hundreds digit and remainder.</summary>
    internal const int HundredDivisor = 100;

    /// <summary>Divisor used to split a two-digit remainder into its tens digit and ones digit.</summary>
    internal const int TenDivisor = 10;

    internal const string Zero = "zero";
    internal const string NegativePrefix = "negative";
    internal const string Hundred = "hundred";
    internal const string And = "and";
    internal const string Hyphen = "-";
    internal const string Space = " ";

    /// <summary>Words for 0 through 19, indexed by value.</summary>
    internal static readonly string[] OnesAndTeens =
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
    };

    /// <summary>Words for the tens digit (2 through 9), indexed by digit; indices 0 and 1 are unused.</summary>
    internal static readonly string[] Tens =
    {
        string.Empty, string.Empty, "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety",
    };

    /// <summary>Scale words for each thousands group, indexed by group position (0 = units group).</summary>
    internal static readonly string[] Scales =
    {
        string.Empty, "thousand", "million", "billion", "trillion", "quadrillion", "quintillion",
    };

    /// <summary>Irregular ordinal words that do not follow the "append th" rule.</summary>
    internal static readonly IReadOnlyDictionary<string, string> IrregularOrdinals = new Dictionary<string, string>
    {
        ["zero"] = "zeroth",
        ["one"] = "first",
        ["two"] = "second",
        ["three"] = "third",
        ["five"] = "fifth",
        ["eight"] = "eighth",
        ["nine"] = "ninth",
        ["twelve"] = "twelfth",
    };

    /// <summary>Suffix appended to regular cardinal words to form an ordinal.</summary>
    internal const string RegularOrdinalSuffix = "th";

    /// <summary>Suffix that replaces a trailing "y" on tens words (twenty becomes twentieth).</summary>
    internal const string TensOrdinalSuffix = "ieth";
}
