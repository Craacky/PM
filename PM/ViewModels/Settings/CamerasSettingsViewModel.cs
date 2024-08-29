using PM.DAL.Entities;
using PM.ViewModels.Base;

namespace PM.ViewModels
{
    public class CamerasSettingsViewModel : ViewModel
    {
        public Settings CurrentSettings { get; set; }


        public CamerasSettingsViewModel(Settings currentSettings)
        {
            CurrentSettings = currentSettings;
        }
    }
}
