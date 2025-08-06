# C# Silliness

This repository contains various small projects that demonstrate ~~awful~~ *creative* ways of accomplishing various things in C#.

## Projects

### [StringMutability](./src/StringMutability/Program.cs)

In C#, [strings are immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), and [`const`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) fields may not be modified.

But can we change the value of a `const string` anyways?

**Conclusion:** Yes, we can get a read-write `Span<char>` from the `string`, and use it to modify the content of the string.

If we rely on string interning, we can even change the content of a string without directly referring to it, by modifying the content of a different interned string with the same content.

This approach does however come with the limitation that the length of the string cannot be expanded.

### [StringOverwriting](./src/StringOverwriting/Program.cs)

Can we change the value of [`string.Empty`](https://learn.microsoft.com/en-us/dotnet/api/system.string.empty), so it is no longer empty?

**Conclusion:** Yes. It is possible to access and modify the internal length counter of a string, as well as change the content of the string, thereby making the string longer.

This will overwrite any memory that is located right after the string. Doing so can result in a `fatal errors`, if the new string content is sufficiently long.

### [DateTimePastMaxValue](./src/DateTimePastMaxValue/Program.cs)

[`DateTime.MaxValue`](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.maxvalue) represents the date 23:59:59.9999999 UTC, December 31, 9999, the last representable instance in time before the year 10.000 begins.

That's a conveniently elegant value, and clearly not representative of the actual maximum value of the `DateTime` data structure. So what happens if we force it beyond its intended maximum value?

**Conclusion:** While doable, there is very limited practical use for this.

If you manipulate a `DateTime` struct by setting all its bytes to `byte.MaxValue`, you can get a `DateTime` that represents a date beyond `DateTime.MaxValue`.

Specifically, the true maximum value is roughly 13.24.02, November 8, 14614. That's additional 4614 years of technically representable moments in time.

To put that into perspective, going back 4614 years from today (in 2025) lands us in 2589 BC. That's roughly around the time that the [Great Pyramid of Giza](https://en.wikipedia.org/wiki/Great_Pyramid_of_Giza) was being constructed. That's a lot of time which is technically possible to represent with a `DateTime`, but apparently, calculating things that go 12589 years into the future was deemend "unnecessary" by the developers of .NET.

You can actually perform comparisons on such values, and it will correctly state that the value is greater than `DateTime.MaxValue`. Which is a bit funny.

However, invoking method calls on such an object which return a new `DateTime`, e.g. `AddYears`, is likely to throw an `ArgumentOutOfRangeException` if, as is likely, the resulting `DateTime` represents a date outside of [`DateTime.MinValue` - `DateTime.MaxValue`].

So while this technically allows you to represent an additional 4614 years worth of dates and times, performing any calculations on such an object is carries the risk of getting an `ArgumentOutOfRangeException` thrown your way, as punishment for your hubris.

Doable? Yes.

Useful? As expected, a resounding "no".

### [ByValueSleep](./src/ByValueSleep/Program.cs)

Value types are passed by value (shocking, I know), meaning that the time it takes to pass them to a method depends on the size of the structure.

If we were to create a really big struct, could we use this to approximate a 1-second delay simply by passing our struct around?

**Conclusion:** Yes, we can approximate a 1-second sleep simply by measuring the time it takes to pass a really bug `struct` around.

With a 100,000 byte struct, we can narrow down the approximate number of times we need to pass it around before reaching a roughly 1-second delay.

100,000 bytes is also well short of the usual 1 MB size of the execution stack, meaning we are completely safe from `StackOverflowException`.

This all of course depends on the particular CPU being used, what it is currently doing besides this project, and so on. But, technically, this works.

Note: This is one of the reasons why it is recommended that `struct` types should be small (16 bytes or less seems to be a common recommendation).

## Failed projects

These are projects that did not work out (which is probably for the best).

### [NegativeFieldOffset](./src/NegativeFieldOffset/Program.cs)

[`FieldOffsetAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.fieldoffsetattribute) does not support negative numbers, and will result in a `CS0591: Invalid value for argument to attribute`, preventing compilation.

But can we work around this by defining a type with a negative `FieldOffsetAttribute` at runtime?

**Conclusion:** No, if a negative number is supplied, an [`ArgumentException`](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception) will be thrown with the message "*Invalid custom attribute provided.*"

## License

The code in this repository is licensed under the [MIT License](./LICENSE).

But, like... please don't actually use any of this code.
