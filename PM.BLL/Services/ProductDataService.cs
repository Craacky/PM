using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class ProductDataService : DataService<DAL.Entities.Product>
    {
        public ProductDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
