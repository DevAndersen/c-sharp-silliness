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
    // We assume that a string with this exact value ís already interned.
    // That way, both this string as well as the const string will refer to the same location in memory.
    string sameText = "Cupcake";

    // Get a ReadOnlyMemory<char> for the string, change it to a Memory<char>, and get a span for it.
    // The whole "ReadOnly" thing is more what you'd call a "guideline" than an actual rule.
    Span<char> span = MemoryMarshal.AsMemory(sameText.AsMemory()).Span;

    // Copy the value of a new string onto the span over the old string.
    string newText = "Muffin";
    span.Clear();
    newText.CopyTo(span);
}
