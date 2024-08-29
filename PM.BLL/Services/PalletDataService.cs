using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class PalletDataService : DataService<DAL.Entities.Pallet>
    {
        public PalletDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
