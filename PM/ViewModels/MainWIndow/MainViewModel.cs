using PM.Commands;
using PM.Services;
using PM.ViewModels.Base;
using System.Windows.Forms;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private ICommand closePalletCommand;
        public ICommand ClosePalletCommand => closePalletCommand;
        private bool CanClosePalletCommandExecute(object p)
        {
            bool isClosedPallet = ReportTaskService.Statistic.PalletCodes.Count > 0 && ReportTaskService.Statistic.PalletCodes[^1].IsFulled;
            return !isClosedPallet && (ReportTaskService.Statistic.PalletCodes.Count > 0 && ReportTaskService.Statistic.CountBoxInCurrentPallet > 0) && ReportTaskService.CurrentReportTask != null;
        }
        private void OnClosePalletCommandlCommandExecuted(object p)
        {
            DeviceService.ReportTaskService.ClosePallet();
            MessageBox.Show("Паллета закрыта");
            DeviceService.PalletPrinterService?.PrintCode();
        }

        private ICommand addBoxesToPreviousPalletCommand;
        public ICommand AddBoxesToPreviousPalletCommand => addBoxesToPreviousPalletCommand;
        private bool CanAddBoxesToPreviousPalletCommandExecute(object p)
        {
            return ReportTaskService.Statistic.PalletCodes.Count > 1 && ReportTaskService.CurrentReportTask != null;
        }
        private void OnAddBoxesToPreviousPalletCommandExecuted(object p)
        {
            DeviceService.ReportTaskService.AddBoxesToPreviousPallet();
            DeviceService.PalletPrinterService?.PrintCode();
        }


        public ReportTaskService ReportTaskService { get; set; }
        public IDeviceService DeviceService { get; set; }
        public ISettingsService ISettingsService { get; set; }
        public ErrorsService ErrorsService { get; set; }

        public MainViewModel(ReportTaskService reportTaskService,
                             ISettingsService settingsService,
                             IDeviceService deviceService)
        {
            ReportTaskService = reportTaskService;
            ISettingsService = settingsService;
            DeviceService = deviceService;

            closePalletCommand = new RelayCommand(OnClosePalletCommandlCommandExecuted, CanClosePalletCommandExecute);
            addBoxesToPreviousPalletCommand = new RelayCommand(OnAddBoxesToPreviousPalletCommandExecuted, CanAddBoxesToPreviousPalletCommandExecute);
        }
    }
}
