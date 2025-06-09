using AIRMDataManager.Library.Modules.Tourney.Person.Models;

namespace AiclaRM.Server.Services.Tourney.Person
{

    // 1) Service interface
    public interface IPersonService
    {
        Task<List<tnm_person>> GetAllPersonsAsync();
        Task<tnm_person?> GetPersonByIdAsync(int id);
        Task<int> InsertPersonAsync(tnm_person person);
        Task<int> UpdatePersonAsync(tnm_person person);
        Task<bool> DeletePersonAsync(int id);
    }
}
