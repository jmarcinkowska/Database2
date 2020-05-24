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

        public static String getDepartment (String ID) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, "CentralnyBank");
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand krakowDepartment = new SqlCommand ("SELECT COUNT(*) FROM Klient WHERE ID_Oddzial LIKE 'KR%' AND ID = @ID", connection);
                krakowDepartment.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                int krakowExists = (int) krakowDepartment.ExecuteScalar ();

                if (krakowExists > 0) {
                    return "OddzialKrakow";
                } else
                    return "OddzialWarszawa";
            }
        }

    }
}