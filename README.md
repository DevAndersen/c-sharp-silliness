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

## Failed projects

These are projects that did not work out (which is probably for the best).

### [NegativeFieldOffset](./src/NegativeFieldOffset/Program.cs)

[`FieldOffsetAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.fieldoffsetattribute) does not support negative numbers, and will result in a `CS0591: Invalid value for argument to attribute`, preventing compilation.

But can we work around this by defining a type with a negative `FieldOffsetAttribute` at runtime?

**Conclusion:** No, if a negative number is supplied, an [`ArgumentException`](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception) will be thrown with the message "*Invalid custom attribute provided.*"

## License

The code in this repository is licensed under the [MIT License](./LICENSE).

But, like... please don't actually use any of this code.
