using System;
using System.Runtime.InteropServices;

namespace PEStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ImageImportDescriptor:IImageStructure
    {
        public DummyUnion DummyUnionName;
        public TimeDateStamp TimeDateStamp;
        public UInt32 ForwarderChain;
        public UInt32 Name;
        public UInt32 FirstThunk;

        public static int StructureSize = Marshal.SizeOf(typeof(ImageImportDescriptor));
        public int MarshalSize => StructureSize;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DummyUnion
    {
        [FieldOffset(0)]
        public UInt32 Characteristics;
        [FieldOffset(0)]
        public UInt32 OriginalFirstThunk;
    }

}
