using System;

namespace SendStuffToPrinter.Helpers
{
    public interface ISystemService
    {
        string ReadFile(string fileName);
    }
}
