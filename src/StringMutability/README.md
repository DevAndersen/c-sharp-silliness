# StringMutability

In C#, [strings are immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), and [`const`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) fields may not be modified.

But can we change the value of a `const string` anyways?

**Conclusion:** Yes, we can get a read-write `Span<char>` from the `string`, and use it to modify the content of the string.

If we rely on string interning, we can even change the content of a string without directly referring to it, by modifying the content of a different interned string with the same content.

This approach does however come with the limitation that the length of the string cannot be expanded.
