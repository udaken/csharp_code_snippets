using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Buffers;
using System.Runtime.CompilerServices;

var sw = new Stopwatch();
sw.Start();
EnumerateFilesPinvoke("D:\\");
System.Console.WriteLine(sw.Elapsed);
sw.Reset();
sw.Start();
//EnumerateFilesBcl("D:\\");
System.Console.WriteLine(sw.Elapsed);

void EnumerateFilesBcl(string path)
{
    long size = 0;
    long count = 0;
    foreach (var f in Directory.EnumerateFiles(path, "*.*", new EnumerationOptions()
    {
        IgnoreInaccessible = true,
        ReturnSpecialDirectories = false,
        RecurseSubdirectories = true,
        MatchType = MatchType.Simple,
    }))
    {
        count++;
    }
    Console.WriteLine($"{size},{count}");
}

void EnumerateFilesPinvoke(string path)
{
    long size = 0;
    long count = 0;
    var e = new FileEnumerator(path, "*.*", new EnumerationOptions());
    foreach (var f in e)
    {
        //Console.WriteLine($"{f.Parent}\\{f.Name}");
        size += f.Length;
        count++;
        Debug.Assert(File.Exists($"{f.Parent}\\{f.Name}"));
    }
    Console.WriteLine($"{size},{count}");
}
readonly ref struct FileInfoSlim
{
    private readonly ReadOnlySpan<WIN32_FIND_DATA> _fd;
    public readonly ReadOnlySpan<char> Parent;
    public readonly ReadOnlySpan<char> Name;

    public FileInfoSlim(in WIN32_FIND_DATA fd, ReadOnlySpan<char> currentDir, ReadOnlySpan<char> name)
    {
        _fd = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in fd), 1);
        Parent = currentDir;
        Name = name;
        //Debug.Assert(File.Exists($"{Parent}\\{Name}"));
    }
    public long Length => (long)(_fd[0].nFileSizeLow | ((ulong)_fd[0].nFileSizeHigh << 32));

}

