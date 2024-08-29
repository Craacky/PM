using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class NomenclatureDataService : DataService<DAL.Entities.Nomenclature>
    {
        public NomenclatureDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
