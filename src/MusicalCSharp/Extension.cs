using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace MusicalCSharp;

internal static class Extension
{
    extension(MemoryStream ms)
    {
        /// <summary>
        /// Write <paramref name="value"/> to the stream, as an unsigned 16-bit integer (little-endian).
        /// </summary>
        /// <param name="value"></param>
        public void WriteU16(ushort value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
            ms.Write(buffer);
        }

        /// <summary>
        /// Write <paramref name="value"/> to the stream, as an unsigned 32-bit integer (little-endian).
        /// </summary>
        /// <param name="value"></param>
        public void WriteU32(uint value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
            ms.Write(buffer);
        }

        /// <summary>
        /// Write <paramref name="value"/> to the stream, as an IEEE-754 floating point value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteAsBytes<T>(T value) where T : struct
        {
            Span<byte> bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1));
            ms.Write(bytes);
        }
    }
}
