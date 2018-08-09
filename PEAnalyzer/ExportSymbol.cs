using System;

namespace PEStructure
{
    public class ExportSymbol
    {
        public string Name { get; set; }
        public UInt32 VirtualAddress { get; set; }
        public string JumpName { get; set; }
    }
}
