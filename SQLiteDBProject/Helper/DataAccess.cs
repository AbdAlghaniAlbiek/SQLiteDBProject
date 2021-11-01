using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteDBProject.Model;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace SQLiteDBProject.Helper
{
    public static class DataAccess
    {
        private static readonly string _databasePath = "Automata.db";

        public static void InitializeDatabase()
        {
            //In this statement It creates db and store it in local data store
            using (SqliteConnection conn = new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                //this statement Initializes a employee's table 
                string tableCommand = "CREATE TABLE IF NOT EXISTS Employee" +
                                           "(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                           "Email NVARCHAR(2048) NOT NULL UNIQUE," +
                                           "Password NVARCHAR(2048) NOT NULL," +
                                           "Name NVARCHAR(2048) NOT NULL);";

                SqliteCommand createTable = new SqliteCommand(tableCommand, conn);

                createTable.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Employee> GetEmployees()
        {
            ObservableCollection<Employee> employees = 
                new ObservableCollection<Employee>();

            using (SqliteConnection conn =
                new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                SqliteCommand getEmployeesCommand = new SqliteCommand("SELECT * FROM Employee", conn);

                SqliteDataReader query = getEmployeesCommand.ExecuteReader();

                while (query.Read())
                {
                    //You can use this as well as ;)
                    //employees.Add(new Employee()
                    //{
                    //    Id = (int)query["Id"],
                    //    Email = (string)query["Email"],
                    //    Password = (string)query["Password"],
                    //    Name = (string)query["Name"]
                    //});

                    employees.Add(new Employee()
                    {
                        Id = query.GetInt32(0),
                        Email = query.GetString(1),
                        Password = query.GetString(2),
                        Name = query.GetString(3)
                    });
                }
                conn.Close();
            }

            return employees;
        }

        public static void AddEmployee(Employee employee)
        {
            using (SqliteConnection conn = 
                new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                SqliteCommand addEmployeeCommand = new SqliteCommand();
                addEmployeeCommand.Connection = conn;

                // Use parameterized query to prevent SQL injection attacks
                addEmployeeCommand.CommandText = "INSERT INTO Employee VALUES (@Entry0, @Entry1, @Entry2, @Entry3);";
                addEmployeeCommand.Parameters.AddWithValue("@Entry0", employee.Id);
                addEmployeeCommand.Parameters.AddWithValue("@Entry1", employee.Email);
                addEmployeeCommand.Parameters.AddWithValue("@Entry2", employee.Password);
                addEmployeeCommand.Parameters.AddWithValue("@Entry3", employee.Name);

                addEmployeeCommand.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void UpdateEmployee(int id, Employee employee)
        {
            using (SqliteConnection conn = 
                new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                SqliteCommand updateEmployeeCommand = new SqliteCommand();
                updateEmployeeCommand.Connection = conn;

                // Use parameterized query to prevent SQL injection attacks
                //please attention to the distance between @Entry3 and WHERE
                updateEmployeeCommand.CommandText = "UPDATE Employee SET Password=@Entry1," +
                                                                        "Name=@Entry2 " +
                                                                        "WHERE Id = @Entry3;";
                updateEmployeeCommand.Parameters.AddWithValue("@Entry1", employee.Password);
                updateEmployeeCommand.Parameters.AddWithValue("@Entry2", employee.Name);
                updateEmployeeCommand.Parameters.AddWithValue("@Entry3", id);

                updateEmployeeCommand.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void DeleteEmployee(int id)
        {
            using (SqliteConnection conn = 
                new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                SqliteCommand deleteEmployeeCommand = new SqliteCommand();
                deleteEmployeeCommand.Connection = conn;

                deleteEmployeeCommand.CommandText = "DELETE FROM Employee WHERE Id=@Entry;";
                deleteEmployeeCommand.Parameters.AddWithValue("@Entry", id);

                deleteEmployeeCommand.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static bool EmailIsFound(string email)
        {
            bool isFound = false;

            using (SqliteConnection conn =
                new SqliteConnection("Filename=" + _databasePath))
            {
                conn.Open();

                SqliteCommand findEmailCommand = new SqliteCommand();
                findEmailCommand.Connection = conn;

                findEmailCommand.CommandText = "SELECT Email FROM Employee WHERE Email LIKE @Entry;";
                findEmailCommand.Parameters.AddWithValue("@Entry", email);

                SqliteDataReader query = findEmailCommand.ExecuteReader();

                while (query.Read())
                {
                    if (!string.IsNullOrEmpty(query.GetString(0)))
                    {
                        isFound = true;
                    }
                }
                conn.Close();
            }

            return isFound;
        }
    }
}
