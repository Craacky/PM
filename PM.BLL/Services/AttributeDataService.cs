using PM.BLL.Services.Base;

namespace PM.BLL.Services
{
    public class AttributeDataService : DataService<DAL.Entities.Attribute>
    {
        public AttributeDataService(string connectionString) : base(connectionString)
        {
        }
    }
}
