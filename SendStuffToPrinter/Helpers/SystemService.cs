using System.IO;

namespace SendStuffToPrinter.Helpers
{
    public class SystemService : ISystemService
    {
        public string ReadFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
