using System.Collections.Generic;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Person.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;

namespace AiclaRM.Server.Services.Tourney.Person
{

    // 2) Concrete implementation
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Task<List<tnm_person>> GetAllPersonsAsync()
            => _personRepository.GetAllPersonsAsync();

        public async Task<tnm_person?> GetPersonByIdAsync(int id)
        {
            // return null if not found
            return await _personRepository.GetPersonByIdAsync(id);
        }

        public async Task<int> InsertPersonAsync(tnm_person person)
            => await _personRepository.InsertPersonAsync(person);

        public async Task<int> UpdatePersonAsync(tnm_person person)
            => await _personRepository.UpdatePersonAsync(person);

        public async Task<bool> DeletePersonAsync(int id)
            => await _personRepository.DeletePersonAsync(id);
    }
}
