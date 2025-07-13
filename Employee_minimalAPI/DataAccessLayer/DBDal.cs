using Dapper;
using Employee_minimalAPI.BusinessLayer;
using Employee_minimalAPI.Model;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Employee_minimalAPI.DataAccessLayer
{
    public class DBDal : IDBDal
    {
        private readonly IDbConnection DBConnection;
        public DBDal(IDbConnection DBConnection)
        {
            this.DBConnection = DBConnection;
        }


        //string connectionString = @"Data Source=Data/Employee_minimal_API.db";
        public async Task<List<Employee>> getAllEmployees()
        {


            string sqlStatement = "SELECT * FROM Employee";

            IEnumerable<Employee> employees = await DBConnection.QueryAsync<Employee>(sqlStatement);

            return employees.AsList();
        }

        public async Task<List<Employee>> getAllEmloyeesByBirthDate(DateTime birthDate)
        {


            string sqlStatement = "SELECT * FROM Employee where BirthDate < @birthD";
            var parameters = new DynamicParameters();
            parameters.Add("@birthD", birthDate, DbType.Date);
            IEnumerable<Employee> employees = await DBConnection.QueryAsync<Employee>(sqlStatement, parameters);

            return employees.AsList();
        }

        public async Task<bool> insertEmployee(Employee employee)
        {


            string sqlStatement = "Insert Into Employee (FirstName, LastName, BirthDate, IsActive) Values(@FirstName,@LastName,@BirthDate,1)";

            int rows = await DBConnection.ExecuteAsync(sqlStatement, new
            {
                employee.FirstName,
                employee.LastName,
                employee.BirthDate
            });



            return rows > 0;
        }
        public async Task<bool> deleteEmployee(int id)
        {
            string sqlStatement = "Delete From Employee Where Id = @id";
            int rows = await DBConnection.ExecuteAsync(sqlStatement, new { ID = id });
            return rows > 0;
        }

        public async Task<bool> updateEmployee(Employee employee)
        {
            List<string> updates = new List<string>();
            var parameters = new DynamicParameters();
            string SqlStatement;

            

            if (!string.IsNullOrWhiteSpace(employee.FirstName))
            {
                updates.Add("FirstName = @FirstName");
                parameters.Add("@FirstName", employee.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(employee.LastName))
            {
                updates.Add("LastName = @LastName");
                parameters.Add("@LastName", employee.LastName);
            }

            if (employee.BirthDate.HasValue)
            {
                updates.Add("BirthDate = @BirthDate");
                parameters.Add("@BirthDate", employee.BirthDate, DbType.Date);
            }

            if (employee.IsActive != null)
            {
                updates.Add("IsActive = @IsActive");
                parameters.Add("@IsActive", employee.IsActive);
            }

            if (updates.Count == 0)
                return false;

            parameters.Add("@Id", employee.Id);
            SqlStatement = $"Update Employee Set {string.Join(",", updates)} where Id = @Id";
            
            int rows = await DBConnection.ExecuteAsync(SqlStatement, parameters);
            return rows > 0;
        }

    }
}
