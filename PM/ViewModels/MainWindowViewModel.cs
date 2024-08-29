using PM.Commands;
using PM.Navigators;
using PM.Services;
using PM.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private ICommand closeWindowCommand;
        public ICommand CloseWindowCommand => closeWindowCommand;
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }


        public  MainWindowNavigator Navigator { get; set; }
        public ISettingsService SettingsService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public MainWindowViewModel(MainWindowNavigator navigator,
                                   ISettingsService settingsService,
                                   ErrorsService errorsService)
        {
            Navigator = navigator;
            SettingsService = settingsService;
            ErrorsService = errorsService;

            closeWindowCommand = new RelayCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);

            Navigator.UpdateCurrentViewModelCommand.Execute(MainWindowViewType.ReportTasksView);
        }
    }

    public class PrinterViewModel : ViewModel
    {
    }

    public class EventsViewModel : ViewModel
    {
    }

}
