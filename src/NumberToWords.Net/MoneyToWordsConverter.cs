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
    /// The amount is rounded to two decimal places (away from zero at the midpoint, matching the standard
    /// invoice/cheque convention rather than <see cref="MidpointRounding.ToEven"/>) before conversion,
    /// so a rounding carry into the major unit, such as 1.995 becoming "two dollars", is handled correctly.
    /// Callers reconciling against <see cref="Math.Round(decimal, int)"/>'s banker's-rounding default should
    /// expect a one-minor-unit difference exactly at the midpoint.
    /// The minor-unit clause is omitted when the rounded amount has no minor units, for example
    /// 1234.00 becomes "one thousand two hundred thirty-four naira" with no trailing "and zero kobo".
    /// A negative amount whose magnitude is entirely minor units, such as -0.005 (which rounds to -0.01),
    /// omits the zero-valued major clause instead of producing the awkward "negative zero naira", so it
    /// becomes "negative one kobo" rather than "negative zero naira and one kobo".
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

        string minorUnitName = minorUnits == SingularUnitCount ? currencyWords.MinorUnitSingularName : currencyWords.MinorUnitPluralName;
        string minorWords = NumberToWordsConverter.ToWords(minorUnits, style);

        // A negative amount whose rounded major units are zero has no sensible "negative zero" to say;
        // the sign is carried entirely by the minor-unit clause instead, for example "negative one kobo".
        bool omitZeroMajorClause = isNegative && majorUnits == 0 && minorUnits != 0;

        string phrase;
        if (omitZeroMajorClause)
        {
            phrase = minorWords + NumberWordConstants.Space + minorUnitName;
        }
        else
        {
            string majorUnitName = majorUnits == SingularUnitCount ? currencyWords.MajorUnitSingularName : currencyWords.MajorUnitPluralName;
            phrase = NumberToWordsConverter.ToWords(majorUnits, style) + NumberWordConstants.Space + majorUnitName;

            if (minorUnits != 0)
            {
                phrase += NumberWordConstants.Space + NumberWordConstants.And + NumberWordConstants.Space + minorWords + NumberWordConstants.Space + minorUnitName;
            }
        }

        bool roundsToZero = totalMinorUnits == 0;
        return isNegative && !roundsToZero
            ? NumberWordConstants.NegativePrefix + NumberWordConstants.Space + phrase
            : phrase;
    }
}
