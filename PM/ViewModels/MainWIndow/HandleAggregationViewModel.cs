using PM.Commands;
using PM.Devices;
using PM.Navigators;
using PM.Services;
using PM.ViewModels.Base;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class HandleAggregationViewModel : ViewModel
    {
        public HandleAggregationAddProductViewModel HandleAggregationAddProductViewModel { get; set; }
        public HandleAggregationDeleteProductViewModel HandleAggregationDeleteProductViewModel { get; set; }
        public HandleAggregationAddBoxViewModel HandleAggregationAddBoxViewModel { get; set; }
        public HandleAggregationDeleteBoxViewModel HandleAggregationDeleteBoxViewModel { get; set; }
        public HandleAggregationAddPalletViewModel HandleAggregationAddPalletViewModel { get; set; }
        public HandleAggregationDeletePalletViewModel HandleAggregationDeletePalletViewModel { get; set; }
        public HandleAggregationCheckCodeViewModel HandleAggregationCheckCodeViewModel { get; set; }

        public HandleAggregationNavigator Navigator { get; set; }


        public ReportTaskService ReportTaskService { get; set; }
        public ProcessingCodeService ProcessingCodeService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public HandleAggregationViewModel(ReportTaskService reportTaskService,
                                          ProcessingCodeService processingCodeService,
                                          LocalDBService localDBService,
                                          ErrorsService errorsService)
        {
            ReportTaskService = reportTaskService;
            ProcessingCodeService = processingCodeService;
            LocalDBService = localDBService;
            ErrorsService = errorsService;

            HandleAggregationAddProductViewModel = new HandleAggregationAddProductViewModel(ProcessingCodeService,
                                                                                        ReportTaskService,
                                                                                        LocalDBService, 
                                                                                        ErrorsService);
            HandleAggregationDeleteProductViewModel = new HandleAggregationDeleteProductViewModel(ProcessingCodeService,
                                                                                                  ReportTaskService,
                                                                                                  LocalDBService,
                                                                                               
                                                                                                  ErrorsService);
            HandleAggregationAddBoxViewModel = new HandleAggregationAddBoxViewModel(ProcessingCodeService,
                                                                                    ReportTaskService,
                                                                                    LocalDBService,
                                                                                    ErrorsService);
            HandleAggregationDeleteBoxViewModel = new HandleAggregationDeleteBoxViewModel(ProcessingCodeService,
                                                                                          ReportTaskService,
                                                                                          LocalDBService,
                                                                                          ErrorsService);
            HandleAggregationAddPalletViewModel = new HandleAggregationAddPalletViewModel(ProcessingCodeService,
                                                                                          ReportTaskService,
                                                                                          LocalDBService,
                                                                                          ErrorsService);
            HandleAggregationDeletePalletViewModel = new HandleAggregationDeletePalletViewModel(ProcessingCodeService,
                                                                                          ReportTaskService,
                                                                                          LocalDBService,
                                                                                          ErrorsService);

            HandleAggregationCheckCodeViewModel = new HandleAggregationCheckCodeViewModel(ProcessingCodeService,
                                                                                          ReportTaskService,
                                                                                          LocalDBService);

            Navigator = new HandleAggregationNavigator(HandleAggregationAddProductViewModel,
                                                       HandleAggregationDeleteProductViewModel,
                                                       HandleAggregationAddBoxViewModel,
                                                       HandleAggregationDeleteBoxViewModel,
                                                       HandleAggregationAddPalletViewModel,
                                                       HandleAggregationDeletePalletViewModel,
                                                       HandleAggregationCheckCodeViewModel);


            Navigator.UpdateCurrentViewModelCommand.Execute(HandleAggregationViewType.CheckCodeView);
        }

    }
}
