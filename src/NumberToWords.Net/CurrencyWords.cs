namespace NumberToWords;

/// <summary>
/// Defines the major-unit and minor-unit noun forms used when rendering a money amount as words,
/// for example naira/kobo or dollars/cents. Singular and plural forms are provided independently
/// because some currencies (naira, kobo) are invariant while others (dollar, cent) are not.
/// </summary>
public sealed class CurrencyWords
{
    /// <summary>
    /// Creates a currency word set.
    /// </summary>
    /// <param name="majorUnitSingularName">Major unit name used when the major amount is exactly one, for example "dollar".</param>
    /// <param name="majorUnitPluralName">Major unit name used otherwise, for example "dollars".</param>
    /// <param name="minorUnitSingularName">Minor unit name used when the minor amount is exactly one, for example "cent".</param>
    /// <param name="minorUnitPluralName">Minor unit name used otherwise, for example "cents".</param>
    public CurrencyWords(string majorUnitSingularName, string majorUnitPluralName, string minorUnitSingularName, string minorUnitPluralName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(majorUnitSingularName);
        ArgumentException.ThrowIfNullOrWhiteSpace(majorUnitPluralName);
        ArgumentException.ThrowIfNullOrWhiteSpace(minorUnitSingularName);
        ArgumentException.ThrowIfNullOrWhiteSpace(minorUnitPluralName);

        MajorUnitSingularName = majorUnitSingularName;
        MajorUnitPluralName = majorUnitPluralName;
        MinorUnitSingularName = minorUnitSingularName;
        MinorUnitPluralName = minorUnitPluralName;
    }

    /// <summary>The major unit name used when the major amount is exactly one, for example "dollar".</summary>
    public string MajorUnitSingularName { get; }

    /// <summary>The major unit name used when the major amount is not exactly one, for example "dollars".</summary>
    public string MajorUnitPluralName { get; }

    /// <summary>The minor unit name used when the minor amount is exactly one, for example "cent".</summary>
    public string MinorUnitSingularName { get; }

    /// <summary>The minor unit name used when the minor amount is not exactly one, for example "cents".</summary>
    public string MinorUnitPluralName { get; }

    /// <summary>Nigerian naira and kobo. Both nouns are invariant, so the singular and plural forms are identical. This is the default currency.</summary>
    public static CurrencyWords NigerianNaira { get; } = new(
        majorUnitSingularName: "naira",
        majorUnitPluralName: "naira",
        minorUnitSingularName: "kobo",
        minorUnitPluralName: "kobo");

    /// <summary>United States dollars and cents.</summary>
    public static CurrencyWords UnitedStatesDollar { get; } = new(
        majorUnitSingularName: "dollar",
        majorUnitPluralName: "dollars",
        minorUnitSingularName: "cent",
        minorUnitPluralName: "cents");
}
