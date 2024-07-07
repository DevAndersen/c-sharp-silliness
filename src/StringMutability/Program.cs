using System.Runtime.InteropServices;

const string text = "Cupcake";

// Print a const string, call an innocent method, and then print the const string again.
// That should definitely print the same thing twice.

Console.WriteLine(text);
InnocentMethod();
Console.WriteLine(text);

Console.ReadLine();

static void InnocentMethod()
{
    string textOne = "Cupcake";
    string textTwo = "Muffin";

    // Get a ReadOnlyMemory<char> for the string, change it to a Memory<char>, and get a span for it.
    // The whole "ReadOnly" thing is more what you'd call a "guideline" than an actual rule.
    Span<char> span = MemoryMarshal.AsMemory(textOne.AsMemory()).Span;

    span.Clear();
    textTwo.CopyTo(span);
}
