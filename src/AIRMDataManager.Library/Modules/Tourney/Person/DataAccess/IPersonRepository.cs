using AIRMDataManager.Library.Modules.Tourney.Person.Models;

namespace AIRMDataManager.Library.Modules.Tourney.Person.DataAccess
{
    public interface IPersonRepository
    {
        Task<bool> DeletePersonAsync(int id);
        Task<List<tnm_person>> GetAllPersonsAsync();
        Task<tnm_person?> GetPersonByIdAsync(int id);
        Task<int> InsertPersonAsync(tnm_person person);
        Task<int> UpdatePersonAsync(tnm_person person);
    }
}