unsafe ref struct FileEnumerator
{
    private const int MAX_PATH = 260;
    private readonly string _pattern;
    ReadOnlyMemory<char> _current = default;
    ReadOnlySpan<char> _currentName = default;
    readonly Queue<ReadOnlyMemory<char>> _dirs = new();
    WIN32_FIND_DATA _fd = default;
    IntPtr _hFindFile = INVALID_HANDLE_VALUE;
    readonly EnumerationOptions _options;
    static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    static readonly int _flags = 0;//(int)FindFirstExFlags.FIND_FIRST_EX_LARGE_FETCH;

    static int BuildFindPattern(ReadOnlySpan<char> path, ReadOnlySpan<char> pattern, Span<char> buf)
    {
        path.CopyTo(buf);
        int len = path.Length;
        if (path[^1] != '\\')
        {
            buf[len] = '\\';
            len++;
        }
        pattern.CopyTo(buf[len..]);
        return (len + pattern.Length);
    }

    static ReadOnlySpan<char> TrimByNullChar(ReadOnlySpan<char> str)
    {
        return str.IndexOf('\0') is > -1 and var len ? str[..len] : str;
    }

    public FileEnumerator GetEnumerator() => this;

    public FileEnumerator(string path, string pattern, EnumerationOptions options)
    {
        _pattern = pattern;
        _dirs.Enqueue(path.AsMemory());
        _options = options;
        _filter = &AcceptFiles;
    }

    static bool AcceptFiles(in WIN32_FIND_DATA fd, EnumerationOptions options) =>
        (fd.dwFileAttributes & FileAttributes.Directory) == 0 && (fd.dwFileAttributes & options.AttributesToSkip) != options.AttributesToSkip;
    delegate*<in WIN32_FIND_DATA, EnumerationOptions, bool> _filter;

    public readonly FileInfoSlim Current => new FileInfoSlim(in _fd, _current.Span, _currentName);

    public void Dispose()
    {
        if (_hFindFile != INVALID_HANDLE_VALUE)
        {
            FindClose(_hFindFile);
            _hFindFile = INVALID_HANDLE_VALUE;
        }
    }

    public bool MoveNext()
    {
        if (_hFindFile != INVALID_HANDLE_VALUE)
        {
            fixed (WIN32_FIND_DATA* fd = &_fd)
                while (FindNextFile(_hFindFile, fd))
                {
                    var name = TrimByNullChar(MemoryMarshal.CreateReadOnlySpan(ref _fd.cFileName[0], MAX_PATH));
                    if ((fd->dwFileAttributes & FileAttributes.Directory) != 0 && !name.SequenceEqual(".") && !name.SequenceEqual(".."))
                    {
#if USE_ARRAYPOOL
                        var arr = ArrayPool<char>.Shared.Rent(MAX_PATH);
#else
                        var arr = new char[MAX_PATH];
#endif
                        int length = BuildFindPattern(_current.Span, name, arr);
                        ReadOnlyMemory<char> dir = arr.AsMemory()[..length];
                        Debug.Assert(Directory.Exists(dir.ToString()));
                        _dirs.Enqueue(dir);
                    }
                    if (_filter(*fd, _options))
                    {
                        Debug.Assert(File.Exists($"{_current}\\{name}"));
                        _currentName = name;
                        return true;
                    }
                }

            FindClose(_hFindFile);
            _hFindFile = INVALID_HANDLE_VALUE;
        }

        Span<char> path = stackalloc char[MAX_PATH];
        while (_dirs.TryDequeue(out var item))
        {
#if USE_ARRAYPOOL            
            if (MemoryMarshal.TryGetArray(_current, out var baseArray) && baseArray.Array != null)
                ArrayPool<char>.Shared.Return(baseArray.Array);
#endif
            _current = item;
            path[BuildFindPattern(item.Span, _pattern, path)] = '\0';

            fixed (char* ppath = &path[0])
            fixed (WIN32_FIND_DATA* fd = &_fd)
            {
                _hFindFile = FindFirstFileEx(ppath, FINDEX_INFO_LEVELS.FindExInfoBasic, fd, FINDEX_SEARCH_OPS.FindExSearchNameMatch, default, _flags);
                if (_hFindFile != INVALID_HANDLE_VALUE)
                {
                    do
                    {
                        var name = TrimByNullChar(MemoryMarshal.CreateReadOnlySpan(ref _fd.cFileName[0], MAX_PATH));
                        if ((fd->dwFileAttributes & FileAttributes.Directory) != 0 && !name.SequenceEqual(".") && !name.SequenceEqual(".."))
                        {
#if USE_ARRAYPOOL
                            var arr = ArrayPool<char>.Shared.Rent(MAX_PATH);
#else
                            var arr = new char[MAX_PATH];
#endif
                            int length = BuildFindPattern(_current.Span, name, arr);
                            _dirs.Enqueue(arr.AsMemory()[..length]);
                        }
                        if (_filter(*fd, _options))
                        {
                            Debug.Assert(File.Exists($"{_current}\\{name}"));
                            _currentName = name;
                            return true;
                        }
                    } while (FindNextFile(_hFindFile, fd));
                    FindClose(_hFindFile);
                    _hFindFile = INVALID_HANDLE_VALUE;
                }
                else if (!_options.IgnoreInaccessible)
                {
                    throw new System.ComponentModel.Win32Exception(); // TODO
                }
            }
        }
        return false;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }


    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr FindFirstFileEx(
         char* lpFileName,
         FINDEX_INFO_LEVELS fInfoLevelId,
         WIN32_FIND_DATA* lpFindFileData,
         FINDEX_SEARCH_OPS fSearchOp,
         IntPtr lpSearchFilter,
         int dwAdditionalFlags);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    static extern bool FindNextFile(IntPtr hFindFile, WIN32_FIND_DATA*
       lpFindFileData);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    static extern bool FindClose(IntPtr hFindFile);

    enum FINDEX_SEARCH_OPS
    {
        FindExSearchNameMatch = 0,
        FindExSearchLimitToDirectories = 1,
        FindExSearchLimitToDevices = 2
    }

    enum FINDEX_INFO_LEVELS
    {
        FindExInfoStandard = 0,
        FindExInfoBasic = 1
    }
    [Flags]
    enum FindFirstExFlags
    {
        None,
        FIND_FIRST_EX_CASE_SENSITIVE = 1,
        FIND_FIRST_EX_LARGE_FETCH = 2,
        FIND_FIRST_EX_ON_DISK_ENTRIES_ONLY = 4,
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
unsafe struct WIN32_FIND_DATA
{
    public FileAttributes dwFileAttributes;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
    public uint nFileSizeHigh;
    public uint nFileSizeLow;
    public uint dwReserved0;
    public uint dwReserved1;
    public fixed char cFileName[260];
    public fixed char cAlternateFileName[14];
    public uint dwFileType;
    public uint dwCreatorType;
    public uint wFinderFlags;
}
