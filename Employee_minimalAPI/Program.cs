
using Employee_minimalAPI.BusinessLayer;
using Employee_minimalAPI.DataAccessLayer;
using Employee_minimalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Xml.Serialization;

namespace Employee_minimalAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                string connString = builder.Configuration["ConnectionStrings:Mssql"];
                return new SqlConnection(connString);
            });

            builder.Services.AddScoped<IDBDal, DBDal>();
            builder.Services.AddScoped<EmployeeService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.MapGet("/getAllEmployees", async (EmployeeService service) =>
            {
                List<Employee> employees = await service.GetAllEmployees();
                return Results.Ok(employees);
            });

            app.MapGet("/getEmployeesByBirthDate", async (EmployeeService service, DateTime date) =>
            {
                List<Employee> employees = await service.GetAllEmployeesByBirthDate(date);
                return Results.Ok(employees);
            });

            app.MapPost("/insertNewEmployee", async (Employee employee, EmployeeService service) =>
            {
                 bool success = await service.InsertEmployee(employee);
                
                if (success) return Results.Ok();
                return Results.Content("Fehler bei den Daten");
            });

            app.MapDelete("/deleteEmployeeById", async (int Id, EmployeeService service) =>
            {
                bool success = await service.DeleteEmployee(Id);
                if (success) return Results.Ok();
                return Results.Content("Fehler bei den Daten");
            });
            app.MapPatch("/updateEmployee", async (Employee employee, EmployeeService service) =>
            {
                bool success = await service.UpdateEmployee(employee);
                if (success) return Results.Ok();
                return Results.Content("Fehler bei den Daten");

            });

            app.Run();
        }
    }
}
