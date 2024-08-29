using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class ReportTaskDataService : DataService<DAL.Entities.ReportTask>
    {
        public ReportTaskDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
