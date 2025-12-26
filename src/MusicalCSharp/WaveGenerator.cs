using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MusicalCSharp;

public static partial class WaveGenerator
{
    public static int GenUInt16(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, float, float, ushort> sampleFunc,
        Func<int, byte[]>? bufferProvider = null)
    {
        return GenInt(sampleRate, channels, seconds, sampleFunc, bufferProvider);
    }

    public static int GenInt32(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, float, float, int> sampleFunc,
        Func<int, byte[]>? bufferProvider = null)
    {
        return GenInt(sampleRate, channels, seconds, sampleFunc, bufferProvider);
    }

    public static int GenFloat32(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, float, float, float> sampleFunc,
        Func<int, byte[]>? bufferProvider = null)
    {
        return GenFloat(sampleRate, channels, seconds, sampleFunc, bufferProvider);
    }

    public static int GenFloat64(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, double, double, double> sampleFunc,
        Func<int, byte[]>? bufferProvider = null)
    {
        return GenFloat(sampleRate, channels, seconds, sampleFunc, bufferProvider);
    }

    public static int GenFloat<T>(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, T, T, T> sampleFunc,
        Func<int, byte[]>? bufferProvider = null)
        where T : struct, IBinaryFloatingPointIeee754<T>
    {
        if (typeof(T) == typeof(Half))
        {
            throw new NotSupportedException("16-bit floating point is not a valid sample format.");
        }

        int sampleCount = CalculateSampleCount(sampleRate, channels, seconds);
        using MemoryStream stream = CreateStreamAndWriteHeader<T>(sampleRate, channels, AudioFormat.Float, sampleCount, bufferProvider);
        WriteFloatingPointSamples(stream, sampleCount, sampleRate, (int)channels, sampleFunc);

        return (int)stream.Position;
    }

    public static int GenInt<T>(
        uint sampleRate,
        AudioChannels channels,
        float seconds,
        Func<int, int, float, float, T> sampleFunc,
        Func<int, byte[]>? bufferProvider)
        where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
    {
        int sampleCount = CalculateSampleCount(sampleRate, channels, seconds);
        using MemoryStream stream = CreateStreamAndWriteHeader<T>(sampleRate, channels, AudioFormat.PCM, sampleCount, bufferProvider);
        WriteIntegerSamples(stream, sampleCount, sampleRate, (int)channels, sampleFunc);

        return (int)stream.Position;
    }

    private static int CalculateSampleCount(uint sampleRate, AudioChannels channels, float seconds)
    {
        return (int)(sampleRate * (int)channels * seconds);
    }

    internal static MemoryStream CreateStreamAndWriteHeader<T>(
        uint sampleRate,
        AudioChannels channels,
        AudioFormat format,
        int sampleCount,
        Func<int, byte[]>? bufferProvider)
    {
        // Create header.
        WaveHeader header = CreateHeader<T>(sampleRate, format, channels, sampleCount);
        int bufferSize = (int)header.Size + 8;

        // Create stream.
        byte[] buffer = bufferProvider?.Invoke(bufferSize) ?? new byte[bufferSize];
        MemoryStream stream = new MemoryStream(buffer);

        // Write header to stream.
        Span<WaveHeader> headerSpan = MemoryMarshal.CreateSpan(ref header, 1);
        Span<byte> headerBytes = MemoryMarshal.Cast<WaveHeader, byte>(headerSpan);
        stream.Write(headerBytes);

        return stream;
    }

    public static void Generate<T>(uint sampleRate, AudioChannels channels, float seconds, Func<int, int, T, T, T> sampleFunc)
        where T : struct, IBinaryFloatingPointIeee754<T>
    {
        if (typeof(T) == typeof(Half))
        {
            throw new NotSupportedException("16-bit floating point is not a valid sample format.");
        }

        int sampleCount = (int)(sampleRate * (int)channels * seconds);
        WaveHeader header = CreateHeader<T>(sampleRate, AudioFormat.Float, channels, sampleCount);

        byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent((int)header.Size + 8);

        try
        {
            using MemoryStream ms = new MemoryStream(rentedBuffer);

            Span<WaveHeader> headerSpan = MemoryMarshal.CreateSpan(ref header, 1);
            Span<byte> headerBytes = MemoryMarshal.Cast<WaveHeader, byte>(headerSpan);
            ms.Write(headerBytes);

            // Sample data
            WriteFloatingPointSamples(ms, sampleCount, sampleRate, (int)channels, sampleFunc);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }
    }

    private static WaveHeader CreateHeader<T>(uint sampleRate, AudioFormat format, AudioChannels channels, int sampleCount)
    {
        const int bitsPerByte = 8;

        ushort bytesPerSample = (ushort)Unsafe.SizeOf<T>();
        ushort bitsPerSample = (ushort)(bytesPerSample * bitsPerByte);
        ushort bytesPerBlock = (ushort)((ushort)channels * bytesPerSample);
        uint bytesPerSecond = sampleRate * bytesPerBlock;

        uint samplesByteCount = (uint)(sampleCount * bytesPerSample);
        uint overallFileSize = (uint)(Unsafe.SizeOf<WaveHeader>() + samplesByteCount);

        return new WaveHeader
        {
            RiffMagicString = MemoryMarshal.Read<WaveHeaderMagicString>("RIFF"u8),
            Size = overallFileSize - 8,
            WaveMagicString = MemoryMarshal.Read<WaveHeaderMagicString>("WAVE"u8),
            FormatMagicString = MemoryMarshal.Read<WaveHeaderMagicString>("fmt "u8),
            ChunkSize = 16,
            Format = format,
            Channels = channels,
            SampleRate = sampleRate,
            BytesPerSecond = bytesPerSecond,
            BytesPerBlock = bytesPerBlock,
            BitsPerSample = bitsPerSample,
            DataMagicString = MemoryMarshal.Read<WaveHeaderMagicString>("data"u8),
            SamplesByteCount = samplesByteCount,
        };
    }

    private static void WriteFloatingPointSamples<T>(MemoryStream ms, int sampleCount, uint sampleRate, int channels, Func<int, int, T, T, T> func)
        where T : struct, IBinaryFloatingPointIeee754<T>
    {
        for (int i = 0; i < sampleCount; i++)
        {
            // The channel of the current sample.
            int channel = i % channels;

            // The index of the current sample.
            int sample = i / channels;

            // Create floating point variants of the sample and sample rate.
            T sampleFloat = T.CreateChecked(sample);
            T sampleRateFloat = T.CreateChecked(sampleRate);

            // One hertz, relative to the current sample.
            T hz = T.Tau * sampleFloat / sampleRateFloat;

            // Current time in seconds.
            T s = sampleFloat / sampleRateFloat;

            ms.WriteAsBytes(func(channel, sample, hz, s));
        }
    }

    private static void WriteIntegerSamples<T>(MemoryStream ms, int sampleCount, uint sampleRate, int channels, Func<int, int, float, float, T> func)
        where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
    {
        for (int i = 0; i < sampleCount; i++)
        {
            // The channel of the current sample.
            int channel = i % channels;

            // The index of the current sample.
            int sample = i / channels;

            // One hertz, relative to the current sample.
            float hz = float.Tau * sample / sampleRate;

            // Current time in seconds.
            float s = (float)sample / sampleRate;

            ms.WriteAsBytes(func(channel, sample, hz, s));
        }
    }
}
