using Employee_minimalAPI.Model;

namespace Employee_minimalAPI.BusinessLayer
{
    public interface IEmployeeService
    {
        Task<bool> DeleteEmployee(int id);
        Task<List<Employee>> GetAllEmployees();
        Task<List<Employee>> GetAllEmployeesByBirthDate(DateTime birthdate);
        Task<bool> InsertEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
    }
}