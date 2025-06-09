using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;
using AIRMDataManager.Library.SystemCoreDataAccess;

namespace AIRMDataManager.Library.Modules.Tourney.Prize.DataAccess
{
    public class MsSqlPrizeRepository : IPrizeRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ISqlDataAccess _sqlDataAccess;

        public MsSqlPrizeRepository(IDatabaseConnectionFactory connectionFactory, ISqlDataAccess sqlDataAccess)
        {
            _connectionFactory = connectionFactory;
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<mst_prize>> GetAllPrizesAsync()
        {
            // Example: Use _connectionFactory.CreateConnection() to get a DB connection,
            // then execute your query asynchronously.
            // For demonstration, we return a dummy list:
            //await Task.CompletedTask;
            //return new List<PrizeModel>
            //{
            //    new PrizeModel { Id = 1, PlaceName = "Champion Trophy", PrizeAmount = 1000 },
            //    new PrizeModel { Id = 2, PlaceName = "Runner-Up Medal", PrizeAmount = 500 }
            //};

            // Assuming you have a stored procedure named 'spGetCatalogBrands'
            var storedProcedure = "sp_Prize_GetAll";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var brands = await _sqlDataAccess.LoadDataStoredProcedureAsync<mst_prize, dynamic>(storedProcedure, null, databaseCode);
            return brands;
        }

        public async Task<mst_prize?> GetPrizeByIdAsync(int id)
        {
            const string storedProcedure = "sp_Prize_GetById";
            const string databaseCode = "DefaultConnection";

            var parameters = new { id };

            var prizes = await _sqlDataAccess.LoadDataStoredProcedureAsync<mst_prize, dynamic>(storedProcedure, parameters, databaseCode);

            return prizes.FirstOrDefault();
        }


        public async Task<int> InsertPrizeAsync(mst_prize prize)
        {
            const string storedProcedure = "sp_Prize_Insert";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                prize.prize_number,
                prize.prize_nm,
                prize.prize_amt,
                prize.prize_pctg,
                cre_dttm = DateTime.Now, // Use current timestamp for creation
                cre_by = "SYS" // Replace this with actual user context
            };

            // Execute the stored procedure using the provided SaveDataCodeProcedureAsync method
            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);

            // Assuming you want to return the newly created ID, you may need an additional query or output parameter.
            // Here, it's assumed the stored procedure handles returning the ID separately.
            return 0; // Placeholder; adjust as needed if ID retrieval is implemented.
        }

        public async Task<int> UpdatePrizeAsync(mst_prize prize)
        {
            const string storedProcedure = "sp_Prize_Update";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                prize.id,
                prize.prize_number,
                prize.prize_nm,
                prize.prize_amt,
                prize.prize_pctg,
                upd_dttm = DateTime.Now, // Use current timestamp for update
                upd_by = "SYS" // Replace this with actual user context
            };

            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);

            return prize.id; // Returning the updated prize ID
        }

        public async Task<bool> DeletePrizeAsync(int id)
        {
            const string storedProcedure = "sp_Prize_Delete";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                id
            };

            try
            {
                await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);
                return true;
            }
            catch (Exception)
            {
                // You can log the exception if needed
                return false;
            }
        }
    }
}
