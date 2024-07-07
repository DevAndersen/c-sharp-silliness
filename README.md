# C# Silliness

This repository contains various small projects that demonstrate ~~awful~~ *creative* ways of accomplishing various things in C#.

## Projects

### StringMutability

In C#, [strings are immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), and [`const`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const) fields may not be modified.

But what if we change the value of a `const string` anyways?

### StringOverwriting

Changing the value of [`string.Empty`](https://learn.microsoft.com/en-us/dotnet/api/system.string.empty), so it is no longer empty.

May contain fatal errors.

## License

The code in this repository is licensed under the [MIT License](./LICENSE).

But, like... please don't actually use any of this code.
