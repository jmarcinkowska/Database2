using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace projekt {
    class DatabaseConnection {
        public static String mainConnection = "Data Source = MSSQLServer; INITIAL CATALOG = {0}; INTEGRATED SECURITY = SSPI";

        static public void connectToDatabase (string databaseName) {
            try {
                String connectionString = String.Format (mainConnection, databaseName);
                using (SqlConnection connection = new SqlConnection (connectionString)) {
                    connection.Open ();
                    if (connection.State != System.Data.ConnectionState.Open)
                        throw new Exception ("Nie można połączyć z bazą");

                }
            } catch (SqlException e) {
                Console.WriteLine ("Problem z połączeniem z bazą");
                System.Environment.Exit (1);
            }
        }

    }
}