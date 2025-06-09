using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

namespace AIRMDataManager.Library.Modules.Tourney.Prize.DataAccess
{
    public interface IPrizeRepository
    {
        Task<bool> DeletePrizeAsync(int id);
        Task<List<mst_prize>> GetAllPrizesAsync();
        Task<mst_prize?> GetPrizeByIdAsync(int id);
        Task<int> InsertPrizeAsync(mst_prize prize);
        Task<int> UpdatePrizeAsync(mst_prize prize);
    }
}
