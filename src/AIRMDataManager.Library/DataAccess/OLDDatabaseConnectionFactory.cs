using AIRMDataManager.Library.Common.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AIRMDataManager.Library.DataAccess
{
    public class OLDDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OLDDatabaseConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            // Ensure your connection string name matches your appsettings.json
        }

        /// <summary>
        /// Creates and returns a new SQL database connection.
        /// </summary>
        /// <returns>A new IDbConnection object.</returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
