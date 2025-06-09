// PrizeService.cs
using AIRMDataManager.Library.Modules.Tourney.Prize.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

namespace AiclaRM.Server.Services.Tourney.Prize
{
    public class PrizeService : IPrizeService
    {
        private readonly IPrizeRepository _prizeRepository;

        public PrizeService(IPrizeRepository prizeRepository)
        {
            _prizeRepository = prizeRepository;
        }

        public Task<List<mst_prize>> GetAllPrizesAsync()
            => _prizeRepository.GetAllPrizesAsync();

        public async Task<mst_prize?> GetPrizeByIdAsync(int id)
        {
            // return null if not found
            return await _prizeRepository.GetPrizeByIdAsync(id);
        }

        public async Task<int> InsertPrizeAsync(mst_prize prize)
            => await _prizeRepository.InsertPrizeAsync(prize);

        public async Task<int> UpdatePrizeAsync(mst_prize prize)
            => await _prizeRepository.UpdatePrizeAsync(prize);

        public async Task<bool> DeletePrizeAsync(int id)
            => await _prizeRepository.DeletePrizeAsync(id);
    }
}
