using System;
using System.Runtime.InteropServices;

namespace PEStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageExportDirectory : IImageStructure
    {
        public UInt32 Characteristics;
        public TimeDateStamp TimeDateStamp;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Name;
        public UInt32 Base;
        public UInt32 NumberOfFunctions;
        public UInt32 NumberOfNames;
        public UInt32 AddressOfFunctions;
        public UInt32 AddressOfNames;
        public UInt32 AddressOfNameOrdinals;
       
        public static int StructureSize = Marshal.SizeOf(typeof(ImageExportDirectory));
        public int MarshalSize => StructureSize;
    }
}
