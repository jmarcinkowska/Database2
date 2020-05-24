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

        public static List<Client> searchClient (String ID) {

            List<Client> client = new List<Client> ();

            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);

            try {
                using (SqlConnection connection = new SqlConnection (sqlconnection)) {

                    connection.Open ();
                    using (SqlCommand command = new SqlCommand ("SELECT k.ID, Imie, Nazwisko, Miasto, PESEL, Saldo FROM Klient k WHERE k.ID = @ID", connection)) {
                        command.Parameters.Add ("@ID", SqlDbType.NVarChar, 10).Value = ID;
                        SqlDataReader dataReader = command.ExecuteReader ();
                        while (dataReader.Read ()) {
                            client.Add (new Client () {
                                ID = (String) dataReader.GetValue (0),
                                    Name = (String) dataReader.GetValue (1),
                                    Surname = (String) dataReader.GetValue (2),
                                    City = (String) dataReader.GetValue (3),
                                    PESEL = (String) dataReader.GetValue (4),
                                    Balance = (double) dataReader.GetValue (5)
                            });
                            Console.WriteLine ("ID: {0}\nImię: {1}\nNazwisko: {2}\nMiasto: {3}\nPESEL: {4}\nSaldo: {5}zł", dataReader["ID"].ToString (),
                                dataReader["Imie"].ToString (), dataReader["Nazwisko"].ToString (),
                                dataReader["Miasto"].ToString (), dataReader["PESEL"].ToString (),
                                dataReader["Saldo"].ToString ());

                        }
                        dataReader.Close ();

                        if (client.Count != 0) {
                            SqlCommand deparment = new SqlCommand ("SELECT Nazwa FROM Oddzial o join Klient_Oddzial ko on o.ID = ko.ID_Oddzial WHERE ko.ID_KLIENT = @ID", connection);
                            deparment.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                            SqlDataReader read = deparment.ExecuteReader ();
                            Console.Write ("Nazwa oddziału: ");
                            while (read.Read ()) {
                                Console.Write ("{0},", read["Nazwa"].ToString ());

                            }
                            Console.WriteLine ();
                            read.Close ();
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

            String reciverBank = getDepartment (ReciverID);
            String senderBank = getDepartment (SenderID);

            String reciverBank2 = "";
            String senderBank2 = "";

            if (checkDepartments (ReciverID))
                reciverBank2 = getOtherDepartment (ReciverID);

            Console.WriteLine ("ODDZIAL " + reciverBank2);

            if (checkDepartments (SenderID))
                senderBank2 = getOtherDepartment (SenderID);

            Console.WriteLine ("ODDZIAL " + senderBank2);

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

                            if (senderBank2.Length != 0) {
                                Department.setSender (amountOfMoney, SenderID, senderBank2);
                            }
                            if (reciverBank2.Length != 0) {
                                Department.setReciver (amountOfMoney, ReciverID, reciverBank2);
                            }

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

        public static String getDepartment (String ID) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, "CentralnyBank");
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand krakowDepartment = new SqlCommand ("SELECT COUNT(*) FROM Klient k join Klient_Oddzial ko on k.ID = ko.ID_Klient WHERE ko.ID_Oddzial LIKE 'KR%' AND k.ID = @ID", connection);
                krakowDepartment.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                int krakowExists = (int) krakowDepartment.ExecuteScalar ();

                if (krakowExists > 0) {
                    return "OddzialKrakow";
                } else
                    return "OddzialWarszawa";
            }
        }

        public static bool checkDepartments (String ID) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, "CentralnyBank");
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand numberOfDepartments = new SqlCommand ("SELECT COUNT(*) FROM Klient_Oddzial WHERE ID_Klient = @ID", connection);
                numberOfDepartments.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                int departments = (int) numberOfDepartments.ExecuteScalar ();
                Console.WriteLine ("ILOSC DEPARTAMENTOW " + departments);

                if (departments == 2)
                    return true;
                else
                    return false;
            }
        }

        public static String getOtherDepartment (String ID) {
            String department;
            if (getDepartment (ID) == "OddzialKrakow")
                department = "OddzialWarszawa";
            else if (getDepartment (ID) == "OddzialWarszawa")
                department = "OddzialKrakow";
            else
                department = "";
            Console.WriteLine ("BRANCH DEPARTMENT " + department);
            return department;
        }

        public static void withdrawMoney (String ID, double amount) {

        }

    }

}