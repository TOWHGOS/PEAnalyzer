using System;

namespace PEStructure
{
    public class ImportSymbol
    {
        public string Name { get; set; }
        public ImageImportDescriptor ImportDescriptor { get; set; }
        public ImageImportByName[] ImportByName { get; set; }
    }

    public class ImageImportByName
    {
        public UInt32 Hint { get; set; }
        public string Name { get; set; }
    }
}
