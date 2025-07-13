using Employee_minimalAPI.DataAccessLayer;
using Employee_minimalAPI.Model;
using System.Text;

namespace Employee_minimalAPI.BusinessLayer
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IDBDal dBdal;
        public EmployeeService(IDBDal dal)
        {
            dBdal = dal;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            List<Employee> employees = await dBdal.getAllEmployees();
            return employees;
        }
        public Task<List<Employee>> GetAllEmployeesByBirthDate(DateTime birthdate)
        {

            Task<List<Employee>> employees = dBdal.getAllEmloyeesByBirthDate(birthdate);
            return employees;
        }
        public async Task<bool> InsertEmployee(Employee employee)
        {
            if (!InsertEmployeeValidate(employee)) return false;
            Task<bool> success = dBdal.insertEmployee(employee);

            return await success;
        }
        public async Task<bool> UpdateEmployee(Employee employee)
        {
            Task<bool> success = dBdal.updateEmployee(employee);
            return await success;
        }
        public async Task<bool> DeleteEmployee(int id)
        {

            bool success = await dBdal.deleteEmployee(id);

            return success;
        }

        #region Hilfsmethoden
        public static bool InsertEmployeeValidate(Employee employee)
        {
            if (employee != null)
            {
                if (!string.IsNullOrWhiteSpace(employee.FirstName) && !string.IsNullOrWhiteSpace(employee.LastName) && employee.BirthDate != null && employee.IsActive != null) return true;
            }


            return false;
        }
        #endregion

    }
}
