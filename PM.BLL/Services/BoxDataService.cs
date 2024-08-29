using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class BoxDataService : DataService<DAL.Entities.Box>
    {
        public BoxDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
