#nullable enable

using System.Runtime.InteropServices;

#if NET6_0_OR_GREATER
public unsafe sealed class SafeNativeMemory : SafeBuffer
{
    public static SafeNativeMemory? Alloc(nuint byteCount, bool zeroed = true)
    {
        var p = zeroed ? NativeMemory.AllocZeroed(byteCount) : NativeMemory.Alloc(byteCount);
        return p != null ? new SafeNativeMemory(p, byteCount) : null;
    }
    public static SafeNativeMemory? Alloc(nuint elementCount, nuint elementSize, bool zeroed = true)
    {
        var p = zeroed ? NativeMemory.AllocZeroed(elementCount, elementSize) : NativeMemory.Alloc(elementCount, elementSize);
        return p != null ? new SafeNativeMemory(p, elementCount * elementSize) : null;
    }

    public SafeNativeMemory() : base(false)
    {
    }

    private SafeNativeMemory(void* pointer, ulong num) : base(true)
    {
        handle = new IntPtr(pointer);
        Initialize(num);
    }

    protected override bool ReleaseHandle()
    {
        NativeMemory.Free(handle.ToPointer());
        return true;
    }
}

public unsafe sealed class SafeAlignedNativeMemory : SafeBuffer
{
    public static SafeAlignedNativeMemory? Alloc(nuint elementCount, nuint elementSize)
    {
        var p = NativeMemory.AlignedAlloc(elementCount, elementSize);
        return p != null ? new SafeAlignedNativeMemory(p, elementCount * elementSize) : null;
    }

    public SafeAlignedNativeMemory() : base(false)
    {
    }

    private SafeAlignedNativeMemory(void* pointer, ulong num) : base(true)
    {
        handle = new IntPtr(pointer);
        Initialize(num);
    }

    protected override bool ReleaseHandle()
    {
        NativeMemory.AlignedFree(handle.ToPointer());
        return true;
    }
}
#endif