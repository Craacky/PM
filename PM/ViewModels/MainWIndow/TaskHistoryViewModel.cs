using PM.Services;
using PM.ViewModels.Base;

namespace PM.ViewModels
{
    public class TaskHistoryViewModel : ViewModel
    {
        public ReportTaskService ReportTaskService { get; set; }

        public TaskHistoryViewModel(ReportTaskService reportTaskService)
        {
            ReportTaskService = reportTaskService;
        }
    }

}
