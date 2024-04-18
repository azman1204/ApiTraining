using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Handlers
{
    public class EmployeeHandler
    {
        const string connectionString = "Server=AZMAN-ASUSG713Q;Database=api_demo;Trusted_Connection=true";
        public static void MapEndPoints(IEndpointRouteBuilder app)
        {
            // define all routes
            app.MapGet("/api/employee", GetList);
            app.MapPost("/api/employee", Insert);
            app.MapPut("/api/employee", Update);
            app.MapGet("/api/employee/{id:int}", Search);
            app.MapDelete("/api/employee/{id:int}", Delete);
        }

        private static IResult Update(Employee emp)
        {
            string sql = $"UPDATE employee SET name = '{emp.name}', age = {emp.age} WHERE id = {emp.id}";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sql, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Results.Ok(emp);
        }

        private static IResult Search(int id)
        {
            Employee employee = new Employee();
            using (SqlConnection conn = new(connectionString))
            {
                String sql = $"SELECT * FROM employee WHERE id = {id}";
                SqlCommand cmd = new(sql, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        employee.id = (int) reader["id"];
                        employee.name = (string) reader["name"];
                        employee.age = (int) reader["age"];
                        Console.WriteLine($"{reader[0]} \t {reader[1]} \t {reader[2]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (employee.id == 0)
            {
                // not data found
                return Results.NotFound(new { Message = "No employee found" });
            } 
            else
            {
                return Results.Ok(employee);
            }
        }

        private static IResult Delete(int id)
        {
            string sql = $"DELETE FROM employee WHERE id = {id}";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sql, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Results.Ok(new {Message = "Deleted Successfully"});
        }

        private static IResult GetList()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new(connectionString))
            {
                String sql = "SELECT * FROM employee";
                SqlCommand cmd = new(sql, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.id = (int) reader[0];
                        employee.name = (string) reader[1];
                        employee.age = (int) reader[2];
                        employees.Add(employee);
                        Console.WriteLine($"{reader[0]} \t {reader[1]} \t {reader[2]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Results.Ok(employees);
        }

        private static IResult Insert(Employee employee)
        {
            string sql = $"INSERT INTO employee(id,name,age) VALUES({employee.id}, '{employee.name}', {employee.age})";
            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sql, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Results.Ok(employee);
        }
    }
}
