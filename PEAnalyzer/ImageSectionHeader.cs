using System;
using System.Runtime.InteropServices;

namespace PEStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageSectionHeader : IImageStructure
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Name;
        public Misc Misc;

        public UInt32 VirtualAddress;
        public UInt32 SizeOfRawData;
        public UInt32 PointerToRawData;
        public UInt32 PointerToRelocations;
        public UInt32 PointerToLinenumbers;
        public UInt16 NumberOfRelocations;
        public UInt16 NumberOfLinenumbers;
        public SectionCharacteristics Characteristics;

        public static int StructureSize = Marshal.SizeOf(typeof(ImageSectionHeader));
        public int MarshalSize => StructureSize;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Misc : IImageStructure
    {
        [FieldOffset(0)]
        public UInt32 PhysicalAddress;
        [FieldOffset(0)]
        public UInt32 VirtualSize;

        public static int StructureSize = Marshal.SizeOf(typeof(Misc));
        public int MarshalSize => StructureSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SectionCharacteristics : IImageStructure
    {
        public UInt32 RawData;

        public bool ImageScnTypeDsect => (RawData & 0b0000_0000_0000_0000_0000_0000_0000_0001) != 0;
        public bool ImageScnTypeNoload => (RawData & 0b0000_0000_0000_0000_0000_0000_0000_0010) != 0;
        public bool ImageScnTypeGroup => (RawData & 0b0000_0000_0000_0000_0000_0000_0000_0100) != 0;
        public bool ImageScnTypeNoPad => (RawData & 0b0000_0000_0000_0000_0000_0000_0000_1000) != 0;
        public bool ImageScnTypeCopy => (RawData & 0b0000_0000_0000_0000_0000_0000_0001_0000) != 0;

        public bool ImageScnCntCode => (RawData & 0b0000_0000_0000_0000_0000_0000_0010_0000) != 0;
        public bool ImageScnCntInitializedData => (RawData & 0b0000_0000_0000_0000_0000_0000_0100_0000) != 0;
        public bool ImageScnCntUninitializedData => (RawData & 0b0000_0000_0000_0000_0000_0000_1000_0000) != 0;

        public bool ImageScnLnkOther => (RawData & 0b0000_0000_0000_0000_0000_0001_0000_0000) != 0;
        public bool ImageScnLnkInfo => (RawData & 0b0000_0000_0000_0000_0000_0010_0000_0000) != 0;
        public bool ImageScnTypeOver => (RawData & 0b0000_0000_0000_0000_0000_0100_0000_0000) != 0;
        public bool ImageScnLnkRemove => (RawData & 0b0000_0000_0000_0000_0000_1000_0000_0000) != 0;
        public bool ImageScnLnkComdat => (RawData & 0b0000_0000_0000_0000_0001_0000_0000_0000) != 0;

        public bool ImageScnNoDeferSpecExec => (RawData & 0b0000_0000_0000_0000_0100_0000_0000_0000) != 0;
        public bool ImageScnGprel => (RawData & 0b0000_0000_0000_0000_1000_0000_0000_0000) != 0;
        public bool ImageScnSysheap => (RawData & 0b0000_0000_0000_0001_0000_0000_0000_0000) != 0;
        public bool ImageScn16bit => (RawData & 0b0000_0000_0000_0010_0000_0000_0000_0000) != 0;
        public bool ImageScnLocked => (RawData & 0b0000_0000_0000_0100_0000_0000_0000_0000) != 0;
        public bool ImageScnPreload => (RawData & 0b0000_0000_0000_1000_0000_0000_0000_0000) != 0;

        public bool ImageScnAlign1bytes => (RawData & 0b0000_0000_0001_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnAlign2bytes => (RawData & 0b0000_0000_0010_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnAlign8bytes => (RawData & 0b0000_0000_0100_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnAlign128bytes => (RawData & 0b0000_0000_1000_0000_0000_0000_0000_0000) != 0;

        public bool ImageScnLnkNrelocOvfl => (RawData & 0b0000_0001_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemDiscardable => (RawData & 0b0000_0010_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemNotCached => (RawData & 0b0000_0100_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemNotPaged => (RawData & 0b0000_1000_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemShared => (RawData & 0b0001_0000_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemExecute => (RawData & 0b0010_0000_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemRead => (RawData & 0b0100_0000_0000_0000_0000_0000_0000_0000) != 0;
        public bool ImageScnMemWrite => (RawData & 0b1000_0000_0000_0000_0000_0000_0000_0000) != 0;

        public static int StructureSize = Marshal.SizeOf(typeof(SectionCharacteristics));
        public int MarshalSize => StructureSize;
    }
}
