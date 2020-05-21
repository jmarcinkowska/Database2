using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace projekt {
    class CentralBank {
        private String databaseName = "CentralnyBank";

        public List<Client> searchClient (String ID) {
            List<Client> client = new List<Client> ();

            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);

            try {
                using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                    SqlCommand command = new SqlCommand ("SELECT * FROM Klient WHERE ID = @ID", connection);
                    command.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                    connection.Open ();

                    SqlDataReader dataReader = command.ExecuteReader ();
                    while (dataReader.Read ()) {

                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.Message);
            }

            return client;
        }
    }
}