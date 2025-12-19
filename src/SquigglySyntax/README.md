# SquigglySyntax

Imagine you wanted to write a [Brainfuck](https://en.wikipedia.org/wiki/Brainfuck) implementation in C#, but you were too lazy to actually do it.

What might you end up with? What compromises might you make? How does C# limit the syntax?

Well, C# does let you concatinate certain unary operators, so... Let's give it a go!

## Conclusion

I managed to make the following unreadable mess, which writes "Hello, World!" (in UTF-8) to a byte array.

```csharp
// "Hello, World!"
Squiggler t = new Squiggler(13);

t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~t;
t = +~~!t;
t = +~~!+~+~+~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~t;
t = +~~!-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~t;
```

Note: This could in theory be all one line, but was split on a one-line-per-character basis. This both helps readability, and avoids `CS8078: An expression is too long or complex to compile`.

`t` is an object of type `Squiggler` (lots of squiggly lines, hence the name). This type has four members:

- A byte array, used as the work/output buffer.
- An int which stores the current position.
- An int which stores the current value.
- An enum which specifies its current mode.
  - 0: Undefined (initial state)
  - 1: Change: Sets the value at the current position to the current value
  - 2: Move: Moves the current position.

The type also defines the following unary operators:

- `~`: Increments the current mode by one
- `+`: Increments the current value/position, depending on the mode, and resets the mode.
- `-`: Decrements the current value/position, depending on the mode, and resets the mode.
- `!`: Stores the current value into the current position.

Note: The `+` and `-` here are the *unary* operators, used to indicate if a number is positive (+5, redundant) or negative (-5). This is not to be confused with the *binary* operators that use the same symbols, typically used to add/subtract (numbers), subscribe/unsubscribe (events), or concatinate (strings).

Because these unary operators all work as prefixes, the syntax is read right-to-left.

The reason why the `+` and `-` operators reset the current mode is because `++` and `--` would otherwise be reasonable to use, which would conflict with the `++` and `--` operators.

Example:

```csharp
t = +~~!+~t;
```

Explanation (right-to-left):

- `t`: The object being worked on.
- `~`: Increment the mode by one (Undefined -> Change)
- `+`: Increment the current value (because mode is "Change") by one, and reset the mode
- `!`: Store the current value (1) to the current position (0)
- `~~`: Increment the mode by two (Undefined -> Move)
- `+`: Increment the current position (because mode is "Move") by one, and reset the mode

In short, this stores the value 1 into position 0, and moves the current position to 1.
