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
                    using (SqlCommand command = new SqlCommand ("SELECT ID, Imie, Nazwisko, Miasto, PESEL, Saldo FROM Klient WHERE PESEL = @PESEL", connection)) {
                        command.Parameters.Add ("@PESEL", SqlDbType.NVarChar).Value = ID;
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
                            SqlCommand deparment = new SqlCommand ("SELECT Nazwa FROM Oddzial o join Klient_Oddzial ko on o.ID = ko.ID_Oddzial join Klient k on ko.ID_Klient = k.ID WHERE k.PESEL = @ID", connection);
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

            Dictionary<String, String> dict1 = new Dictionary<string, string> ();
            Dictionary<String, String> dict2 = new Dictionary<string, string> ();

            dict1 = dep (SenderID);
            dict2 = dep (ReciverID);

            int len = dict1.Count;
            int len2 = dict2.Count;
            int max = Math.Max (len, len2);

            String reciverBank = getDepartment (ReciverID);
            String senderBank = getDepartment (SenderID);

            String reciverBank2 = "";
            String senderBank2 = "";

            bool reciver2 = checkDepartments (ReciverID);
            bool sender2 = checkDepartments (SenderID);

            if ((senderBank == reciverBank) && ((reciver2) || (sender2))) {
                if ((senderBank == "OddzialKrakow") && (reciverBank == "OddzialKrakow") && (sender2))
                    senderBank = "OddzialWarszawa";
                else if ((senderBank == "OddzialWarszawa") && (reciverBank == "OddzialWarszawa") && (sender2))
                    senderBank = "OddzialKrakow";
                else if ((senderBank == "OddzialKrakow") && (reciverBank == "OddzialKrakow") && (reciver2))
                    reciverBank = "OddzialWarszawa";
                else if ((senderBank == "OddzialWarszawa") && (reciverBank == "OddzialWarszawa") && (reciver2))
                    reciverBank = "OddzialKrakow";
            }

            if (reciver2)
                reciverBank2 = getOtherDepartment (reciverBank);

            //Console.WriteLine("ODDZIAL " + reciverBank2);

            if (sender2)
                senderBank2 = getOtherDepartment (senderBank);

            //Console.WriteLine("ODDZIAL " + senderBank2);

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
                            command2.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                            command2.ExecuteNonQuery ();

                            // uaktualnienie stanu konta osoby, która odbiera przelew
                            String cmd2 = "UPDATE Klient SET Saldo = Saldo + @amountOfMoney where ID = @ReciverID";
                            SqlCommand command3 = new SqlCommand (cmd2, connection);
                            command3.Parameters.Add ("@ReciverID", SqlDbType.NVarChar).Value = ReciverID;
                            command3.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                            command3.ExecuteNonQuery ();

                            bool sameDepartment;
                            if (senderBank == reciverBank)
                                sameDepartment = true;
                            else
                                sameDepartment = false;

                            Console.WriteLine ("SAME DEPARTMENT " + sameDepartment);

                            Department.sendMoneyDepartment (amountOfMoney, SenderID, ReciverID, true, senderBank);
                            //Console.WriteLine("D E P " + senderBank);
                            //Console.WriteLine("D E P " + reciverBank);
                            if (!sameDepartment)
                                Department.sendMoneyDepartment (amountOfMoney, ReciverID, SenderID, false, reciverBank);
                            else
                                Department.sendSameDepartment (amountOfMoney, ReciverID, SenderID, false, reciverBank);

                            if (senderBank2.Length != 0) {
                                Console.WriteLine ("TU POWINNO BYC 1");
                                Department.setSender (amountOfMoney, SenderID, senderBank2);
                            }
                            if (reciverBank2.Length != 0) {
                                Console.WriteLine ("TO TEZ SIE POWINNO POJAWIC ");
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
                //Console.WriteLine("ILOSC DEPARTAMENTOW " + departments);

                if (departments == 2)
                    return true;
                else
                    return false;
            }
        }

        public static String getOtherDepartment (String department) {
            String department2;
            if (department == "OddzialKrakow")
                department2 = "OddzialWarszawa";
            else if (department == "OddzialWarszawa")
                department2 = "OddzialKrakow";
            else
                department2 = "";
            //Console.WriteLine("BRANCH DEPARTMENT " + department);
            return department2;
        }

        public static void withdrawMoney (String ID, double amount) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
            Dictionary<String, String> dict = new Dictionary<string, string> ();
            dict = dep (ID);

            try {
                using (TransactionScope scope = new TransactionScope ()) {
                    using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                        connection.Open ();
                        SqlCommand checkBalance = new SqlCommand ("SELECT Saldo FROM Klient WHERE ID = @ID", connection);
                        checkBalance.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;

                        if ((double) checkBalance.ExecuteScalar () - amount < 0) {
                            Console.WriteLine ("Niewystarczajaca ilość pieniędzy do wypłacenia");
                            System.Environment.Exit (1);
                        }

                        SqlCommand withdraw = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amount WHERE ID = @ID", connection);
                        withdraw.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                        withdraw.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                        withdraw.ExecuteNonQuery ();

                        var d = dict.ElementAt (0);
                        String v = d.Value;
                        if (dict.Count == 2) {
                            Console.WriteLine ("Teraz powienien sie wykonac ");
                            Department.withdrawMoneyDepartment (dict["KR1234"], ID, amount);
                            Department.withdrawMoneyDepartment (dict["WA1234"], ID, amount);
                        } else {
                            Department.withdrawMoneyDepartment (v, ID, amount);
                        }
                        Console.WriteLine ("Pomyślnie udało się wypłacić pieniądze");

                        scope.Complete ();
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.Message);
                Console.WriteLine ("Nie udało się wypłacić pieniędzy");
            }
        }

        public static void depositMoney (String ID, double amount) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
            Dictionary<String, String> dict = new Dictionary<string, string> ();
            dict = dep (ID);

            try {
                using (TransactionScope scope = new TransactionScope ()) {
                    using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                        connection.Open ();

                        SqlCommand withdraw = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amount WHERE ID = @ID", connection);
                        withdraw.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                        withdraw.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                        withdraw.ExecuteNonQuery ();

                        var d = dict.ElementAt (0);
                        String v = d.Value;
                        if (dict.Count == 2) {
                            Console.WriteLine ("Teraz powienien sie wykonac ");
                            Department.depositMoneyDepartment (dict["KR1234"], ID, amount);
                            Department.depositMoneyDepartment (dict["WA1234"], ID, amount);
                        } else {
                            Department.depositMoneyDepartment (v, ID, amount);
                        }

                        Console.WriteLine ("Pomyślnie udało się wpłacić pieniądze");

                        scope.Complete ();
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.Message);
                Console.WriteLine ("Nie udało się wpłacić pieniędzy");
            }
        }

        public static String getDepartmentID (String department) {
            String dep = "";
            if (department == "OddzialKrakow")
                dep = "KR1234";
            if (department == "OddzialWarszawa")
                dep = "WA1234";
            return dep;
        }

        public static void register (String name, String surname, String city, String pesel, double balance, String department) {
            try {
                String departmentID = getDepartmentID (department);
                String ID = RandomID ();

                string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
                using (TransactionScope scope = new TransactionScope ()) {
                    using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                        connection.Open ();
                        if (!checkClientID (ID)) {
                            while (checkClientID (ID)) {
                                ID = RandomID ();
                            }
                        }

                        SqlCommand registerClient = new SqlCommand ("INSERT INTO Klient VALUES(@ID, @name, @surname, @city, @pesel, @amount)", connection);
                        registerClient.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                        registerClient.Parameters.Add ("@name", SqlDbType.NVarChar).Value = name;
                        registerClient.Parameters.Add ("@surname", SqlDbType.NVarChar).Value = surname;
                        registerClient.Parameters.Add ("@city", SqlDbType.NVarChar).Value = city;
                        registerClient.Parameters.Add ("@pesel", SqlDbType.NVarChar).Value = pesel;
                        registerClient.Parameters.Add ("@amount", SqlDbType.Float).Value = balance;
                        registerClient.ExecuteNonQuery ();

                        SqlCommand registerDepartment = new SqlCommand ("INSERT INTO Klient_Oddzial VALUES(@ID, @departmentID)", connection);
                        registerDepartment.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                        registerDepartment.Parameters.Add ("departmentID", SqlDbType.NVarChar).Value = departmentID;
                        registerDepartment.ExecuteNonQuery ();
                    }

                    Department.registerClient (ID, name, surname, city, pesel, balance, department);

                    Console.WriteLine ("Pomyślnie udało się zarejestrować użytkownika");
                    Console.WriteLine ("Twój numer ID to: \n" + ID);
                    scope.Complete ();
                }
            } catch (SqlException) {
                Console.WriteLine ("Nie udało się zarejestrować\n");
            }
        }

        public static void getTransaction (String ID) {
            Console.Clear ();
            String department = getDepartment (ID);
            try {
                Department.transaction (ID, department);

            } catch (SqlException) {
                Console.WriteLine ("Nie udało się znaleźć transakcji");
            }

        }

        public static Dictionary<String, String> dep (String ID) {
            Dictionary<String, String> t = new Dictionary<String, String> ();
            List<String> branches = new List<String> ();

            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();

                SqlCommand br = new SqlCommand ("SELECT ID_Oddzial FROM Klient_Oddzial WHERE ID_Klient = @ID", connection);
                br.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                SqlDataReader dataReader = br.ExecuteReader ();
                while (dataReader.Read ()) {
                    branches.Add (dataReader.GetValue (0).ToString ());
                    Console.WriteLine ("BRANCHES " + dataReader.GetValue (0).ToString ());
                }

                for (int i = 0; i < branches.Count; i++) {
                    if (branches[i] == "KR1234")
                        t.Add ("KR1234", "OddzialKrakow");
                    if (branches[i] == "WA1234")
                        t.Add ("WA1234", "OddzialWarszawa");

                }

            }
            return t;
        }

        public static bool checkPesel (String pesel) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, databaseName);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand command = new SqlCommand ("SELECT COUNT(*) FROM Klient WHERE PESEL = @pesel", connection);
                command.Parameters.Add ("@pesel", SqlDbType.NVarChar).Value = pesel;
                int userExists = (int) command.ExecuteScalar ();
                if (userExists == 0)
                    return true;
                else
                    return false;
            }
        }

        public static string RandomID () {
            Random random = new Random ();
            string r = "";
            int i;
            for (i = 0; i < 10; i++) {
                r += random.Next (0, 9).ToString ();
            }
            return r;
        }

    }

}