
using PM.Commands;
using PM.Models.Base;
using PM.ViewModels;
using PM.ViewModels.Base;
using System.Windows.Input;

namespace PM.Navigators
{
    public enum SettingsViewType
    {
        LineView,
        CamerasView,
        PrintersView,
        DataBasesView
    }

    public class SettingsNavigator : ObservableObject
    {
        private ViewModel currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }


        public LineSettingsViewModel LineSettingsViewModel { get; set; }
        public CamerasSettingsViewModel CamerasSettingsViewModel { get; set; }
        public PrintersSettingsViewModel PrintersSettingsViewModel { get; set; }
        public DataBasesSettingsViewModel DataBasesSettingsViewModel { get; set; }


        private ICommand updateCurrentViewModelCommand;
        public ICommand UpdateCurrentViewModelCommand => updateCurrentViewModelCommand;
        private bool CanUpdateCurrentViewModelCommandExecute(object p) => true;
        private void OnUpdateCurrentViewModelCommandExecuted(object p)
        {
            if (p is SettingsViewType viewType)
            {
                switch (viewType)
                {
                    case SettingsViewType.LineView:
                        CurrentViewModel = LineSettingsViewModel;
                        break;
                    case SettingsViewType.CamerasView:
                        CurrentViewModel = CamerasSettingsViewModel;
                        break;
                    case SettingsViewType.PrintersView:
                        CurrentViewModel = PrintersSettingsViewModel;
                        break;
                    case SettingsViewType.DataBasesView:
                        CurrentViewModel = DataBasesSettingsViewModel;
                        break;
                    default:
                        break;
                }
            }
        }


        public SettingsNavigator(LineSettingsViewModel lineSettingsViewModel, 
                                 CamerasSettingsViewModel camerasSettingsViewModel, 
                                 PrintersSettingsViewModel printersSettingsViewModel, 
                                 DataBasesSettingsViewModel dataBasesSettingsViewModel)
        {
            LineSettingsViewModel = lineSettingsViewModel;
            CamerasSettingsViewModel = camerasSettingsViewModel;
            PrintersSettingsViewModel = printersSettingsViewModel;
            DataBasesSettingsViewModel = dataBasesSettingsViewModel;

            updateCurrentViewModelCommand = new RelayCommand(OnUpdateCurrentViewModelCommandExecuted, CanUpdateCurrentViewModelCommandExecute);
        }
    }
}
