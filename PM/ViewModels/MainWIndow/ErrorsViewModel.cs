using PM.Services;
using PM.ViewModels.Base;

namespace PM.ViewModels
{
    public class ErrorsViewModel : ViewModel
    {
        public ErrorsService ErrorsService { get; set; }

        public ErrorsViewModel(ErrorsService errorsService)
        {
            ErrorsService = errorsService;
        }
    }
}
