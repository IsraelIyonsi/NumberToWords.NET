using NumberToWords.Internal;

namespace NumberToWords;

/// <summary>
/// Converts a decimal money amount into an English amount-in-words phrase suitable for invoices,
/// for example 1234.50 becomes "one thousand two hundred thirty-four naira and fifty kobo".
/// </summary>
public static class MoneyToWordsConverter
{
    private const int MinorUnitsPerMajorUnit = 100;
    private const int RoundToWholeMinorUnits = 0;
    private const int SingularUnitCount = 1;

    /// <summary>
    /// Converts <paramref name="amount"/> to an amount-in-words phrase.
    /// The amount is rounded to two decimal places (away from zero at the midpoint) before conversion,
    /// so a rounding carry into the major unit, such as 1.995 becoming "two dollars", is handled correctly.
    /// The minor-unit clause is omitted when the rounded amount has no minor units, for example
    /// 1234.00 becomes "one thousand two hundred thirty-four naira" with no trailing "and zero kobo".
    /// </summary>
    /// <param name="amount">The money amount to convert. Negative amounts are prefixed with "negative".</param>
    /// <param name="currency">The currency word set to use. Defaults to <see cref="CurrencyWords.NigerianNaira"/> when null.</param>
    /// <param name="style">The hundreds and-joining convention to use for both the major and minor number words.</param>
    /// <returns>The amount-in-words phrase.</returns>
    /// <exception cref="OverflowException">The amount, scaled to minor units, does not fit in a 64-bit integer.</exception>
    public static string ToWords(decimal amount, CurrencyWords? currency = null, NumberToWordsStyle style = NumberToWordsStyle.American)
    {
        CurrencyWords currencyWords = currency ?? CurrencyWords.NigerianNaira;
        bool isNegative = amount < 0;

        decimal absoluteAmount = Math.Abs(amount);
        decimal roundedMinorUnits = Math.Round(absoluteAmount * MinorUnitsPerMajorUnit, RoundToWholeMinorUnits, MidpointRounding.AwayFromZero);
        long totalMinorUnits = (long)roundedMinorUnits;

        long majorUnits = totalMinorUnits / MinorUnitsPerMajorUnit;
        int minorUnits = (int)(totalMinorUnits % MinorUnitsPerMajorUnit);

        string majorUnitName = majorUnits == SingularUnitCount ? currencyWords.MajorUnitSingularName : currencyWords.MajorUnitPluralName;
        string phrase = NumberToWordsConverter.ToWords(majorUnits, style) + NumberWordConstants.Space + majorUnitName;

        if (minorUnits != 0)
        {
            string minorUnitName = minorUnits == SingularUnitCount ? currencyWords.MinorUnitSingularName : currencyWords.MinorUnitPluralName;
            string minorWords = NumberToWordsConverter.ToWords(minorUnits, style);
            phrase += NumberWordConstants.Space + NumberWordConstants.And + NumberWordConstants.Space + minorWords + NumberWordConstants.Space + minorUnitName;
        }

        bool roundsToZero = totalMinorUnits == 0;
        return isNegative && !roundsToZero
            ? NumberWordConstants.NegativePrefix + NumberWordConstants.Space + phrase
            : phrase;
    }
}
