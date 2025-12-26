using System.Runtime.CompilerServices;

namespace MusicalCSharp;

/// <summary>
/// A 4-byte magic string (UTF-8).
/// </summary>
[InlineArray(Size)]
internal struct WaveHeaderMagicString
{
    public const int Size = 4;

    private byte _element0;
}
