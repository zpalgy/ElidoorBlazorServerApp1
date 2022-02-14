using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace BlazorServerApp1.Data
{
    public static class Helper
    {
        public static bool[] hideFld = new bool[200];

        public static bool setHideFld (int i, bool val)
        {
            hideFld[i] = val;
            return true;
        }


        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        #region Gets the build date and time (by reading the COFF header)

        // http://msdn.microsoft.com/en-us/library/ms680313

        struct _IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        };

        public static DateTime GetBuildDateTime(Assembly assembly)
        {
            var path = assembly.GetName().CodeBase;
            if (File.Exists(path))
            {
                var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = 0x3C;
                    fileStream.Read(buffer, 0, 4);
                    fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                    fileStream.Read(buffer, 0, 4); // "PE\0\0"
                    fileStream.Read(buffer, 0, buffer.Length);
                }
                var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));

                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                }
                finally
                {
                    pinnedBuffer.Free();
                }
            }
            return new DateTime();
        }
        public static DateTime getBuildTime ()
        {
            try
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                string codebase = currentAssembly.CodeBase;
                int pathStart = codebase.ToLower().IndexOf(@"c:/");
                string path = codebase.Substring(pathStart);
                DateTime lastWriteTime = File.GetLastWriteTime(path);
                return lastWriteTime;
            }
            catch (Exception ex)
            {
                string errMsg2 = string.Format("שגיאה : אנא פנה למנהל המערכת : {0} , {1}    י", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg2);
                return DateTime.Now.AddYears(-10);
            }
        }

        #endregion
        public static string DecorFormat2Code (string decorFormat)
        {
            switch (decorFormat)
            {
                case "חוץ":
                    return "O";
                case "פנים":
                    return "I";
                case "דו צדדי":
                    return "B";
                default:
                    return "Error";
            }
        }

    }
}