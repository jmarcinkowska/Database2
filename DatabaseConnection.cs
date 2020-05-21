using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace projekt
{
    class DatabaseConnection
    {
        public static String mainConnection = "Data Source = MSSQLServer; INITIAL CATALOG = {0}; INTEGRATED SECURITY = SSPI";

        static public void connectToDatabase(string databaseName)
        {
            try
            {
                String connectionString = String.Format(mainConnection, databaseName);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    validateConnection(connection);

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Problem z połączeniem z bazą");
                Console.ReadKey();
                System.Environment.Exit(1);
            }
        }
        static public void validateConnection(SqlConnection sqlConn)
        {
            if (sqlConn.State != System.Data.ConnectionState.Open)
                throw new Exception("Nie można połączyć z bazą");
        }
    }
}
