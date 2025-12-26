using System.Buffers;
using System.Diagnostics;
using MusicalCSharp;

static float GetSampleF32(int channel, int sample, float hz, float seconds)
{
    float DoubleTone(float frequency) => float.Sin(frequency * hz + float.Sin(frequency * hz * 0.5F));
    float DropOff(float time, float drop) => 1 / (1 + float.Pow(time * drop, 3));

    return DoubleTone(277.183F)
         * DropOff(seconds, 3);
}

int writtenBytes;
byte[]? rentedBuffer = null;

try
{
    writtenBytes = WaveGenerator.GenFloat<float>(
        44100,
        AudioChannels.Stereo,
        2F,
        GetSampleF32,
        length => rentedBuffer = ArrayPool<byte>.Shared.Rent(length)
    );
}
finally
{
    if (rentedBuffer != null)
    {
        ArrayPool<byte>.Shared.Return(rentedBuffer);
    }
}

string filePath = "sample.wav";

// Save the audio file..
using FileStream fs = File.Open(filePath, FileMode.Create);
Span<byte> bufferSpan = rentedBuffer.AsSpan()[..(int)writtenBytes];
fs.Write(bufferSpan);

// Execute the audio file.
Process.Start(new ProcessStartInfo(filePath)
{
    UseShellExecute = true
});
