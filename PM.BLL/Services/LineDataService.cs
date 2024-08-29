using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class LineDataService : DataService<DAL.Entities.Line>
    {
        public LineDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
