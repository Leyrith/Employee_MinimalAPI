using Employee_minimalAPI.Model;

namespace Employee_minimalAPI.DataAccessLayer
{
    public interface IDBDal
    {
        Task<bool> deleteEmployee(int id);
        Task<List<Employee>> getAllEmloyeesByBirthDate(DateTime birthDate);
        Task<List<Employee>> getAllEmployees();
        Task<bool> insertEmployee(Employee employee);
        Task<bool> updateEmployee(Employee employee);
    }
}