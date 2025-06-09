using AIRMDataManager.Library.Modules.Tourney.Employee.Models;

namespace AiclaRM.Server.Services.Tourney.Employee
{
    public interface IEmployeeService
    {
        Task<bool> DeleteEmployeeAsync(int id);
        Task<List<mst_employee>> GetAllEmployeesAsync();
        Task<mst_employee?> GetEmployeeByIdAsync(int id);
        Task<int> InsertEmployeeAsync(mst_employee employee);
        Task<int> UpdateEmployeeAsync(mst_employee employee);
    }
}
