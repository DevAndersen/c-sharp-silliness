# C# Silliness

This repository contains various small projects that demonstrate ~~awful~~ *creative* things one can do with C#.

## Successful projects

### [StringMutability](./src/StringMutability)

Unsafely mutating a string to change its contents (no `unsafe` required).

### [StringOverwriting](./src/StringOverwriting)

Unsafely changing `string.Empty` to no longer be empty (and very likely causing a crash).

### [DateTimePastMaxValue](./src/DateTimePastMaxValue)

Creating a `DateTime` that exceeds `DateTime.MaxValue`.

### [ByValueSleep](./src/ByValueSleep)

Approximating a 1-second sleep by passing a `struct` around.

## Failed projects

These are projects that did not work out (which is probably for the best).

### [NegativeFieldOffset](./src/NegativeFieldOffset)

Extending a struct's memory layout backwards from its starting location.

## License

The code in this repository is licensed under the [MIT License](./LICENSE).

But, like... please don't actually use any of this code.
