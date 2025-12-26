namespace MusicalCSharp;

/// <summary>
/// Wave file header.
/// </summary>
internal readonly struct WaveHeader
{
    /// <summary>
    /// The magic string <c>"RIFF"</c> (UTF-8).
    /// </summary>
    public required WaveHeaderMagicString RiffMagicString { get; init; }

    /// <summary>
    /// The size of the file, minus 8.
    /// </summary>
    public required uint Size { get; init; }

    /// <summary>
    /// The magic string <c>"WAVE"</c> (UTF-8).
    /// </summary>
    public required WaveHeaderMagicString WaveMagicString { get; init; }

    /// <summary>
    /// The magic string <c>"fmt "</c> (UTF-8).
    /// </summary>
    public required WaveHeaderMagicString FormatMagicString { get; init; }

    /// <summary>
    /// The chunk size.
    /// </summary>
    public required uint ChunkSize { get; init; }

    /// <summary>
    /// The format of the audio samples.
    /// </summary>
    public required AudioFormat Format { get; init; }

    /// <summary>
    /// The number of audio channels.
    /// </summary>
    public required AudioChannels Channels { get; init; }

    /// <summary>
    /// The sample rate.
    /// </summary>
    /// <remarks>
    /// Prefer using commonly known values (<c>8000</c>, <c>44100</c>, etc.), as unexpected values can result in invalid files.
    /// </remarks>
    public required uint SampleRate { get; init; }

    /// <summary>
    /// The number of bytes per second of audio.
    /// </summary>
    public required uint BytesPerSecond { get; init; }

    /// <summary>
    /// The number of bytes per block.
    /// </summary>
    public required ushort BytesPerBlock { get; init; }

    /// <summary>
    /// The number of bits per sample.
    /// </summary>
    public required ushort BitsPerSample { get; init; }

    /// <summary>
    /// The magic string <c>"data"</c> (UTF-8).
    /// </summary>
    public required WaveHeaderMagicString DataMagicString { get; init; }

    /// <summary>
    /// The total number of bytes worth of sample data.
    /// </summary>
    public required uint SamplesByteCount { get; init; }
}
