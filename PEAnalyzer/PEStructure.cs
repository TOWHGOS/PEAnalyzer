using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PEStructure
{
    public class PEAnalyzer
    {
        public string FilePath { get; private set; }

        public ImageDosHeader DosHeader { get; private set; }
        public byte[] DosStub { get; private set; }
        public ImageNTHeaders NTHeader { get; private set; }
        public ImageSectionHeader[] SectionHeaders { get; private set; }
        public ImageExportDirectory ExportDirectory { get; private set; }
        public ExportSymbol[] ExportSymbols { get; private set; }
        public ImportSymbol[] ImportSymbols { get; private set; }
        public string Name { get; private set; }
        public string[] ExportNames { get; private set; }
        public string[] ImportNames { get; private set; }
        public bool HasExport => ExportSymbols?.Length > 0;
        public bool HasImport => ImportSymbols?.Length > 0;

        private BinaryReader BinaryReader;

        public PEAnalyzer(string path)
        {
            FilePath = path;
            Analyze();
        }

        public static PEAnalyzer Analyze(string path) => new PEAnalyzer(path);

        private void Analyze()
        {
            using (var fs = File.OpenRead(FilePath))
            using (BinaryReader = new BinaryReader(fs, Encoding.ASCII))
            {
                ReadDosHeader();
                ReadNTHeader();
                StaticConvrter.SectionHeaders = SectionHeaders;
                ReadExportSymbols();
                ReadImportSymbols();
            }
        }

        private void ReadDosHeader()
        {
            DosHeader = BinaryReader.ReadStructre<ImageDosHeader>();
            DosStub = BinaryReader.ReadBytes((int)(DosHeader.AddressOfNewExeHeader - BinaryReader.BaseStream.Position));
        }

        private void ReadNTHeader()
        {
            var TempNTHeader = new ImageNTHeaders
            {
                Signature = BinaryReader.ReadUInt32(),
                FileHeader = BinaryReader.ReadStructre<ImageFileHeader>()
            };

            var OpthinalHeaderMagic = (OptionalMagic)BinaryReader.ReadUInt16();
            BinaryReader.BaseStream.Seek(-sizeof(UInt16), SeekOrigin.Current);
            switch (OpthinalHeaderMagic)
            {
                case OptionalMagic.PE32:
                    TempNTHeader.OptionalHeader = BinaryReader.ReadStructre<ImageOptionalHeader32>();
                    break;

                case OptionalMagic.PE64:
                    TempNTHeader.OptionalHeader = BinaryReader.ReadStructre<ImageOptionalHeader64>();
                    break;

                case OptionalMagic.ROM:
                default:
                    break;
            }

            NTHeader = TempNTHeader;

            SectionHeaders = new ImageSectionHeader[NTHeader.FileHeader.NumberOfSections];
            for (int i = 0; i < SectionHeaders.Length; i++)
            {
                SectionHeaders[i] = BinaryReader.ReadStructre<ImageSectionHeader>();
            }

        }

        private void ReadExportSymbols()
        {
            if (NTHeader.OptionalHeader.DataDirectoryArray.Export.VirtualAddress == 0
                || NTHeader.OptionalHeader.DataDirectoryArray.Export.Size == 0)
            {
                return;
            }

            BinaryReader.BaseStream.Seek(NTHeader.OptionalHeader.DataDirectoryArray.Export.VirtualAddress.ToFOA(), SeekOrigin.Begin);
            ExportDirectory = BinaryReader.ReadStructre<ImageExportDirectory>();

            var FuncArrayFOA = ExportDirectory.AddressOfFunctions.ToFOA();
            BinaryReader.BaseStream.Seek(ExportDirectory.Name.ToFOA(), SeekOrigin.Begin);
            Name = BinaryReader.ReadStdString();

            var ExportSymbolList = new List<ExportSymbol>();
            var NameFOAList = new List<UInt32>();
            var OrdinalList = new List<UInt16>();
            var NameList = new List<string>();

            BinaryReader.BaseStream.Seek(ExportDirectory.AddressOfNames.ToFOA(), SeekOrigin.Begin);
            for (int i = 0; i < ExportDirectory.NumberOfNames; i++)
            {
                NameFOAList.Add(BinaryReader.ReadUInt32().ToFOA());
            }

            BinaryReader.BaseStream.Seek(ExportDirectory.AddressOfNameOrdinals.ToFOA(), SeekOrigin.Begin);
            for (int i = 0; i < ExportDirectory.NumberOfNames; i++)
            {
                OrdinalList.Add(BinaryReader.ReadUInt16());
            }

            foreach (var foa in NameFOAList)
            {
                BinaryReader.BaseStream.Seek(foa, SeekOrigin.Begin);
                NameList.Add(BinaryReader.ReadStdString());
            }

            var NameEnumerator = NameList.GetEnumerator();
            var OrdiEnumerator = OrdinalList.GetEnumerator();
            while (NameEnumerator.MoveNext() && OrdiEnumerator.MoveNext())
            {
                BinaryReader.BaseStream.Seek(FuncArrayFOA + OrdiEnumerator.Current * sizeof(UInt32), SeekOrigin.Begin);
                if (!string.IsNullOrWhiteSpace(NameEnumerator.Current))
                {
                    var FuncRVA = BinaryReader.ReadUInt32();
                    var JmpName = "";
                    if (FuncRVA >= NTHeader.OptionalHeader.DataDirectoryArray.Export.VirtualAddress
                        && FuncRVA < NTHeader.OptionalHeader.DataDirectoryArray.Export.VirtualAddress
                        + NTHeader.OptionalHeader.DataDirectoryArray.Export.Size)
                    {
                        BinaryReader.BaseStream.Seek(FuncRVA.ToFOA(), SeekOrigin.Begin);
                        JmpName = BinaryReader.ReadStdString();
                    }
                    ExportSymbolList.Add(new ExportSymbol { JumpName = JmpName, Name = NameEnumerator.Current, VirtualAddress = FuncRVA });
                }
            }

            ExportSymbols = ExportSymbolList.ToArray();
            ExportNames = ExportSymbolList.Select(q => q.Name).ToArray();
        }

        private void ReadImportSymbols()
        {
            if (NTHeader.OptionalHeader.DataDirectoryArray.Import.VirtualAddress == 0
                || NTHeader.OptionalHeader.DataDirectoryArray.Import.Size == 0)
            {
                return;
            }

            BinaryReader.BaseStream.Seek(NTHeader.OptionalHeader.DataDirectoryArray.Import.VirtualAddress.ToFOA(), SeekOrigin.Begin);
            var ImportList = new List<ImageImportDescriptor>();
            while (true)
            {
                var ImportDescriptor = BinaryReader.ReadStructre<ImageImportDescriptor>();
                if (ImportDescriptor.DummyUnionName.OriginalFirstThunk == 0)
                {
                    break;
                }
                ImportList.Add(ImportDescriptor);
            }

            var ImportSymbolsList = new List<ImportSymbol>();
            var ImportNameList = new List<string>();
            foreach (var Import in ImportList)
            {
                BinaryReader.BaseStream.Seek(Import.Name.ToFOA(), SeekOrigin.Begin);
                var ImportName = BinaryReader.ReadStdString();
                var ImportSymbol = new ImportSymbol { ImportDescriptor = Import, Name = ImportName };
                if (NTHeader.OptionalHeader.Magic != OptionalMagic.PE64)
                {
                    var ThrunkList = new List<UInt32>();
                    BinaryReader.BaseStream.Seek(Import.DummyUnionName.OriginalFirstThunk.ToFOA(), SeekOrigin.Begin);
                    while (true)
                    {
                        var Thrunk = BinaryReader.ReadUInt32();
                        if (Thrunk == 0)
                        {
                            break;
                        }
                        ThrunkList.Add(Thrunk);
                    }
                    var ImportByNameList = new List<ImageImportByName>();
                    foreach (var Thrunk in ThrunkList)
                    {
                        BinaryReader.BaseStream.Seek(Thrunk.ToFOA(), SeekOrigin.Begin);
                        var ImportByName = new ImageImportByName
                        {
                            Hint = BinaryReader.ReadUInt32(),
                            Name = BinaryReader.ReadStdString()
                        };
                        if (!string.IsNullOrWhiteSpace(ImportByName.Name))
                        {
                            ImportByNameList.Add(ImportByName);
                        }
                    }
                    ImportSymbol.ImportByName = ImportByNameList.ToArray();
                }
                ImportNameList.Add(ImportName);
                ImportSymbolsList.Add(ImportSymbol);
            }

            ImportSymbols = ImportSymbolsList.ToArray();
            ImportNames = ImportNameList.ToArray();
        }
    }
}
