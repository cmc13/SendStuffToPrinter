using System;
namespace SendStuffToPrinter.Helpers
{
    public interface IDialogService
    {
        bool? ShowOpenFileDialog(out string fileName);
    }
}
