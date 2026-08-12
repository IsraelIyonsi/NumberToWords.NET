namespace NumberToWords;

/// <summary>
/// Selects the English convention used to join a hundreds digit to the
/// remainder of its group, and the last group to the groups before it.
/// </summary>
public enum NumberToWordsStyle
{
    /// <summary>
    /// American style: no "and" is inserted anywhere.
    /// For example, 1234 becomes "one thousand two hundred thirty-four".
    /// </summary>
    American = 0,

    /// <summary>
    /// British style: "and" joins a hundreds digit to a nonzero remainder,
    /// and joins a final sub-hundred group to the groups before it.
    /// For example, 1234 becomes "one thousand two hundred and thirty-four",
    /// and 1001 becomes "one thousand and one".
    /// </summary>
    British = 1,
}
