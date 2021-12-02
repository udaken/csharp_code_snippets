using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System {
readonly unsafe struct @nint : IEquatable<@nint>, IComparable<@nint>
{
    public static readonly @nint Zero = default;
    public static @nint MaxValue
        => IntPtr.Size == 4 ?
        new @nint((Int32.MaxValue)) :
        new @nint((Int64.MaxValue));

    public static @nint MinValue
        => IntPtr.Size == 4 ?
        new @nint((Int32.MinValue)) :
        new @nint((Int64.MinValue));

    readonly IntPtr _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nint(void* p) => _value = new IntPtr(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nint(Int32 p) => _value = new IntPtr(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nint(Int64 p) 
        => _value = IntPtr.Size == 8 ? 
            new IntPtr(p) :
             Int32.MaxValue < p ?  new IntPtr((Int32)p) : throw new ArgumentException()   ;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator @nint(void* b) => new @nint(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Int32 (@nint b) => (Int32)(b._value);
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator @nint(Int32 b) => new @nint(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator void*(@nint b) => (b._value.ToPointer());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator +(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() + b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() + b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator +(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() + b)) :
        new @nint((void*)(a._value.ToInt64() + b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator -(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() - b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() - b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator -(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() - b)) :
        new @nint((void*)(a._value.ToInt64() - b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator *(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() * b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() * b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator *(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() * b)) :
        new @nint((void*)(a._value.ToInt64() * b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator /(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() / b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() / b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator /(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() / b)) :
        new @nint((void*)(a._value.ToInt64() / b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator &(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() & b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() & b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator &(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() & b)) :
        new @nint((void*)(a._value.ToInt64() & b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator %(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() % b._value.ToInt32())) :
        new @nint((void*)(a._value.ToInt64() % b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator %(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() % b)) :
        new @nint((void*)(a._value.ToInt64() % b));



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator >>(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() >> b)) :
        new @nint((void*)(a._value.ToInt64() >> b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator <<(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        new @nint((void*)(a._value.ToInt32() << b)) :
        new @nint((void*)(a._value.ToInt64() << b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator +(void* a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint(a) + b :
        new @nint(a) + b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator +(byte* a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint(a) + b :
        new @nint(a) + b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator -(void* a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint(a) - b :
        new @nint(a) - b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator -(byte* a, @nint b)
        => IntPtr.Size == 4 ?
        new @nint(a) - b :
        new @nint(a) - b;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator +(@nint a)
        => IntPtr.Size == 4 ?
        new @nint((Int32)(+a._value.ToInt32())) :
        new @nint((Int64)(+a._value.ToInt64()));
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator -(@nint a)
        => IntPtr.Size == 4 ?
        new @nint((Int32)(-a._value.ToInt32())) :
        new @nint((Int64)(-a._value.ToInt64()));
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator ~(@nint a)
        => IntPtr.Size == 4 ?
        new @nint((Int32)(~a._value.ToInt32())) :
        new @nint((Int64)(~a._value.ToInt64()));
    
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() < b._value.ToInt32())) :
        ((a._value.ToInt64() < b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() < b)) :
        ((a._value.ToInt64() < b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() <= b._value.ToInt32())) :
        ((a._value.ToInt64() <= b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() <= b)) :
        ((a._value.ToInt64() <= b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() > b._value.ToInt32())) :
        ((a._value.ToInt64() > b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() > b)) :
        ((a._value.ToInt64() > b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() >= b._value.ToInt32())) :
        ((a._value.ToInt64() >= b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() >= b)) :
        ((a._value.ToInt64() >= b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() == b._value.ToInt32())) :
        ((a._value.ToInt64() == b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() == b)) :
        ((a._value.ToInt64() == b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(@nint a, @nint b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() != b._value.ToInt32())) :
        ((a._value.ToInt64() != b._value.ToInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(@nint a, Int32 b)
        => IntPtr.Size == 4 ?
        ((a._value.ToInt32() != b)) :
        ((a._value.ToInt64() != b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator ++(@nint a)
        => IntPtr.Size == 4 ?
        new @nint(a._value.ToInt32() + 1 ) :
        new @nint(a._value.ToInt64() + 1 );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nint operator --(@nint a)
        => IntPtr.Size == 4 ?
        new @nint(a._value.ToInt32() - 1 ) :
        new @nint(a._value.ToInt64() - 1 );


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other) => other is @nint a && Equals(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(@nint other) => _value == other._value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(@nint  other) => this > other ? 1 : this < other ? -1 : 0;
}
readonly unsafe struct @nuint : IEquatable<@nuint>, IComparable<@nuint>
{
    public static readonly @nuint Zero = default;
    public static @nuint MaxValue
        => UIntPtr.Size == 4 ?
        new @nuint((UInt32.MaxValue)) :
        new @nuint((UInt64.MaxValue));

    public static @nuint MinValue
        => UIntPtr.Size == 4 ?
        new @nuint((UInt32.MinValue)) :
        new @nuint((UInt64.MinValue));

    readonly UIntPtr _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nuint(void* p) => _value = new UIntPtr(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nuint(UInt32 p) => _value = new UIntPtr(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public @nuint(UInt64 p) 
        => _value = UIntPtr.Size == 8 ? 
            new UIntPtr(p) :
             UInt32.MaxValue < p ?  new UIntPtr((UInt32)p) : throw new ArgumentException()   ;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator @nuint(void* b) => new @nuint(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator UInt32 (@nuint b) => (UInt32)(b._value);
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Int32 (@nuint b) => (Int32)(b._value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator @nuint(UInt32 b) => new @nuint(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator void*(@nuint b) => (b._value.ToPointer());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator +(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() + b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() + b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator +(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() + b)) :
        new @nuint((void*)(a._value.ToUInt64() + b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator -(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() - b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() - b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator -(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() - b)) :
        new @nuint((void*)(a._value.ToUInt64() - b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator *(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() * b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() * b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator *(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() * b)) :
        new @nuint((void*)(a._value.ToUInt64() * b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator /(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() / b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() / b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator /(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() / b)) :
        new @nuint((void*)(a._value.ToUInt64() / b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator &(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() & b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() & b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator &(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() & b)) :
        new @nuint((void*)(a._value.ToUInt64() & b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator %(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() % b._value.ToUInt32())) :
        new @nuint((void*)(a._value.ToUInt64() % b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator %(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() % b)) :
        new @nuint((void*)(a._value.ToUInt64() % b));



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator >>(@nuint a, Int32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() >> b)) :
        new @nuint((void*)(a._value.ToUInt64() >> b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator <<(@nuint a, Int32 b)
        => UIntPtr.Size == 4 ?
        new @nuint((void*)(a._value.ToUInt32() << b)) :
        new @nuint((void*)(a._value.ToUInt64() << b));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator +(void* a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint(a) + b :
        new @nuint(a) + b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator +(byte* a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint(a) + b :
        new @nuint(a) + b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator -(void* a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint(a) - b :
        new @nuint(a) - b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator -(byte* a, @nuint b)
        => UIntPtr.Size == 4 ?
        new @nuint(a) - b :
        new @nuint(a) - b;


            

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() < b._value.ToUInt32())) :
        ((a._value.ToUInt64() < b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() < b)) :
        ((a._value.ToUInt64() < b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() <= b._value.ToUInt32())) :
        ((a._value.ToUInt64() <= b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() <= b)) :
        ((a._value.ToUInt64() <= b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() > b._value.ToUInt32())) :
        ((a._value.ToUInt64() > b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() > b)) :
        ((a._value.ToUInt64() > b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() >= b._value.ToUInt32())) :
        ((a._value.ToUInt64() >= b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() >= b)) :
        ((a._value.ToUInt64() >= b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() == b._value.ToUInt32())) :
        ((a._value.ToUInt64() == b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() == b)) :
        ((a._value.ToUInt64() == b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(@nuint a, @nuint b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() != b._value.ToUInt32())) :
        ((a._value.ToUInt64() != b._value.ToUInt64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(@nuint a, UInt32 b)
        => UIntPtr.Size == 4 ?
        ((a._value.ToUInt32() != b)) :
        ((a._value.ToUInt64() != b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator ++(@nuint a)
        => UIntPtr.Size == 4 ?
        new @nuint(a._value.ToUInt32() + 1 ) :
        new @nuint(a._value.ToUInt64() + 1 );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static @nuint operator --(@nuint a)
        => UIntPtr.Size == 4 ?
        new @nuint(a._value.ToUInt32() - 1 ) :
        new @nuint(a._value.ToUInt64() - 1 );


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other) => other is @nuint a && Equals(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(@nuint other) => _value == other._value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(@nuint  other) => this > other ? 1 : this < other ? -1 : 0;
}
}