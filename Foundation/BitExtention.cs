
    internal static partial class UInt32Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetHiPart(this uint i)
        {
            return (ushort)(i >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetLoPart(this uint i)
        {
            return (ushort)(i & 0xffff);
        }
    }
    internal static partial class Int32Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetHiPart(this int i)
        {
            return (ushort)(((uint)i) >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetLoPart(this int i)
        {
            return (ushort)(((uint)i) & 0xffff);
        }
    }
    internal static partial class UInt16Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetHiPart(this ushort i)
        {
            return (byte)(i >> 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetLoPart(this ushort i)
        {
            return (byte)(i & 0xff);
        }
    }