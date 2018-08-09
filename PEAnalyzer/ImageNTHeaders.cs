using System;
using System.Runtime.InteropServices;

namespace PEStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageNTHeaders : IImageStructure
    {
        public UInt32 Signature;
        public ImageFileHeader FileHeader;
        public IImageOptionalHeader OptionalHeader;

        public static int StructureSize = Marshal.SizeOf(typeof(ImageNTHeaders));
        public int MarshalSize => StructureSize;
    }

    public enum ImageMachine : UInt16
    {
        IMAGE_MACHINE_UNKNOWN = 0,
        I386 = 0x014c,
        R3000 = 0x0162,  // MIPS little-endian, 0x160 big-endian
        R4000 = 0x0166,  // MIPS little-endian
        R10000 = 0x0168,  // MIPS little-endian
        WCEMIPSV2 = 0x0169,  // MIPS little-endian WCE v2
        ALPHA = 0x0184,  // Alpha_AXP
        SH3 = 0x01a2,  // SH3 little-endian
        SH3DSP = 0x01a3,
        SH3E = 0x01a4,  // SH3E little-endian
        SH4 = 0x01a6,  // SH4 little-endian
        SH5 = 0x01a8,  // SH5
        ARM = 0x01c0,  // ARM Little-Endian
        THUMB = 0x01c2,
        AM33 = 0x01d3,
        POWERPC = 0x01F0,  // IBM PowerPC Little-Endian
        POWERPCFP = 0x01f1,
        IA64 = 0x0200,  // Intel 64
        MIPS16 = 0x0266,  // MIPS
        ALPHA64 = 0x0284,  // ALPHA64
        MIPSFPU = 0x0366,  // MIPS
        MIPSFPU16 = 0x0466,  // MIPS
        TRICORE = 0x0520,  // Infineon
        CEF = 0x0CEF,
        EBC = 0x0EBC,  // EFI Byte Code
        AMD64 = 0x8664,  // AMD64 (K8)
        M32R = 0x9041,  // M32R little-endian
        CEE = 0xC0EE
    }

    public enum ImageSubsystem : UInt16
    {
        IMAGE_SUBSYSTEM_UNKNOWN = 0,   // Unknown subsystem.
        NATIVE = 1,  // Image doesn't require a subsystem.
        WINDOWS_GUI = 2,   // Image runs in the Windows GUI subsystem.
        WINDOWS_CUI = 3,   // Image runs in the Windows character subsystem.
        OS2_CUI = 5,   // image runs in the OS/2 character subsystem.
        POSIX_CUI = 7,   // image runs in the Posix character subsystem.
        NATIVE_WINDOWS = 8,   // image is a native Win9x driver.
        WINDOWS_CE_GUI = 9,   // Image runs in the Windows CE subsystem.
        EFI_APPLICATION = 10,
        EFI_BOOT_SERVICE_DRIVER = 11,
        EFI_RUNTIME_DRIVER = 12,
        EFI_ROM = 13,
        XBOX = 14,
        WINDOWS_BOOT_APPLICATION = 16
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Characteristics : IImageStructure
    {
        public UInt16 RawData;
        public bool ImageFileRelocsStripped => (RawData & 0b0000_0000_0000_0001) != 0;
        public bool ImageFileExecutableImage => (RawData & 0b0000_0000_0000_0010) != 0;
        public bool ImageFileLineNumsStripped => (RawData & 0b0000_0000_0000_0100) != 0;
        public bool ImageFileLocalSysmStripped => (RawData & 0b0000_0000_0000_1000) != 0;
        public bool ImageFileAggresiveWsTrim => (RawData & 0b0000_0000_0001_0000) != 0;
        public bool ImageFileLargeeAddressAware => (RawData & 0b0000_0000_0010_0000) != 0;
        public bool ImageFileBytesReversedLo => (RawData & 0b0000_0000_1000_0000) != 0;
        public bool ImageFile32bitMachine => (RawData & 0b0000_0001_0000_0000) != 0;
        public bool ImageFileDebugStripped => (RawData & 0b0000_0010_0000_0000) != 0;
        public bool ImageFileRemovableRunFromSwap => (RawData & 0b0100_0000_0000_0000) != 0;
        public bool ImageFileNetRunFromSwap => (RawData & 0b0000_1000_0000_0000) != 0;
        public bool ImageFileSystem => (RawData & 0b0001_0000_0000_0000) != 0;
        public bool ImageFileDll => (RawData & 0b0010_0000_0000_0000) != 0;
        public bool ImageFileUpSystemOnly => (RawData & 0b0100_0000_0000_0000) != 0;
        public bool ImageFileBytesReversedHi => (RawData & 0b1000_0000_0000_0000) != 0;

        public static int StructureSize = Marshal.SizeOf(typeof(Characteristics));
        public int MarshalSize => StructureSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageFileHeader : IImageStructure
    {
        [MarshalAs(UnmanagedType.U2)]
        public ImageMachine Machine;
        public UInt16 NumberOfSections;
        public TimeDateStamp TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public Characteristics Characteristics;

        public static int StructureSize = Marshal.SizeOf(typeof(ImageFileHeader));
        public int MarshalSize => StructureSize;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageDataDirectory : IImageStructure
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;

        public static int StructureSize = Marshal.SizeOf(typeof(ImageDataDirectory));
        public int MarshalSize => StructureSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageDataDirectoryArray : IImageStructure
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = Count)]
        public ImageDataDirectory[] DataDirectory;

        public ImageDataDirectory Export => (DataDirectory?.Length ?? 0) > 0 ? DataDirectory[0] : Empty;
        public ImageDataDirectory Import => (DataDirectory?.Length ?? 0) > 1 ? DataDirectory[1] : Empty;
        public ImageDataDirectory Resource => (DataDirectory?.Length ?? 0) > 2 ? DataDirectory[2] : Empty;
        public ImageDataDirectory Exception => (DataDirectory?.Length ?? 0) > 3 ? DataDirectory[3] : Empty;
        public ImageDataDirectory Security => (DataDirectory?.Length ?? 0) > 4 ? DataDirectory[4] : Empty;
        public ImageDataDirectory BaseRelocationTable => (DataDirectory?.Length ?? 5) > 0 ? DataDirectory[5] : Empty;
        public ImageDataDirectory DebugDirectory => (DataDirectory?.Length ?? 0) > 6 ? DataDirectory[6] : Empty;
        public ImageDataDirectory CopyrightOrArchitectureSpecificData => (DataDirectory?.Length ?? 0) > 7 ? DataDirectory[7] : Empty;
        public ImageDataDirectory GlobalPtr => (DataDirectory?.Length ?? 0) > 8 ? DataDirectory[8] : Empty;
        public ImageDataDirectory TLSDirectory => (DataDirectory?.Length ?? 0) > 9 ? DataDirectory[9] : Empty;
        public ImageDataDirectory LoadConfigurationDirectory => (DataDirectory?.Length ?? 10) > 0 ? DataDirectory[10] : Empty;
        public ImageDataDirectory BoundImportDirectory => (DataDirectory?.Length ?? 0) > 11 ? DataDirectory[11] : Empty;
        public ImageDataDirectory ImportAddressTable => (DataDirectory?.Length ?? 0) > 12 ? DataDirectory[12] : Empty;
        public ImageDataDirectory DelayLoadImportDescriptors => (DataDirectory?.Length ?? 13) > 0 ? DataDirectory[13] : Empty;
        public ImageDataDirectory COMRuntimedescriptor => (DataDirectory?.Length ?? 0) > 14 ? DataDirectory[14] : Empty;
        public ImageDataDirectory Reserved => (DataDirectory?.Length ?? 0) > 15 ? DataDirectory[15] : Empty;

        private const int Count = 16;
        private static readonly ImageDataDirectory Empty = new ImageDataDirectory();
        public static int StructureSize = Marshal.SizeOf(typeof(ImageDataDirectoryArray));
        public int MarshalSize => StructureSize;
    }

    public enum OptionalMagic : UInt16
    {
        PE32 = 0x10b,
        PE64 = 0x20b,
        ROM = 0x107
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DllCharacteristics : IImageStructure
    {
        public UInt16 RawData;

        public bool ImageLibraryProcessInit => (RawData & 0b0000_0000_0000_0001) != 0;
        public bool ImageLibraryProcessTerm => (RawData & 0b0000_0000_0000_0010) != 0;
        public bool ImageLibraryThreadInit => (RawData & 0b0000_0000_0000_0100) != 0;
        public bool ImageLibraryThreadTerm => (RawData & 0b0000_0000_0000_1000) != 0;

        public bool ImageDllCharacteristicsDynamicBase => (RawData & 0b0000_0000_0100_0000) != 0;
        public bool ImageDllCharacteristicsForceInterity => (RawData & 0b0000_0000_1000_0000) != 0;
        public bool ImageDllCharacteristicsNxCompat => (RawData & 0b0000_0001_0000_0000) != 0;
        public bool ImageDllCharacteristicsNoIsolation => (RawData & 0b0000_0010_0000_0000) != 0;
        public bool ImageDllCharacteristicsSeh => (RawData & 0b0000_0100_0000_0000) != 0;
        public bool ImageDllCharacteristicsBind => (RawData & 0b0000_1000_0000_0000) != 0;

        public bool ImageDllCharacteristicsWdmDriver => (RawData & 0b0010_0000_0000_0000) != 0;

        public bool ImageDllCharacteristicsTerminalServerAware => (RawData & 0b1000_0000_0000_0000) != 0;

        public static int StructureSize = Marshal.SizeOf(typeof(DllCharacteristics));
        public int MarshalSize => StructureSize;
    }

    public interface IImageOptionalHeader : IImageStructure
    {
        OptionalMagic Magic { get; }
        byte MajorLinkerVersion { get; }
        byte MinorLinkerVersion { get; }
        uint SizeOfCode { get; }
        uint SizeOfInitializedData { get; }
        uint SizeOfUninitializedData { get; }
        uint AddressOfEntryPoint { get; }
        uint BaseOfCode { get; }
        uint BaseOfData { get; }
        ulong ImageBase { get; }
        uint SectionAlignment { get; }
        uint FileAlignment { get; }
        ushort MajorOperatingSystemVersion { get; }
        ushort MinorOperatingSystemVersion { get; }
        ushort MajorImageVersion { get; }
        ushort MinorImageVersion { get; }
        ushort MajorSubsystemVersion { get; }
        ushort MinorSubsystemVersion { get; }
        uint Win32VersionValue { get; }
        uint SizeOfImage { get; }
        uint SizeOfHeaders { get; }
        uint CheckSum { get; }
        ImageSubsystem Subsystem { get; }
        DllCharacteristics DllCharacteristics { get; }
        ulong SizeOfStackReserve { get; }
        ulong SizeOfStackCommit { get; }
        ulong SizeOfHeapReserve { get; }
        ulong SizeOfHeapCommit { get; }
        uint LoaderFlags { get; }
        uint NumberOfRvaAndSizes { get; }
        ImageDataDirectoryArray DataDirectoryArray { get; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageOptionalHeader64 : IImageOptionalHeader
    {
        [MarshalAs(UnmanagedType.U2)]
        private OptionalMagic magic;
        private byte majorLinkerVersion;
        private byte minorLinkerVersion;
        private UInt32 sizeOfCode;
        private UInt32 sizeOfInitializedData;
        private UInt32 sizeOfUninitializedData;
        private UInt32 addressOfEntryPoint;
        private UInt32 baseOfCode;
        private UInt64 imageBase;
        private UInt32 sectionAlignment;
        private UInt32 fileAlignment;
        private UInt16 majorOperatingSystemVersion;
        private UInt16 minorOperatingSystemVersion;
        private UInt16 majorImageVersion;
        private UInt16 minorImageVersion;
        private UInt16 majorSubsystemVersion;
        private UInt16 minorSubsystemVersion;
        private UInt32 win32VersionValue;
        private UInt32 sizeOfImage;
        private UInt32 sizeOfHeaders;
        private UInt32 checkSum;
        [MarshalAs(UnmanagedType.U2)]
        private ImageSubsystem subsystem;
        private DllCharacteristics dllCharacteristics;
        private UInt64 sizeOfStackReserve;
        private UInt64 sizeOfStackCommit;
        private UInt64 sizeOfHeapReserve;
        private UInt64 sizeOfHeapCommit;
        private UInt32 loaderFlags;
        private UInt32 numberOfRvaAndSizes;
        private ImageDataDirectoryArray dataDirectoryArray;

        public static int StructureSize => Marshal.SizeOf(typeof(ImageOptionalHeader64));
        public int MarshalSize => StructureSize;

        public OptionalMagic Magic => magic;
        public byte MajorLinkerVersion => majorLinkerVersion;
        public byte MinorLinkerVersion => minorLinkerVersion;
        public uint SizeOfCode => sizeOfCode;
        public uint SizeOfInitializedData => sizeOfInitializedData;
        public uint SizeOfUninitializedData => sizeOfUninitializedData;
        public uint AddressOfEntryPoint => addressOfEntryPoint;
        public uint BaseOfCode => baseOfCode;
        public ulong ImageBase => imageBase;
        public uint SectionAlignment => sectionAlignment;
        public uint FileAlignment => fileAlignment;
        public ushort MajorOperatingSystemVersion => majorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion => minorOperatingSystemVersion;
        public ushort MajorImageVersion => majorImageVersion;
        public ushort MinorImageVersion => minorImageVersion;
        public ushort MajorSubsystemVersion => majorSubsystemVersion;
        public ushort MinorSubsystemVersion => minorSubsystemVersion;
        public uint Win32VersionValue => win32VersionValue;
        public uint SizeOfImage => sizeOfImage;
        public uint SizeOfHeaders => sizeOfHeaders;
        public uint CheckSum => checkSum;
        public ImageSubsystem Subsystem => subsystem;
        public DllCharacteristics DllCharacteristics => dllCharacteristics;
        public ulong SizeOfStackReserve => sizeOfStackReserve;
        public ulong SizeOfStackCommit => sizeOfStackCommit;
        public ulong SizeOfHeapReserve => sizeOfHeapReserve;
        public ulong SizeOfHeapCommit => sizeOfHeapCommit;
        public uint LoaderFlags => loaderFlags;
        public uint NumberOfRvaAndSizes => numberOfRvaAndSizes;
        public ImageDataDirectoryArray DataDirectoryArray => dataDirectoryArray;

        public uint BaseOfData => throw new NotImplementedException();
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageOptionalHeader32 : IImageOptionalHeader
    {
        [MarshalAs(UnmanagedType.U2)]
        private OptionalMagic magic;
        private byte majorLinkerVersion;
        private byte minorLinkerVersion;
        private UInt32 sizeOfCode;
        private UInt32 sizeOfInitializedData;
        private UInt32 sizeOfUninitializedData;
        private UInt32 addressOfEntryPoint;
        private UInt32 baseOfCode;
        private UInt32 baseOfData;
        private UInt32 imageBase;
        private UInt32 sectionAlignment;
        private UInt32 fileAlignment;
        private UInt16 majorOperatingSystemVersion;
        private UInt16 minorOperatingSystemVersion;
        private UInt16 majorImageVersion;
        private UInt16 minorImageVersion;
        private UInt16 majorSubsystemVersion;
        private UInt16 minorSubsystemVersion;
        private UInt32 win32VersionValue;
        private UInt32 sizeOfImage;
        private UInt32 sizeOfHeaders;
        private UInt32 checkSum;
        [MarshalAs(UnmanagedType.U2)]
        private ImageSubsystem subsystem;
        private DllCharacteristics dllCharacteristics;
        private UInt32 sizeOfStackReserve;
        private UInt32 sizeOfStackCommit;
        private UInt32 sizeOfHeapReserve;
        private UInt32 sizeOfHeapCommit;
        private UInt32 loaderFlags;
        private UInt32 numberOfRvaAndSizes;
        private ImageDataDirectoryArray dataDirectoryArray;

        public static int StructureSize => Marshal.SizeOf(typeof(ImageOptionalHeader32));
        public int MarshalSize => StructureSize;

        public OptionalMagic Magic => magic;
        public byte MajorLinkerVersion => majorLinkerVersion;
        public byte MinorLinkerVersion => minorLinkerVersion;
        public uint SizeOfCode => sizeOfCode;
        public uint SizeOfInitializedData => sizeOfInitializedData;
        public uint SizeOfUninitializedData => sizeOfUninitializedData;
        public uint AddressOfEntryPoint => addressOfEntryPoint;
        public uint BaseOfCode => baseOfCode;
        public uint BaseOfData => baseOfData;
        public ulong ImageBase => imageBase;
        public uint SectionAlignment => sectionAlignment;
        public uint FileAlignment => fileAlignment;
        public ushort MajorOperatingSystemVersion => majorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion => minorOperatingSystemVersion;
        public ushort MajorImageVersion => majorImageVersion;
        public ushort MinorImageVersion => minorImageVersion;
        public ushort MajorSubsystemVersion => majorSubsystemVersion;
        public ushort MinorSubsystemVersion => minorSubsystemVersion;
        public uint Win32VersionValue => win32VersionValue;
        public uint SizeOfImage => sizeOfImage;
        public uint SizeOfHeaders => sizeOfHeaders;
        public uint CheckSum => checkSum;
        public ImageSubsystem Subsystem => subsystem;
        public DllCharacteristics DllCharacteristics => dllCharacteristics;
        public ulong SizeOfStackReserve => sizeOfStackReserve;
        public ulong SizeOfStackCommit => sizeOfStackCommit;
        public ulong SizeOfHeapReserve => sizeOfHeapReserve;
        public ulong SizeOfHeapCommit => sizeOfHeapCommit;
        public uint LoaderFlags => loaderFlags;
        public uint NumberOfRvaAndSizes => numberOfRvaAndSizes;
        public ImageDataDirectoryArray DataDirectoryArray => dataDirectoryArray;
    }
}
