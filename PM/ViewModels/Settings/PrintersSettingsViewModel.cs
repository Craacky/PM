using PM.DAL.Entities;
using PM.ViewModels.Base;

namespace PM.ViewModels
{
    public class PrintersSettingsViewModel : ViewModel
    {
        public Settings CurrentSettings { get; set; }

        public PrintersSettingsViewModel(Settings currentSettings)
        {
            CurrentSettings = currentSettings;
        }
    }
}
