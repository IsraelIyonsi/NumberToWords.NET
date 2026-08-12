namespace NumberToWords.Tests.Support;

internal static class FixtureFile
{
    private const char FieldSeparator = '\t';
    private const char CommentPrefix = '#';

    internal static IEnumerable<object[]> ReadRows(string fixtureFileName, int expectedFieldCount)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "fixtures", fixtureFileName);

        foreach (string line in File.ReadLines(path))
        {
            if (line.Length == 0 || line[0] == CommentPrefix)
            {
                continue;
            }

            string[] fields = line.Split(FieldSeparator);
            if (fields.Length != expectedFieldCount)
            {
                throw new InvalidDataException(
                    $"Fixture '{fixtureFileName}' line has {fields.Length} fields, expected {expectedFieldCount}: '{line}'");
            }

            yield return fields;
        }
    }

    internal static NumberToWordsStyle ParseStyle(string styleToken)
    {
        return Enum.Parse<NumberToWordsStyle>(styleToken);
    }
}
