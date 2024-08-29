using PM.DAL.Entities;
using PM.ViewModels.Base;

namespace PM.ViewModels
{
    public class DataBasesSettingsViewModel : ViewModel
    {
        public Settings CurrentSettings { get; set; }

        public DataBasesSettingsViewModel(Settings currentSettings)
        {
            CurrentSettings = currentSettings;
        }
    }
}
