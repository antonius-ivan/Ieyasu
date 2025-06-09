using System.Data;

namespace AIRMDataManager.Library.Common.DataAccess
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
