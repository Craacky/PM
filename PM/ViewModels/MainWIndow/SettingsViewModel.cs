using PM.Commands;
using PM.DAL.Entities;
using PM.Navigators;
using PM.Services;
using PM.ViewModels.Base;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private Settings currentSettings;
        public Settings CurrentSettings 
        { 
            get => currentSettings;
            set
            {
                currentSettings = value;
                OnPropertyChanged(nameof(CurrentSettings));
            }
        }


        public LineSettingsViewModel LineSettingsViewModel { get; set; }
        public CamerasSettingsViewModel CamerasSettingsViewModel { get; set; }
        public PrintersSettingsViewModel PrintersSettingsViewModel { get; set; }
        public DataBasesSettingsViewModel DataBasesSettingsViewModel { get; set; }


        private ICommand updateSettingsCommand;
        public ICommand UpdateSettingsCommand => updateSettingsCommand;
        private bool CanUpdateSettingsCommandExecute(object p)
        {
            return ReportTaskService.CurrentReportTask == null;
        }
        private void OnUpdateSettingsCommandExecuted(object p)
        {
            SettingsService.SaveSettings((Settings)CurrentSettings.Clone());
            DeviceService.CreateDevice();
        }


        public SettingsNavigator Navigator { get; set; }
        public ISettingsService SettingsService { get; set; }
        public IDeviceService DeviceService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }

        public SettingsViewModel(ISettingsService settingsService,
                                 IDeviceService deviceService,
                                 ReportTaskService reportTaskService)
        {
            SettingsService = settingsService;
            DeviceService = deviceService;
            ReportTaskService = reportTaskService;

            CurrentSettings = (Settings)SettingsService.Settings.Clone();


            LineSettingsViewModel = new LineSettingsViewModel(CurrentSettings);
            CamerasSettingsViewModel = new CamerasSettingsViewModel(CurrentSettings);
            PrintersSettingsViewModel = new PrintersSettingsViewModel(CurrentSettings);
            DataBasesSettingsViewModel = new DataBasesSettingsViewModel(CurrentSettings);

            Navigator = new SettingsNavigator(LineSettingsViewModel,
                                              CamerasSettingsViewModel,
                                              PrintersSettingsViewModel,
                                              DataBasesSettingsViewModel);

            updateSettingsCommand = new RelayCommand(OnUpdateSettingsCommandExecuted, CanUpdateSettingsCommandExecute);

            Navigator.UpdateCurrentViewModelCommand.Execute(SettingsViewType.LineView);
        }
    }
}
