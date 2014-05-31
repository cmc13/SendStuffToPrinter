using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SendStuffToPrinter.Helpers;
using System.Collections.ObjectModel;

namespace SendStuffToPrinter.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Data Members

        private readonly IPrinterService printerService;
        private ObservableCollection<string> installedPrinters = new ObservableCollection<string>();
        private string printerText;
        private string selectedPrinter;
        private readonly IDialogService dialogService;
        private readonly ISystemService systemService;
        private int line;
        private int column;

        #endregion

        #region Public Constructor Definitions

        public MainViewModel() : this(new PrinterService(), new DialogService(), new SystemService())
        { }

        public MainViewModel(IPrinterService printerService, IDialogService dialogService, ISystemService systemService)
        {
            this.printerService = printerService;
            this.dialogService = dialogService;
            this.systemService = systemService;

            this.SendCommand = new RelayCommand(() => this.printerService.SendStringToPrinter(this.SelectedPrinter, this.PrinterText),
                () => !string.IsNullOrEmpty(this.PrinterText) && !string.IsNullOrEmpty(this.SelectedPrinter));
            this.OpenFileCommand = new RelayCommand(() =>
            {
                string fileName;
                var result = this.dialogService.ShowOpenFileDialog(out fileName);
                if (result.HasValue && result.Value)
                    this.PrinterText = this.systemService.ReadFile(fileName);
            });

            foreach (var printer in this.printerService.ListInstalledPrinters())
                this.installedPrinters.Add(printer);
        }

        #endregion

        #region Command Declarations

        public RelayCommand SendCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }

        #endregion

        #region Public Property Definitions

        public ObservableCollection<string> InstalledPrinters
        {
            get { return this.installedPrinters; }
            set
            {
                if (this.installedPrinters != value)
                {
                    this.installedPrinters = value;
                    base.RaisePropertyChanged(() => this.InstalledPrinters);
                }
            }
        }

        public string SelectedPrinter
        {
            get { return this.selectedPrinter; }
            set
            {
                if (this.selectedPrinter != value)
                {
                    this.selectedPrinter = value;
                    base.RaisePropertyChanged(() => this.SelectedPrinter);
                    this.SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string PrinterText
        {
            get { return this.printerText; }
            set
            {
                if (this.printerText != value)
                {
                    this.printerText = value;
                    base.RaisePropertyChanged(() => this.PrinterText);
                    this.SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int Line
        {
            get { return this.line; }
            set
            {
                if (this.line != value)
                {
                    this.line = value;
                    base.RaisePropertyChanged(() => this.Line);
                }
            }
        }

        public int Column
        {
            get { return this.column; }
            set
            {
                if (this.column != value)
                {
                    this.column = value;
                    base.RaisePropertyChanged(() => this.Column);
                }
            }
        }

        #endregion
    }
}
