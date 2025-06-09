using AIRMDataManager.Library.Modules.Tourney.Employee.Models;

namespace AIRMDataManager.Library.Modules.Tourney.Employee.DataAccess
{
    public interface IEmployeeRepository
    {
        Task<bool> DeleteEmployeeAsync(int id);
        Task<List<mst_employee>> GetAllEmployeesAsync();
        Task<mst_employee?> GetEmployeeByIdAsync(int id);
        Task<int> InsertEmployeeAsync(mst_employee employee);
        Task<int> UpdateEmployeeAsync(mst_employee employee);
    }
}
