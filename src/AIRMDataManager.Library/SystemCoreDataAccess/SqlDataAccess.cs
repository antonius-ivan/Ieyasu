using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AIRMDataManager.Library.SystemCoreDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        //private bool disposed = false;
        private IConfiguration _config;
        private ILogger<SqlDataAccess> _logger;
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }
        public string GetConnectionString(string name)
        {
            // Use the null-coalescing operator to return an empty string if the result is null
            return _config.GetConnectionString(name) ?? "";
        }

        public async Task<List<T>> LoadDataStoredProcedure<T, U>(string storedProcedure, U parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                //List<T> rows = connection.QueryAsync<T>(storedProcedure, parameters,
                //    commandType: CommandType.StoredProcedure).ToList();

                //return rows;
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public async Task<List<T>> LoadDataStoredProcedureAsync<T, U>(string storedProcedure, U parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Use 'await' to asynchronously wait for the query result
                var rows = (await connection.QueryAsync<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure)).ToList();

                return rows;
            }
        }
        public void SaveDataStoredProcedure<T>(string storedProcedure, T parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task SaveDataStoredProcedureAsync<T>(string storedProcedure, T parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<List<T>> LoadDataCodeProcedureAsync<T>(string sqlQuery, object parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = (await connection.QueryAsync<T>(sqlQuery, parameters)).ToList();

                return rows;
            }
        }

        public List<T> LoadDataCodeProcedure<T>(string sqlQuery, object parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = connection.Query<T>(sqlQuery, parameters).ToList();

                return rows;
            }
        }
        public async Task SaveDataCodeProcedureAsync<T>(string sqlQuery, T parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlQuery, parameters);
            }
        }

        public void SaveDataCodeProcedure<T>(string sqlQuery, T parameters, string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlQuery, parameters);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string pDbCode)
        {
            var connectionString = GetConnectionString(pDbCode);

            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            isClosed = false;
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            // Check if the connection is null and return an empty list if it is
            if (_connection == null)
            {
                return new List<T>();
            }

            // If the connection is not null, proceed with the query
            var rows = _connection.Query<T>(storedProcedure, parameters,
                           commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;

            //List<T> rows = _connection.Query<T>(storedProcedure, parameters,
            //           commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            //return rows;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            // Check if the connection is null and return an empty list if it is
            if (_connection == null)
            {
                return;
            }

            _connection.Execute(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        private bool isClosed = false;

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    // TODO: Log this issue
                    _logger.LogError(ex, "Commit transaction failed in the dispose method.");
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
