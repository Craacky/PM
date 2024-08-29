using PM.DAL.Entities;
using PM.Devices;

namespace PM.Services
{
    public interface ISettingsService
    {
        LocalDBService LocalDBService { get; set; }
        Settings Settings { get; set; }

        void LoadSettings();
        void SaveSettings(Settings settings);
    }
}