using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Common.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AIRMDataManager.Library.DataAccess.MsSql
{
    public class MsSqlDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public MsSqlDatabaseConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }

    //Commented 2025-03-30 16:44
    //public class MsSqlDatabaseConnectionFactory : IDatabaseConnectionFactory
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly string _connectionString;

    //    public MsSqlDatabaseConnectionFactory(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    //        // Ensure your connection string name matches your appsettings.json
    //    }

    //    /// <summary>
    //    /// Creates and returns a new SQL database connection.
    //    /// </summary>
    //    /// <returns>A new IDbConnection object.</returns>
    //    public IDbConnection CreateConnection()
    //    {
    //        return new SqlConnection(_connectionString);
    //    }
    //}
}
