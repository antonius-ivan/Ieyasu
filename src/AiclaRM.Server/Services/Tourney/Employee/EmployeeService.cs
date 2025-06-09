using System.Collections.Generic;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Employee.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Employee.Models;

namespace AiclaRM.Server.Services.Tourney.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<List<mst_employee>> GetAllEmployeesAsync()
            => _employeeRepository.GetAllEmployeesAsync();

        public async Task<mst_employee?> GetEmployeeByIdAsync(int id)
            => await _employeeRepository.GetEmployeeByIdAsync(id);

        public async Task<int> InsertEmployeeAsync(mst_employee employee)
            => await _employeeRepository.InsertEmployeeAsync(employee);

        public async Task<int> UpdateEmployeeAsync(mst_employee employee)
            => await _employeeRepository.UpdateEmployeeAsync(employee);

        public async Task<bool> DeleteEmployeeAsync(int id)
            => await _employeeRepository.DeleteEmployeeAsync(id);
    }
}
