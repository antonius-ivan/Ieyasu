using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;
using AIRMDataManager.Library.SystemCoreDataAccess;

namespace AIRMDataManager.Library.Modules.Tourney.Person.DataAccess
{
    public class MsSqlPersonRepository : IPersonRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ISqlDataAccess _sqlDataAccess;
        private const string DatabaseCode = "DefaultConnection";

        public MsSqlPersonRepository(
            IDatabaseConnectionFactory connectionFactory,
            ISqlDataAccess sqlDataAccess)
        {
            _connectionFactory = connectionFactory;
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<tnm_person>> GetAllPersonsAsync()
        {
            const string storedProcedure = "sp_Person_GetAll";
            // no parameters
            var list = await _sqlDataAccess
                .LoadDataStoredProcedureAsync<tnm_person, dynamic>(storedProcedure, null, DatabaseCode);
            return list;
        }

        public async Task<tnm_person?> GetPersonByIdAsync(int id)
        {
            const string storedProcedure = "sp_Person_GetById";
            var parameters = new { id };
            var list = await _sqlDataAccess
                .LoadDataStoredProcedureAsync<tnm_person, dynamic>(storedProcedure, parameters, DatabaseCode);
            return list.FirstOrDefault();
        }

        public async Task<int> InsertPersonAsync(tnm_person person)
        {
            const string storedProcedure = "sp_Person_Insert";
            var parameters = new
            {
                person.person_firstname,
                person.person_lastname,
                // if fullname is a computed column, you can omit it here
                person_fullname = $"{person.person_firstname} {person.person_lastname}",
                person.person_email,
                w_cellphone1 = person.w_cellphone1,
                h_cellphone1 = person.h_cellphone1,
                cre_dttm = DateTime.Now,
                cre_by = "SYS"
            };

            // If your SP returns the new ID as a result set, you could LoadDataStoredProcedureAsync<int,...>
            await _sqlDataAccess
                .SaveDataStoredProcedureAsync(storedProcedure, parameters, DatabaseCode);

            // Adjust if you retrieve the new ID from the SP
            return 0;
        }

        public async Task<int> UpdatePersonAsync(tnm_person person)
        {
            const string storedProcedure = "sp_Person_Update";
            var parameters = new
            {
                person.id,
                person.person_firstname,
                person.person_lastname,
                person_fullname = $"{person.person_firstname} {person.person_lastname}",
                person.person_email,
                w_cellphone1 = person.w_cellphone1,
                h_cellphone1 = person.h_cellphone1,
                upd_dttm = DateTime.Now,
                upd_by = "SYS"
            };

            await _sqlDataAccess
                .SaveDataStoredProcedureAsync(storedProcedure, parameters, DatabaseCode);

            return person.id;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            const string storedProcedure = "sp_Person_Delete";
            var parameters = new { id };

            try
            {
                await _sqlDataAccess
                    .SaveDataStoredProcedureAsync(storedProcedure, parameters, DatabaseCode);
                return true;
            }
            catch
            {
                // log or rethrow as needed
                return false;
            }
        }
    }
}
