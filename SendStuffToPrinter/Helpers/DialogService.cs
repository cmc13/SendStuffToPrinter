using Microsoft.Win32;
using System;

namespace SendStuffToPrinter.Helpers
{
    public class DialogService : IDialogService
    {
        public bool? ShowOpenFileDialog(out string fileName)
        {
            var dlg = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true
            };

            fileName = null;

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
                fileName = dlg.FileName;

            return result;
        }
    }
}
