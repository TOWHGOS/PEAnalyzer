using System;
using System.Runtime.InteropServices;

namespace PEStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageDosHeader : IImageStructure
    {
        public UInt16 e_magic;
        public UInt16 e_cblp;
        public UInt16 e_cp;
        public UInt16 e_crlc;
        public UInt16 e_cparhdr;
        public UInt16 e_minalloc;
        public UInt16 e_maxalloc;
        public UInt16 e_ss;
        public UInt16 e_sp;
        public UInt16 e_csum;
        public UInt16 e_ip;
        public UInt16 e_cs;
        public UInt16 e_lfarlc;
        public UInt16 e_ovno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] e_res;
        public UInt16 e_oemid;
        public UInt16 e_oeminfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] e_res2;
        public UInt32 e_lfanew;
        
        public ushort MZSignature => e_magic;
        public ushort UsedBytesInTheLastPage => e_cblp;
        public ushort FileSizeInPages => e_cp;
        public ushort NumberOfRelocationItems => e_crlc;
        public ushort HeaderSizeInParagraphs => e_cparhdr;
        public ushort MinimumExtraParagraphs => e_minalloc;
        public ushort MaximumExtraParagraphs => e_maxalloc;
        public ushort InitialRelativeSS => e_ss;
        public ushort InitialSP => e_sp;
        public ushort Checksum => e_csum;
        public ushort InitialIP => e_ip;
        public ushort InitialRelativeCS => e_cs;
        public ushort AddressOfRelocationTable => e_lfarlc;
        public ushort OverlayNumber => e_ovno;
        public byte[] Reserved => e_res;
        public ushort OEMid => e_oemid;
        public ushort OEMinfo => e_oeminfo;
        public byte[] Reserved2 => e_res2;
        public uint AddressOfNewExeHeader => e_lfanew;

        public static int StructureSize => Marshal.SizeOf(typeof(ImageDosHeader));
        public int MarshalSize => StructureSize;
    }
}
