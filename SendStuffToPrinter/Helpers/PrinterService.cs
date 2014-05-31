using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;

namespace SendStuffToPrinter.Helpers
{
    public class PrinterService : IPrinterService
    {
        private static class Win32NativeMethods
        {
            [DllImport("winspool.drv", EntryPoint = "OpenPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", EntryPoint = "StartDocPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [MarshalAs(UnmanagedType.LPStruct), In] DOCINFOA di);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
        }

        [StructLayout(LayoutKind.Sequential)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        public IEnumerable<string> ListInstalledPrinters()
        {
            using (var printers = new ManagementObjectSearcher("Select Name from Win32_Printer"))
            {
                foreach (var printer in printers.Get())
                    yield return printer.GetPropertyValue("Name") as string;
            }
        }

        public void SendStringToPrinter(string printerName, string printerText)
        {
            IntPtr num = IntPtr.Zero;
            try
            {
                num = Marshal.StringToCoTaskMemAnsi(printerText);
                int length = printerText.Length;
                this.SendBytesToPrinter(printerName, num, length);
            }
            finally
            {
                if (num != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(num);
            }
        }

        public void SendBytesToPrinter(string printerName, IntPtr pBytes, int dwCount)
        {
            int dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA { pDocName = "Raw Text Document", pDataType = "RAW" };
            bool flag = false;

            if (Win32NativeMethods.OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (Win32NativeMethods.StartDocPrinter(hPrinter, 1, di))
                {
                    if (Win32NativeMethods.StartPagePrinter(hPrinter))
                    {
                        flag = Win32NativeMethods.WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        Win32NativeMethods.EndPagePrinter(hPrinter);
                    }

                    Win32NativeMethods.EndDocPrinter(hPrinter);
                }

                Win32NativeMethods.ClosePrinter(hPrinter);
            }

            if (!flag)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
