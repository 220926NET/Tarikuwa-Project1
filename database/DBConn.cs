using System;
using Microsoft.Data.SqlClient;

// using Microsoft.Data.SqlClient;

namespace Expense.database
{
    public class DBConn
    {
        private static SqlConnection connection;

        public static SqlConnection OpenConnection()
        {
            try
            {
                if (connection == null)
                    connection = new SqlConnection(
                        $"Server=tcp:revature.database.windows.net,1433;Initial Catalog=RevatureDB;Persist Security Info=False;User ID={DBInfo.Username};Password={DBInfo.Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Can't connect to database with {e.Message}");
            }

            return connection;
        }

        public static void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Can't close the database with {e.Message}");
            }
        }
    }
}