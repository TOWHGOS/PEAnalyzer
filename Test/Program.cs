using System;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ThisFile = Process.GetCurrentProcess().MainModule.FileName;
            var PE = PEStructure.PEAnalyzer.Analyze(ThisFile);

            if (PE.HasImport)
            {
                Console.WriteLine("Import Symbols:");
                foreach (var Import in PE.ImportSymbols)
                {
                    Console.WriteLine(Import.Name);
                }
                Console.WriteLine();
            }

            if (PE.HasExport)
            {
                Console.WriteLine("Export Symbols:");
                foreach (var Export in PE.ExportSymbols)
                {
                    Console.WriteLine($"{Export.Name}:{Export.VirtualAddress}");
                }
                Console.WriteLine();
            }

            Console.ReadKey(true);
        }
    }
}
