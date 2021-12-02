using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace BRWExt
{
    public static class BinaryReaderExtension
    {
        public static sbyte[] ReadSBytes(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadSByte, count);
        }
        public static ushort[] ReadUInt16s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadUInt16, count);
        }
        public static uint[] ReadUInt32s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadUInt32, count);
        }
        public static ulong[] ReadUInt64s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadUInt64, count);
        }
        public static short[] ReadInt16s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadInt16, count);
        }
        public static int[] ReadInt32s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadInt32, count);
        }
        public static long[] ReadInt64s(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadInt64, count);
        }
        public static float[] ReadSingles(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadSingle, count);
        }
        public static double[] ReadDoubles(this BinaryReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException();

            return ReadArray(reader.ReadDouble, count);
        }
        public static T[] ReadArray<T>(Func<T> func, int count) where T : struct
        {
            if (func == null)
                throw new ArgumentNullException("func");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = func();
            }
            return result;
        }
        public static IEnumerable<T> ReadMultiple<T>(Func<T> func, int count) where T : struct
        {
            if (func == null)
                throw new ArgumentNullException("func");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0; i < count; i++)
            {
                yield return func();
            }
        }
        public static byte[] ReadAnsiString(this BinaryReader reader, int initCapacity = 64)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var buf = new List<byte>(initCapacity);
            do
            {
                var b = reader.ReadByte();
                if (b == 0)
                    break;

                buf.Add(b);
            } while (true);

            return buf.ToArray();
        }
        public static string ReadAnsiString(this BinaryReader reader, Encoding enc, int initCapacity = 64)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var buf = new byte[initCapacity];
            int count;
            for (count = 0; ; count++)
            {
                var b = reader.ReadByte();
                if (b == 0)
                    break;

                if (buf.Length < count)
                    Array.Resize(ref buf, buf.Length + 64);
                buf[count] = b;
            }

            return enc.GetString(buf, 0, count);
        }
        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            byte[] buff = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }
    }

    public static class BinaryWriterExtension
    {
        public static void WriteSBytes(this BinaryWriter writer, sbyte[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteUInt16s(this BinaryWriter writer, ushort[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteUInt32s(this BinaryWriter writer, uint[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteUInt64s(this BinaryWriter writer, ulong[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteInt16s(this BinaryWriter writer, short[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteInt32s(this BinaryWriter writer, int[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteInt64s(this BinaryWriter writer, long[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteSingles(this BinaryWriter writer, float[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteDoubles(this BinaryWriter writer, double[] values, int offset, int count)
        {
            if (writer == null)
                throw new ArgumentNullException();

            WriteArray(writer.Write, values, offset, count);
        }
        public static void WriteArray<T>(Action<T> func, T[] values, int offset, int count) where T : struct
        {
            if (func == null)
                throw new ArgumentNullException("func");
            if (values == null)
                throw new ArgumentNullException("values");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            for (int i = offset; i < count; i++)
            {
                func(values[i]);
            }
        }
    }
}