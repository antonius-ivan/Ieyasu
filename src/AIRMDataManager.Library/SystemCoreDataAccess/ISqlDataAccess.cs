
namespace AIRMDataManager.Library.SystemCoreDataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadDataCodeProcedure<T>(string sqlQuery, object parameters, string pDbCode);
        Task<List<T>> LoadDataCodeProcedureAsync<T>(string sqlQuery, object parameters, string pDbCode);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        Task<List<T>> LoadDataStoredProcedure<T, U>(string storedProcedure, U parameters, string pDbCode);
        Task<List<T>> LoadDataStoredProcedureAsync<T, U>(string storedProcedure, U parameters, string pDbCode);
        void RollbackTransaction();
        void SaveDataCodeProcedure<T>(string sqlQuery, T parameters, string pDbCode);
        Task SaveDataCodeProcedureAsync<T>(string sqlQuery, T parameters, string pDbCode);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void SaveDataStoredProcedure<T>(string storedProcedure, T parameters, string pDbCode);
        Task SaveDataStoredProcedureAsync<T>(string storedProcedure, T parameters, string pDbCode);
        void StartTransaction(string pDbCode);
    }
}