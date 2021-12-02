using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace UnrarWwrapper
{
    public enum RarErrorCode : uint
    {
        Success = 0,
        EndArchive = 10,
        NoMemory = 11,
        BadData = 12,
        BadArchive = 13,
        UnknownFormat = 14,
        EOpen = 15,
        ECreate = 16,
        EClose = 17,
        ERead = 18,
        EWrite = 19,
        SmallBuf = 20,
        Unknown = 21,
        MissingPassword = 22,
        EReference = 23,
        BadPassword = 24,
    }

    public enum RarOpenMode : uint
    {
        List = 0,
        Extract = 1,
        ListIncsplit  = 2,
    }

    public enum RarOperation : int
    {
        Skip = 0,
        Test = 1,
        Extract = 2,
    }

    public enum RarVol
    {
        Ask = 0,
        Notify = 1,
    }

    public enum RarHash : uint
    {
        None = 0,
        Crc32 = 1,
        Blake2 = 2,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct RARHeaderData
    {
        /// <summary>
        /// char         ArcName[260];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string ArcName;
        /// <summary>
        /// char         FileName[260];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string FileName;
        public uint Flags;
        public uint PackSize;
        public uint UnpSize;
        public uint HostOS;
        public uint FileCRC;
        public uint FileTime;
        public uint UnpVer;
        public uint Method;
        public uint FileAttr;
        /// <summary>
        /// char         *CmtBuf;
        /// </summary>
        public IntPtr CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct NameAnsi
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string Value;

        public NameAnsi(string s)
        {
            Value = s;
        }
        public static implicit operator string(NameAnsi _this)
        {
            return _this.Value;
        }
        public static implicit operator NameAnsi(string s)
        {
            return new NameAnsi(s);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct RARHeaderDataEx
    {
        /// <summary>
        /// char         ArcName[1024];
        /// </summary>
        public NameAnsi ArcName;
        /// <summary>
        /// wchar_t ArcNameW[1024];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string ArcNameW;
        /// <summary>
        /// char FileName[1024];
        /// </summary>
        public NameAnsi FileName;
        /// <summary>
        /// wchar_t FileNameW[1024];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string FileNameW;
        public uint Flags;
        public uint PackSize;
        public uint PackSizeHigh;
        public uint UnpSize;
        public uint UnpSizeHigh;
        public uint HostOS;
        public uint FileCRC;
        public uint FileTime;
        public uint UnpVer;
        public uint Method;
        public uint FileAttr;
        /// <summary>
        /// char* CmtBuf;
        /// </summary>
        public IntPtr CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
        public uint DictSize;
        public RarHash HashType;
        /// <summary>
        /// char Hash[32];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] Hash;
        public uint RedirType;
        /// <summary>
        /// wchar_t* RedirName;
        /// </summary>
        public string RedirName;
        public uint RedirNameSize;
        public uint DirTarget;
        /// <summary>
        /// unsigned int Reserved[994];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 994)]
        public uint[] Reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct RAROpenArchiveData
    {
        /// <summary>
        /// char* ArcName;
        /// </summary>
        public string ArcName;
        public uint OpenMode;
        public RarErrorCode OpenResult;
        /// <summary>
        /// char* CmtBuf;
        /// </summary>
        public string CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
    };

    /// <summary>
    /// typedef int (CALLBACK *UNRARCALLBACK)(UINT msg,LPARAM UserData,LPARAM P1,LPARAM P2);
    /// </summary>
    /// <returns></returns>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int UNRARCALLBACK(uint msg, IntPtr UserData, IntPtr P1, IntPtr P2);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct RAROpenArchiveDataEx
    {
        /// <summary>
        /// char* ArcName;
        /// </summary>
        IntPtr ArcName;
        /// <summary>
        /// wchar_t* ArcNameW;
        /// </summary>
        public string ArcNameW;
        public RarOpenMode OpenMode;
        public RarErrorCode OpenResult;
        /// <summary>
        /// char* CmtBuf;
        /// </summary>
        public IntPtr CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
        public uint Flags;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public UNRARCALLBACK Callback;
        public IntPtr UserData;
        /// <summary>
        /// unsigned int Reserved[28];
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public uint[] Reserved;
    };

    public enum UNRARCALLBACK_MESSAGES
    {
        UCM_CHANGEVOLUME, UCM_PROCESSDATA, UCM_NEEDPASSWORD, UCM_CHANGEVOLUMEW,
        UCM_NEEDPASSWORDW
    };

    /// <summary>
    /// typedef int (PASCAL* CHANGEVOLPROC)(char* ArcName,int Mode);
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate int CHANGEVOLPROC(string ArcName, int Mode);
    /// <summary>
    /// typedef int (PASCAL* PROCESSDATAPROC)(unsigned char* Addr,int Size);
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int PROCESSDATAPROC(byte[] Addr, int Size);

    public class SafeArchiveHandle : SafeHandle
    {
        internal SafeArchiveHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override bool ReleaseHandle()
        {
            RarErrorCode result = UnRARNative.RARCloseArchive(this);
            return result == RarErrorCode.Success;
        }
    }
    public class UnrarException : Exception
    {
        RarErrorCode code;
        public UnrarException(RarErrorCode code)
        {
            this.code = code;
        }
    }
    public class UnRarEntry
    {

    }
    public class UnRAR : IDisposable
    {
        private RAROpenArchiveDataEx archiveData;
        private RARHeaderDataEx headerData;
        private SafeArchiveHandle handle;

        private List<UnRarEntry> entries = new List<UnRarEntry>();

        public void Dispose()
        {
            handle.Dispose();
        }
        
        public static UnRAR Open(string path)
        {
            var archive = new UnRAR();
            archive.archiveData.ArcNameW = path;
            archive.archiveData.Callback = archive.UNRARCALLBACK;
            archive.handle = UnRARNative.RAROpenArchiveEx(ref archive.archiveData);
            if(archive.handle.IsInvalid)
            {
                throw new UnrarException(archive.archiveData.OpenResult);
            }
            archive.Init();
            return archive;
        }

        private int UNRARCALLBACK(uint msg, IntPtr UserData, IntPtr P1, IntPtr P2)
        {
            return 0;
        }

        private void Init(Func<int, bool> callback = null)
        {
            for (int i = 0;;i++)
            {
                if (callback != null)
                    if (!callback(i))
                        break;

                var result = UnRARNative.RARReadHeaderEx(handle, ref headerData);
                switch(result)
                {
                    case RarErrorCode.Success:
                        entries.Add(new UnRarEntry());
                        continue;
                    case RarErrorCode.EndArchive:
                        return;
                    default:
                        throw new UnrarException(result);
                }
            }
        }

        public ulong UnpackSize
        {
            get
            {
                return (ulong)headerData.UnpSizeHigh << 32 | headerData.UnpSize;
            }
        }

        private void GetHeaders()
        {
        }

        public IEnumerable<UnRarEntry> Entries
        {
            get
            {
                UnRARNative.RARProcessFileW(handle, RarOperation.Skip,null, null);
                yield return null;
            }
        }

        public static int DllVersion
        {
            get
            {
                return UnRARNative.RARGetDllVersion();
            }
        }
    }
    internal static class UnRARNative
    {
#if x64
        public const string LibraryName = "unrar64.dll";
#else
        public const string LibraryName = "unrar.dll";
#endif
        /// <summary>
        /// HANDLE PASCAL RAROpenArchive(struct RAROpenArchiveData *ArchiveData);
        /// </summary>
        /// <param name="ArchiveData"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern SafeArchiveHandle RAROpenArchive(ref RAROpenArchiveData ArchiveData);
        /// <summary>
        /// HANDLE PASCAL RAROpenArchiveEx(struct RAROpenArchiveDataEx *ArchiveData);
        /// </summary>
        /// <param name="ArchiveData"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern SafeArchiveHandle RAROpenArchiveEx(ref RAROpenArchiveDataEx ArchiveData);
        /// <summary>
        /// int    PASCAL RARCloseArchive(HANDLE hArcData);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern RarErrorCode RARCloseArchive(SafeArchiveHandle hArcData);
        /// <summary>
        /// int    PASCAL RARReadHeader(HANDLE hArcData,struct RARHeaderData *HeaderData);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="HeaderData"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern RarErrorCode RARReadHeader(SafeArchiveHandle hArcData,ref RARHeaderData HeaderData);
        /// <summary>
        /// int    PASCAL RARReadHeaderEx(HANDLE hArcData,struct RARHeaderDataEx *HeaderData);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="HeaderData"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern RarErrorCode RARReadHeaderEx(SafeArchiveHandle hArcData,ref RARHeaderDataEx HeaderData);
        /// <summary>
        /// int    PASCAL RARProcessFile(HANDLE hArcData,int Operation,char *DestPath,char *DestName);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="Operation"></param>
        /// <param name="DestPath"></param>
        /// <param name="DestName"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern RarErrorCode RARProcessFile(SafeArchiveHandle hArcData, RarOperation Operation, string DestPath, string DestName);
        /// <summary>
        /// int    PASCAL RARProcessFileW(HANDLE hArcData,int Operation,wchar_t *DestPath,wchar_t *DestName);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="Operation"></param>
        /// <param name="DestPath"></param>
        /// <param name="DestName"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern RarErrorCode RARProcessFileW(SafeArchiveHandle hArcData, RarOperation Operation, string DestPath, string DestName);
        /// <summary>
        /// void   PASCAL RARSetCallback(HANDLE hArcData,UNRARCALLBACK Callback,LPARAM UserData);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="Callback"></param>
        /// <param name="UserData"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern void RARSetCallback(SafeArchiveHandle hArcData, UNRARCALLBACK Callback, IntPtr UserData);
        /// <summary>
        /// void   PASCAL RARSetChangeVolProc(HANDLE hArcData,CHANGEVOLPROC ChangeVolProc);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="ChangeVolProc"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern void RARSetChangeVolProc(SafeArchiveHandle hArcData, CHANGEVOLPROC ChangeVolProc);
        /// <summary>
        /// void   PASCAL RARSetProcessDataProc(HANDLE hArcData,PROCESSDATAPROC ProcessDataProc);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="ProcessDataProc"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern void RARSetProcessDataProc(SafeArchiveHandle hArcData, PROCESSDATAPROC ProcessDataProc);
        /// <summary>
        /// void   PASCAL RARSetPassword(HANDLE hArcData,char *Password);
        /// </summary>
        /// <param name="hArcData"></param>
        /// <param name="Password"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void RARSetPassword(SafeArchiveHandle hArcData, string Password);
        /// <summary>
        /// int    PASCAL RARGetDllVersion();
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        public static extern int RARGetDllVersion();
    }
}