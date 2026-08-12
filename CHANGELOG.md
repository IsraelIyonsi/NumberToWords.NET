# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-08-12

### Added

- `NumberToWordsConverter.ToWords(long, NumberToWordsStyle)`: cardinal English words for any signed 64-bit integer, including `long.MinValue`, with scale words through quintillion.
- `NumberToWordsConverter.ToOrdinalWords(long, NumberToWordsStyle)`: ordinal English words, including irregular forms (first, second, third, fifth, eighth, ninth, twelfth) and the "y" to "ieth" tens transformation (twenty to twentieth).
- `MoneyToWordsConverter.ToWords(decimal, CurrencyWords?, NumberToWordsStyle)`: amount-in-words phrases for invoices, with exact rounding of the fractional part to two decimal places (away from zero at the midpoint, so rounding carries correctly into the major unit) and an omitted minor-unit clause when the rounded amount has no minor units.
- `CurrencyWords`: configurable major/minor unit noun pairs with independent singular and plural forms; built-in `NigerianNaira` (default) and `UnitedStatesDollar` presets.
- `NumberToWordsStyle`: `American` (no "and") and `British` ("and" before the final sub-hundred group and after a nonzero hundreds digit) grouping conventions.
- Correct hyphenation of compound tens-and-ones (twenty-one), and no hyphen between a hundreds word and its remainder.
- Culture-invariant output: unaffected by the operating system locale or the current thread culture.
- Verified against a table-driven fixture of known cardinal, ordinal, and money values spanning teens, tens, hundreds, every scale boundary from thousand through quintillion, both American and British styles, and negative numbers including `long.MinValue`.
- Zero runtime dependencies; no reflection anywhere in the conversion path, so the library is trim-safe and AOT-safe.
- SourceLink (GitHub), deterministic CI builds and `.snupkg` symbol packages.
