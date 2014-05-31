using System;
using System.Collections.Generic;

namespace SendStuffToPrinter.Helpers
{
    public interface IPrinterService
    {
        void SendBytesToPrinter(string printerName, IntPtr pBytes, int dwCount);
        void SendStringToPrinter(string printerName, string printerText);
        IEnumerable<string> ListInstalledPrinters();
    }
}
