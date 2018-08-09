using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PEStructure
{
    public interface IImageStructure
    {
        int MarshalSize { get; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TimeDateStamp : IImageStructure
    {
        public UInt32 RawData;

        public DateTime DateTime => BaseDateTime.AddSeconds(RawData);
        private static readonly DateTime BaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        public static int StructureSize = Marshal.SizeOf(typeof(TimeDateStamp));
        public int MarshalSize => StructureSize;
    }

    static class StaticConvrter
    {
        public static IEnumerable<ImageSectionHeader> SectionHeaders { get; set; }

        public static UInt32 ToFOA(this UInt32 RVA) =>
            SectionHeaders.Where(q => q.VirtualAddress <= RVA && q.VirtualAddress + q.SizeOfRawData > RVA)
                .Select(q => q.PointerToRawData - q.VirtualAddress).First() + RVA;

        public static string GetString(this byte[] bytes) => Encoding.ASCII.GetString(bytes);

        public static byte[] ToBytes(this object obj)
        {
            var size = Marshal.SizeOf(obj);
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, buffer, false);
                var bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            catch
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static T ToStructure<T>(this byte[] bytes) where T : struct
        {
            var buffer = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                Marshal.Copy(bytes, 0, buffer, bytes.Length);
                var obj = (T)Marshal.PtrToStructure(buffer, typeof(T));
                return obj;
            }
            catch
            {
                return default(T);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static T ReadStructre<T>(this BinaryReader binaryReader) where T : struct => binaryReader.ReadBytes(Marshal.SizeOf(typeof(T))).ToStructure<T>();

        public static string ReadStdString(this BinaryReader binaryReader)
        {
            char ch;
            var sb = new StringBuilder();
            while ((ch = binaryReader.ReadChar()) != 0)
            {
                sb.Append(ch);
            }
            return sb.ToString();
        }
    }

}
