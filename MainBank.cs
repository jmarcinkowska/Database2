using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace projekt {
    class MainBank {
        private static String databaseName = "CentralnyBank";
        private static String krakowBranch = "OddzialKrakow";
        private static String warszawaBranch = "OddzialWarszawa";

        public static List<Client> searchClient (String ID) {
            List<Client> client = new List<Client> ();

            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);

            try {
                using (SqlConnection connection = new SqlConnection (sqlconnection)) {

                    connection.Open ();
                    using (SqlCommand command = new SqlCommand ("SELECT k.ID, Imie, Nazwisko, Miasto, PESEL, Saldo, ID_Oddzial, Nazwa FROM Klient k join Oddzial o on  k.ID_Oddzial = o.ID WHERE k.ID = @ID", connection)) {
                        command.Parameters.Add ("@ID", SqlDbType.NVarChar, 10).Value = ID;
                        SqlDataReader dataReader = command.ExecuteReader ();
                        while (dataReader.Read ()) {
                            client.Add (new Client () {
                                ID = (String) dataReader.GetValue (0),
                                    Name = (String) dataReader.GetValue (1),
                                    Surname = (String) dataReader.GetValue (2),
                                    City = (String) dataReader.GetValue (3),
                                    PESEL = (String) dataReader.GetValue (4),
                                    Balance = (double) dataReader.GetValue (5),
                                    DepartmentID = (String) dataReader.GetValue (6)
                            });
                            Console.WriteLine ("ID: {0}\nImię: {1}\nNazwisko: {2}\nMiasto: {3}\nPESEL: {4}\nSaldo: {5}zł\nID Oddziału: {6}\nNazwa oddziału: {7}", dataReader["ID"].ToString (),
                                dataReader["Imie"].ToString (), dataReader["Nazwisko"].ToString (),
                                dataReader["Miasto"].ToString (), dataReader["PESEL"].ToString (),
                                dataReader["Saldo"].ToString (), dataReader["ID_Oddzial"].ToString (),
                                dataReader["Nazwa"].ToString ());

                        }
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.Message);
            }

            return client;
        }

        public static bool checkClientID (String ID) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand command = new SqlCommand ("SELECT COUNT(*) FROM Klient WHERE ID = @ID", connection);
                command.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                int userExists = (int) command.ExecuteScalar ();
                if (userExists > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool sendMoney (double amountOfMoney, String SenderID, String ReciverID) {
            double clientBalance;
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);

            String reciverBank = DatabaseConnection.getDepartment (ReciverID);
            String senderBank = DatabaseConnection.getDepartment (SenderID);

            try {
                using (TransactionScope oTran = new TransactionScope ()) {
                    using (SqlConnection connection = new SqlConnection (sqlconnection)) {

                        connection.Open ();

                        using (SqlCommand command = new SqlCommand ("SELECT Saldo FROM Klient WHERE ID = @SenderID", connection)) {
                            command.Parameters.Add ("@SenderID", SqlDbType.Float, 10).Value = SenderID;
                            clientBalance = (double) command.ExecuteScalar ();

                            if (clientBalance - amountOfMoney < 0) {
                                Console.WriteLine ("Niewystarczająca ilość pieniędzy");
                                return false;
                            }

                            //uaktualnienie stanu konto osoby, która wysyła przelew
                            String cmd = "UPDATE Klient SET Saldo = Saldo - @amountOfMoney where ID = @SenderID";
                            SqlCommand command2 = new SqlCommand (cmd, connection);
                            command2.Parameters.Add ("@SenderID", SqlDbType.NVarChar).Value = SenderID;
                            command2.Parameters.Add ("@amountOfMoney", SqlDbType.NVarChar).Value = amountOfMoney;
                            command2.ExecuteNonQuery ();

                            // uaktualnienie stanu konta osoby, która odbiera przelew
                            String cmd2 = "UPDATE Klient SET Saldo = Saldo + @amountOfMoney where ID = @ReciverID";
                            SqlCommand command3 = new SqlCommand (cmd2, connection);
                            command3.Parameters.Add ("@ReciverID", SqlDbType.NVarChar).Value = ReciverID;
                            command3.Parameters.Add ("@amountOfMoney", SqlDbType.NVarChar).Value = amountOfMoney;
                            command3.ExecuteNonQuery ();

                            Department.sendMoneyDepartment (amountOfMoney, SenderID, ReciverID, true, senderBank);
                            Department.sendMoneyDepartment (amountOfMoney, ReciverID, SenderID, false, reciverBank);

                        }
                    }

                    oTran.Complete ();
                    return true;
                }
            } catch (SqlException e) {
                Console.WriteLine (e.Message);
                return false;
            }
        }

    }

}