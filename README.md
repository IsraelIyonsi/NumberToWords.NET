# NumberToWords.NET

Convert numbers to English words and money amounts to words for invoices in .NET: cardinals, ordinals and currency including Nigerian Naira and kobo. Zero dependencies.

Every business that prints an invoice, a cheque, or a legal document eventually needs a line like "One Thousand Two Hundred and Thirty-Four Naira, Fifty Kobo Only". Most number-to-words packages on NuGet are either abandoned, hard-coded to US dollars, or silently wrong at the boundaries: they drop the hyphen in "twenty-one", mishandle `long.MinValue`, or round `1.995` down to `1.99` instead of carrying it to `2.00`. NumberToWords.NET is a small, dependency-free library that gets those details right and is verified against a table-driven fixture of known values.

## Install

```
dotnet add package NumberToWords.Net
```

## Usage

### Cardinal numbers

```csharp
using NumberToWords;

string words = NumberToWordsConverter.ToWords(1234);
// one thousand two hundred thirty-four

string british = NumberToWordsConverter.ToWords(1234, NumberToWordsStyle.British);
// one thousand two hundred and thirty-four
```

### Ordinal numbers

```csharp
using NumberToWords;

string ordinal = NumberToWordsConverter.ToOrdinalWords(21);
// twenty-first

string hundredth = NumberToWordsConverter.ToOrdinalWords(100);
// one hundredth
```

### Money amount-in-words for invoices

```csharp
using NumberToWords;

string naira = MoneyToWordsConverter.ToWords(1234.50m);
// one thousand two hundred thirty-four naira and fifty kobo

string dollars = MoneyToWordsConverter.ToWords(1234.50m, CurrencyWords.UnitedStatesDollar);
// one thousand two hundred thirty-four dollars and fifty cents

string pounds = MoneyToWordsConverter.ToWords(2.05m, new CurrencyWords("pound", "pounds", "penny", "pence"));
// two pounds and five pence
```

`MoneyToWordsConverter.ToWords` rounds the amount to two decimal places (away from zero at the midpoint) before conversion, so a rounding carry into the major unit is handled correctly: `1.995m` becomes "two naira", not "one naira and ninety-nine kobo point five". This is the standard invoice/cheque rounding convention, and is deliberately **not** `MidpointRounding.ToEven` (banker's rounding), which is what `decimal.Round`/`Math.Round` use by default. If you reconcile amounts-in-words against a value you separately rounded with `Math.Round(amount, 2)`, expect a one-minor-unit difference exactly at the midpoint (for example `1.005m`: this library says "one dollar and one cent", `Math.Round` with its default `ToEven` says `1.00`).

The minor-unit clause is omitted entirely when the rounded amount has no minor units, so `1234.00m` becomes "one thousand two hundred thirty-four naira" with no trailing "and zero kobo". A negative amount that rounds to zero major units, such as `-0.005m` (which rounds to -0.01), omits the zero-valued major clause rather than saying the nonsensical "negative zero naira": it becomes "negative one kobo".

## API

| Type | Purpose |
|---|---|
| `NumberToWordsConverter.ToWords(long, NumberToWordsStyle)` | Cardinal words for a signed 64-bit integer |
| `NumberToWordsConverter.ToOrdinalWords(long, NumberToWordsStyle)` | Ordinal words for a signed 64-bit integer |
| `MoneyToWordsConverter.ToWords(decimal, CurrencyWords?, NumberToWordsStyle)` | Amount-in-words phrase for a money value |
| `CurrencyWords` | Major/minor unit noun pair, with singular and plural forms; presets `NigerianNaira` (default) and `UnitedStatesDollar` |
| `NumberToWordsStyle` | `American` (no "and") or `British` ("and" before the final sub-hundred group and after a nonzero hundreds digit) |

## Style: American vs British

- `American` (default): `1234` becomes "one thousand two hundred thirty-four", `1001` becomes "one thousand one".
- `British`: `1234` becomes "one thousand two hundred and thirty-four", `1001` becomes "one thousand and one".

The style only changes where "and" appears inside the number itself. The word joining the currency's major and minor units, for example "... naira **and** fifty kobo", is always present when both units are nonzero, regardless of style. Because British style already inserts its own "and" before a final sub-hundred group, combining it with a nonzero minor-unit clause produces two consecutive "and"s: `MoneyToWordsConverter.ToWords(1000001.01m, style: NumberToWordsStyle.British)` becomes "one million and one naira and one kobo". This is the correct, unambiguous reading of both conventions applied independently, not a bug.

## Notes and limitations

- Supports the full `long` range, including `long.MinValue`, and scale words through quintillion, which comfortably covers `long.MaxValue` (about 9.2 quintillion).
- Culture-invariant: output does not depend on the operating system locale or the current thread culture. There is no localization to languages other than English.
- Zero runtime dependencies. Built entirely on the .NET base class library.
- No reflection is used anywhere in the conversion path, so the library is trim-safe and AOT-safe.
- `MoneyToWordsConverter.ToWords` throws `OverflowException` if the amount, scaled to minor units, does not fit in a 64-bit integer. This only matters for amounts in the quintillions and is not a realistic invoice value.
- `CurrencyWords` lets you define any currency's major/minor unit nouns; the library does not attempt to enumerate real-world currencies.

## License

MIT. See [LICENSE](LICENSE).
