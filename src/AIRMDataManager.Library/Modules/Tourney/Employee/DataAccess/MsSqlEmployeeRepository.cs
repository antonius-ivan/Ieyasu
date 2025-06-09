using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Employee.Models;
using AIRMDataManager.Library.SystemCoreDataAccess;

namespace AIRMDataManager.Library.Modules.Tourney.Employee.DataAccess
{
    public class MsSqlEmployeeRepository : IEmployeeRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ISqlDataAccess _sqlDataAccess;

        public MsSqlEmployeeRepository(IDatabaseConnectionFactory connectionFactory, ISqlDataAccess sqlDataAccess)
        {
            _connectionFactory = connectionFactory;
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<mst_employee>> GetAllEmployeesAsync()
        {
            const string storedProcedure = "sp_Employee_GetAll";
            const string databaseCode = "DefaultConnection"; // adjust as needed

            var employees = await _sqlDataAccess.LoadDataStoredProcedureAsync<mst_employee, dynamic>(storedProcedure, null, databaseCode);
            return employees;
        }

        public async Task<mst_employee?> GetEmployeeByIdAsync(int id)
        {
            const string storedProcedure = "sp_Employee_GetById";
            const string databaseCode = "DefaultConnection";

            var parameters = new { id };
            var employees = await _sqlDataAccess.LoadDataStoredProcedureAsync<mst_employee, dynamic>(storedProcedure, parameters, databaseCode);
            return employees.FirstOrDefault();
        }

        public async Task<int> InsertEmployeeAsync(mst_employee employee)
        {
            const string storedProcedure = "sp_Employee_Insert";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                employee.first_name,
                employee.last_name,
                employee.email,
                employee.department,
                employee.job_title,
                employee.hire_date,
                employee.salary,
                employee.is_active,
                employee.comments,
                created_at = DateTime.Now,
                created_by = "SYS"
            };

            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);
            // If your SP returns the new ID, capture and return it; otherwise return 0 or throw
            return 0;
        }

        public async Task<int> UpdateEmployeeAsync(mst_employee employee)
        {
            const string storedProcedure = "sp_Employee_Update";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                employee.id,
                employee.first_name,
                employee.last_name,
                employee.email,
                employee.department,
                employee.job_title,
                employee.hire_date,
                employee.salary,
                employee.is_active,
                employee.comments,
                updated_by = "SYS"    // SP sets updated_at itself
            };

            // <-- use the async SP method
            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);

            return employee.id;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            const string storedProcedure = "sp_Employee_Delete";
            const string databaseCode = "DefaultConnection";

            var parameters = new { id };

            try
            {
                await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);
                return true;
            }
            catch
            {
                // Log or handle exception as needed
                return false;
            }
        }
    }
}
