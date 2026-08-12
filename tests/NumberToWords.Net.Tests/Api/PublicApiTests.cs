using System.Reflection;

namespace NumberToWords.Tests.Api;

public class PublicApiTests
{
    private static readonly Assembly LibraryAssembly = typeof(NumberToWordsConverter).Assembly;

    [Fact]
    public void NumberToWordsConverter_IsStaticWithExpectedMethods()
    {
        Type type = typeof(NumberToWordsConverter);

        Assert.True(type.IsAbstract && type.IsSealed);
        AssertHasMethod(type, nameof(NumberToWordsConverter.ToWords), typeof(long), typeof(NumberToWordsStyle));
        AssertHasMethod(type, nameof(NumberToWordsConverter.ToOrdinalWords), typeof(long), typeof(NumberToWordsStyle));
    }

    [Fact]
    public void MoneyToWordsConverter_IsStaticWithExpectedMethod()
    {
        Type type = typeof(MoneyToWordsConverter);

        Assert.True(type.IsAbstract && type.IsSealed);
        AssertHasMethod(type, nameof(MoneyToWordsConverter.ToWords), typeof(decimal), typeof(CurrencyWords), typeof(NumberToWordsStyle));
    }

    [Fact]
    public void NumberToWordsStyle_HasAmericanAndBritishValues()
    {
        var values = Enum.GetValues<NumberToWordsStyle>();

        Assert.Contains(NumberToWordsStyle.American, values);
        Assert.Contains(NumberToWordsStyle.British, values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void CurrencyWords_ExposesFourReadOnlyStringProperties()
    {
        Type type = typeof(CurrencyWords);
        string[] expectedProperties =
        {
            nameof(CurrencyWords.MajorUnitSingularName),
            nameof(CurrencyWords.MajorUnitPluralName),
            nameof(CurrencyWords.MinorUnitSingularName),
            nameof(CurrencyWords.MinorUnitPluralName),
        };

        foreach (string propertyName in expectedProperties)
        {
            PropertyInfo? property = type.GetProperty(propertyName);
            Assert.NotNull(property);
            Assert.Equal(typeof(string), property!.PropertyType);
            Assert.False(property.CanWrite);
        }
    }

    [Fact]
    public void InternalTypes_AreNotPartOfThePublicSurface()
    {
        Type[] publicTypes = LibraryAssembly.GetExportedTypes();

        Assert.DoesNotContain(publicTypes, type => type.Namespace == "NumberToWords.Internal");
    }

    private static void AssertHasMethod(Type type, string methodName, params Type[] parameterTypes)
    {
        MethodInfo? method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, parameterTypes);
        Assert.NotNull(method);
    }
}
