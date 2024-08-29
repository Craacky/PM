
using PM.Commands;
using PM.Models.Base;
using PM.ViewModels;
using PM.ViewModels.Base;
using System.Windows.Input;

namespace PM.Navigators
{
    public enum HandleAggregationViewType
    {
        CheckCodeView,
        AddProductView,
        AddBoxView,
        AddPalletView,
        DeleteProductView,
        DeleteBoxView,
        DeletePalletView
    }
    public class HandleAggregationNavigator : ObservableObject
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


        public HandleAggregationAddProductViewModel HandleAggregationAddProductViewModel { get; set; }
        public HandleAggregationDeleteProductViewModel HandleAggregationDeleteProductViewModel { get; set; }
        public HandleAggregationAddBoxViewModel HandleAggregationAddBoxViewModel { get; set; }
        public HandleAggregationDeleteBoxViewModel HandleAggregationDeleteBoxViewModel { get; set; }
        public HandleAggregationAddPalletViewModel HandleAggregationAddPalletViewModel { get; set; }
        public HandleAggregationDeletePalletViewModel HandleAggregationDeletePalletViewModel { get; set; }
        public HandleAggregationCheckCodeViewModel HandleAggregationCheckCodeViewModel { get; set; }


        private ICommand updateCurrentViewModelCommand;
        public ICommand UpdateCurrentViewModelCommand => updateCurrentViewModelCommand;
        private bool CanUpdateCurrentViewModelCommandExecute(object p) => true;
        private void OnUpdateCurrentViewModelCommandExecuted(object p)
        {
            if (p is HandleAggregationViewType viewType)
            {
                switch (viewType)
                {
                    case HandleAggregationViewType.CheckCodeView:
                        CurrentViewModel = HandleAggregationCheckCodeViewModel;
                        break;
                    case HandleAggregationViewType.AddProductView:
                        CurrentViewModel = HandleAggregationAddProductViewModel;
                        break;
                    case HandleAggregationViewType.DeleteProductView:
                        CurrentViewModel = HandleAggregationDeleteProductViewModel;
                        break;
                    case HandleAggregationViewType.AddBoxView:
                        CurrentViewModel = HandleAggregationAddBoxViewModel;
                        break;
                    case HandleAggregationViewType.DeleteBoxView:
                        CurrentViewModel = HandleAggregationDeleteBoxViewModel;
                        break;
                    case HandleAggregationViewType.AddPalletView:
                        CurrentViewModel = HandleAggregationAddPalletViewModel;
                        break;
                    case HandleAggregationViewType.DeletePalletView:
                        CurrentViewModel = HandleAggregationDeletePalletViewModel;
                        break;
                    default:
                        break;
                }
            }
        }


        public HandleAggregationNavigator(HandleAggregationAddProductViewModel handleAggregationAddProductViewModel, 
                                          HandleAggregationDeleteProductViewModel handleAggregationDeleteProductViewModel, 
                                          HandleAggregationAddBoxViewModel handleAggregationAddBoxViewModel, 
                                          HandleAggregationDeleteBoxViewModel handleAggregationDeleteBoxViewModel, 
                                          HandleAggregationAddPalletViewModel handleAggregationAddPalletViewModel, 
                                          HandleAggregationDeletePalletViewModel handleAggregationDeletePalletViewModel, 
                                          HandleAggregationCheckCodeViewModel handleAggregationCheckCodeViewModel)
        {
            HandleAggregationAddProductViewModel = handleAggregationAddProductViewModel;
            HandleAggregationDeleteProductViewModel = handleAggregationDeleteProductViewModel;
            HandleAggregationAddBoxViewModel = handleAggregationAddBoxViewModel;
            HandleAggregationDeleteBoxViewModel = handleAggregationDeleteBoxViewModel;
            HandleAggregationAddPalletViewModel = handleAggregationAddPalletViewModel;
            HandleAggregationDeletePalletViewModel = handleAggregationDeletePalletViewModel;
            HandleAggregationCheckCodeViewModel = handleAggregationCheckCodeViewModel;

            updateCurrentViewModelCommand = new RelayCommand(OnUpdateCurrentViewModelCommandExecuted, CanUpdateCurrentViewModelCommandExecute);
        }       
    }
}
