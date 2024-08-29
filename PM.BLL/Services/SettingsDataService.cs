using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class SettingsDataService : DataService<DAL.Entities.Settings>
    {
        public SettingsDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
