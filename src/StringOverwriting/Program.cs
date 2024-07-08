// Does what it says on the tin. Nothing extraordinary going on here.
Console.WriteLine($"> '{string.Empty}'");

// Calls our magic method.
OverwriteString(string.Empty, "Weird");

// This now prints something different.
Console.WriteLine($"> '{string.Empty}'");
Console.ReadLine();

unsafe static void OverwriteString(string original, string replacement)
{
    // Get a pointer to the first character in the string.
    ReadOnlySpan<char> span = original.AsSpan();
    char* firstCharPtr = *(char**)&span;

    // Modify the length of the string (int located before the characters of the string).
    // If the string length is not modified, it wouldn't be possible to replace the value of the original string with the value of a longer string.
    int* stringLengthPtr = (int*)firstCharPtr - 1;
    *stringLengthPtr = replacement.Length;

    // Copy the characters of the replacement string onto the original string.
    // This can cause some very minor issues, such as a "Fatal error", if something important is located right after the string value in memory.
    // The longer the replacement string, the higher the odds of this happening. But, as they say: no guts, no glory.
    Span<char> charSpan = new Span<char>(firstCharPtr, replacement.Length);
    replacement.AsSpan().CopyTo(charSpan);
}
