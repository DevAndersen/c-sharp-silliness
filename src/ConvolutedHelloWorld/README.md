# ConvolutedHelloWorld

The aim of this project is to write a ["Hello, World!"](https://en.wikipedia.org/wiki/%22Hello,_World!%22_program) application in C#.

The twist: make it as convoluted as possible.

This will be done by defining a number of methods, each of which returns one or a few characters, which, when joined together, forms the text `Hello, World!`, with that exact capitalization and punctuation, and print it to the console.

Goals:

- Print the text `Hello, World!` to the console.
- Avoid reusing the same "tricks", as much as is reasonably possible.
- Each line of code must do something productive. That means, methods or loops that do not contribute to the final result are prohibited.
- Everything must be done entirely within the Base Class Library (BCL). No NuGet packages, no P/Invoke, no depending on the underlying OS, environment, or file system.
- Everything else is fair game, no matter if it's bad practice, stupid, or borderline illegal.

## Conclusion

This project took significantly longer to make than expected, mostly because I wanted to try to be creative with how I implemented each of the methods.

The aim was to write code that no sane person would ever write. If all of the methods make you go "*What is wrong with you?*", then I have succeeded in my goal.

I also tried to make the comments both informative and humorous. I just wrote what felt funny in the moment, so the tone swings from "*this is perfectly normal*" to "*this is awful*".

Looking throught he code, it looks like I managed to utilize the following:

- MD5 hashing
- Gzip compression
- Endianness
- Unicode case bit
- LINQ
- Dynamic
- Linear interpolation
- Character normalization
- Extension methods
- Duck typing (`foreach`, `await`, `await foreach`, and `using`)
- Implicit casting
- Bitwise operators
- Expressions
- Regular expressions
- UTF-8 character literals
- URL encoding
- Assembly creation at runtime
- Implementing a method body with IL emit
- IEEE 754 (Standard for Floating-Point Arithmetic) bit layout
- Indexers
- String mutation
- Integer literals (hexadecimal, binary)
- UTF-8 string literals
- Overlapping fields
- Pointers
- Internal memory layout of `string`
- Function pointers

I'm reasonably satisfied with the variety in techniques used, as I tried to avoid using the same tricks over and over again (ignoring integer-to-character conversations, seeing as they're kinda necessary).

My favorite part was abusing contextual keywords and duck typing. I am equal parts proud of, and disgusted by, managing to turn the following code into valid C#:

```csharp
await foreach (int async in await await (int)nint)
{
    var ^= -await async & await (await await await async * ~await await async);
}
```

The `Hell` method, which was the first method I started working on, ironically took the longest because I stopped halfway through when I had a managed to turn 270,291,474 into "hell", but starting with some big random number that seemed a bit too simple.

This project also spawned [`InstanceOfVoid`](../InstanceOfVoid). I initially planned on using `System.Void` to somehow arrive at a space character (void to space, seemed funny). But it turns out that merely looking at that type funny is enough to make the compiler and runtime stare angrily at you while cracking their knuckles. So I had to settle with URL encoding (which then necessitated coming up with a creative way of getting a plus character).

But ultimately, the project succeeded, and I'm satisfied with the result.
