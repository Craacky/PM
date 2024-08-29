using PM.Commands;
using PM.Models.Base;
using PM.ViewModels;
using PM.ViewModels.Base;
using System.Windows.Input;

namespace PM.Navigators
{
    public enum MainWindowViewType
    {
       MainView,
       ReportTasksView,
       HandleAggregationView,
       EventsView,
       PrinterView,
       TaskHistoryView,
       ErrorsView,
       SettingsView
    }

    public class MainWindowNavigator : ObservableObject
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }


        public MainViewModel MainViewModel { get; set; }
        public ReportTasksViewModel ReportTasksViewModel { get; set; }
        public HandleAggregationViewModel HandleAggregationViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public PrinterViewModel PrinterViewModel { get; set; }
        public TaskHistoryViewModel NomenclaturesViewModel { get; set; }
        public ErrorsViewModel ErrorsViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }


        private ICommand updateCurrentViewModelCommand;
        public ICommand UpdateCurrentViewModelCommand => updateCurrentViewModelCommand;
        private bool CanUpdateCurrentViewModelCommandExecute(object parameter) => true;
        private void OnUpdateCurrentViewModelCommandExecuted(object parameter)
        {
            if (parameter is MainWindowViewType viewType)
            {
                switch (viewType)
                {
                    case MainWindowViewType.MainView:
                        CurrentViewModel = MainViewModel;
                        break;
                    case MainWindowViewType.ReportTasksView:
                        CurrentViewModel = ReportTasksViewModel;
                        break;
                    case MainWindowViewType.HandleAggregationView:
                        CurrentViewModel = HandleAggregationViewModel;
                        break;
                    case MainWindowViewType.EventsView:
                        CurrentViewModel = EventsViewModel;
                        break;
                    case MainWindowViewType.PrinterView:
                        CurrentViewModel = PrinterViewModel;
                        break;
                    case MainWindowViewType.TaskHistoryView:
                        CurrentViewModel = NomenclaturesViewModel;
                        break;
                    case MainWindowViewType.ErrorsView:
                        CurrentViewModel = ErrorsViewModel;
                        break;
                    case MainWindowViewType.SettingsView:
                        CurrentViewModel = new SettingsViewModel(SettingsViewModel.SettingsService, SettingsViewModel.DeviceService, SettingsViewModel.ReportTaskService);
                        break;
                    default:
                        break;
                }
            }
        }


        public MainWindowNavigator(MainViewModel mainViewModel, 
                                   ReportTasksViewModel reportTasksViewModel, 
                                   HandleAggregationViewModel handleAggregationViewModel, 
                                   EventsViewModel eventsViewModel, 
                                   PrinterViewModel printerViewModel, 
                                   TaskHistoryViewModel nomenclaturesViewModel,
                                   ErrorsViewModel errorsViewModel,
                                   SettingsViewModel settingsViewModel)
        {
            MainViewModel = mainViewModel;
            ReportTasksViewModel = reportTasksViewModel;
            HandleAggregationViewModel = handleAggregationViewModel;
            EventsViewModel = eventsViewModel;
            PrinterViewModel = printerViewModel;
            NomenclaturesViewModel = nomenclaturesViewModel;
            ErrorsViewModel = errorsViewModel;
            SettingsViewModel = settingsViewModel;

            updateCurrentViewModelCommand = new RelayCommand(OnUpdateCurrentViewModelCommandExecuted, CanUpdateCurrentViewModelCommandExecute);
        }
    }
}
