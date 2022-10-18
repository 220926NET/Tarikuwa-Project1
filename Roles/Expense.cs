using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Expense.database;
using Expense.Models;
using Expense.UI;

// using Microsoft.Data.SqlClient;

namespace Expense.Roles
{
    public class
        Expenses : IExpense //class Expenses implementing the IExpense elemets
    {
        public User Login(User user) //method used to read the user who login
        {
            try
            {
                var connection = DBConn.OpenConnection();
                var query = $"select * from users where Email = '{user.Email}' and Password = '{user.Password}'";
                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                if (reader.Read()) //get the result//the data we execute 
                {
                    var id = (int)reader["Id"];
                    var email = (string)reader["Email"];
                    var password = (string)reader["Password"];
                    var phone = (string)reader["Phone"];
                    var fullname = (string)reader["FullName"];
                    var address = (string)reader["Address"];
                    var role = (string)reader["Role"];

                    var user_ = new User(id, email, password, phone, fullname, address, role);
                    DBConn.CloseConnection();
                    return user_;
                }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBConn.CloseConnection();
            }

            throw new ArgumentException("Sorry, that user doesn't exist. Check credentials");
        }

        public bool CheckEmail(string email)//this method is usedto validate email if it is in database or not
        {
            try
            {
                var connection = DBConn.OpenConnection();
                var query = $"select Email from users where Email = '{email}'";
                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                if (reader.Read()) //get the result//the data we execute 
                {
                    Validators.DisplayMessage(false, "Sorry email already exist. You can't use this.");

                    DBConn.CloseConnection();
                    return true;
                }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBConn.CloseConnection();
            }

            DBConn.CloseConnection();
            return false;
        }

        public void Register(User user) //Registering the user //double check later!!!
        {
            try
            {
                var connection = DBConn.OpenConnection();
                //SqlCommand command = new SqlCommand($"INSERT into users values(@Emai, @password, @Phone, @FullName, @Address, @Role)", connection);//dangerous so don't use it

                //prevent from very very very smart people!!
                var command =
                    new SqlCommand("INSERT into Users values (@Email, @Password, @Phone, @FullName, @Address, @Role)",
                        connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Address", "");
                command.Parameters.AddWithValue("@Role", user.Role);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException)
            {
                Console.WriteLine("sorry something went wrong");
            }
            finally
            {
                DBConn.CloseConnection();
            }
        }


        public List<User> GetAllUsers()//to read all users from the database
        {
            var users = new List<User>();
            try
            {
                var connection = DBConn.OpenConnection();
                var query = "select * from Users";
                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        var id = (int)reader["Id"];
                        var email = (string)reader["Email"];
                        var password = (string)reader["Password"];
                        var phone = (string)reader["Phone"];
                        var fullname = (string)reader["FullName"];
                        var address = (string)reader["Address"];
                        var role = (string)reader["Role"];
                        var a = new User(id, email, password, phone, fullname, address, role);
                        users.Add(a);
                    }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            DBConn.CloseConnection();
            return users;
        }

        public List<Tickets> ViewTicketsToProcess(int managerId)//reading the tickes where the managerid is manager Id and status is pending
        {
            var tickets = new List<Tickets>();
            try
            {
                var connection = DBConn.OpenConnection();
                var query =
                    $"select * from Tickets where ManagerId = @ManagerId and Status = '{Status.Pending.ToString()}'";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ManagerId", managerId);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        int? ManagerId;
                        var id = (int)reader["Id"];
                        var Name = (string)reader["Name"];
                        if (reader["ManagerId"].Equals(DBNull.Value))
                            ManagerId = null;
                        else
                            ManagerId = (int)reader["ManagerId"];
                        var EmployeeId = (int)reader["EmployeeId"];
                        var Price = (double)reader["Price"];
                        var Date = (string)reader["Date"];
                        var Status = (string)reader["Status"];
                        var a = new Tickets(id, Name, ManagerId, EmployeeId, Price, Date, Status);
                        tickets.Add(a);
                    }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            DBConn.CloseConnection();
            return tickets;
        }

        public List<User> GetManagerUsers()//reading all from users where their tole is manager
        {
            var users = new List<User>();
            try
            {
                var connection = DBConn.OpenConnection();
                var query = $"select * from Users where Role = '{Role.Manager.ToString()}'";
                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        var id = (int)reader["Id"];
                        var email = (string)reader["Email"];
                        var password = (string)reader["Password"];
                        var phone = (string)reader["Phone"];
                        var fullname = (string)reader["FullName"];
                        var address = (string)reader["Address"];
                        var role = (string)reader["Role"];
                        var a = new User(id, email, password, phone, fullname, address, role);
                        users.Add(a);
                    }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            DBConn.CloseConnection();
            return users;
        }

        public void SubmitRequest(Tickets tickets)// this method is used to submit tickets
        {
            try
            {
                var connection = DBConn.OpenConnection();
                //SqlCommand command = new SqlCommand($"INSERT into users values(@Emai, @password, @Phone, @FullName, @Address, @Role)", connection);//dangerous so don't use it
                var a = tickets.ManagerId == null ? "null" : $"{tickets.ManagerId}";
                var query =
                    $"insert into Tickets VALUES ('{tickets.Name}', {a}, {tickets.EmployeeId}, {tickets.Price}, '{tickets.Date}', '{tickets.Status}');";
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine($"sorry something went wrong {e.Message}");
            }
            finally
            {
                DBConn.CloseConnection();
            }
        }

        public List<Tickets> ViewPriorTickets(int employeeId)//this method is used to view the previous tickets got submited
        {
            var tickets = new List<Tickets>();
            try
            {
                var connection = DBConn.OpenConnection();
                var query = "select * from Tickets where EmployeeId = @EmployeeId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        int? ManagerId;
                        var id = (int)reader["Id"];
                        var Name = (string)reader["Name"];
                        if (reader["ManagerId"].Equals(DBNull.Value))
                            ManagerId = null;
                        else
                            ManagerId = (int)reader["ManagerId"];
                        var EmployeeId = (int)reader["EmployeeId"];
                        var Price = (double)reader["Price"];
                        var Date = (string)reader["Date"];
                        var Status = (string)reader["Status"];
                        var a = new Tickets(id, Name, ManagerId, EmployeeId, Price, Date, Status);
                        tickets.Add(a);
                    }

                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            DBConn.CloseConnection();
            return tickets;
        }


        public void ChangeTicketStatus(User manager, int ticketId, Status status)//this method is used to change the tickets status
        {
            try
            {
                var connection = DBConn.OpenConnection();
                var query =
                    $"update Tickets set Status = '{status.ToString()}' where Id = '{ticketId}' and ManagerId = '{manager.Id}'";
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBConn.CloseConnection();
            }
        }

        public void UpdateInfo(User user)// this method is used to update information
        {
            //$"update users set Email  = '{user.Email}', Password = '{user.Password}', phone ='{user.Phone}', FullName ='{user.FullName}', Address='{user.Address}', Role = '{user.Role}'";
            try
            {
                var connection = DBConn.OpenConnection();
                //SqlCommand command = new SqlCommand($"INSERT into users values(@Emai, @password, @Phone, @FullName, @Address, @Role)", connection);//dangerous so don't use it
                var query =
                    $"update users set Email  = '{user.Email}', Password = '{user.Password}', phone ='{user.Phone}', FullName ='{user.FullName}', Address='{user.Address}', Role = '{user.Role}' where Id = {user.Id}";
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine($"sorry something went wrong {e.Message}");
            }
            finally
            {
                DBConn.CloseConnection();
            }
        }
    }
}