using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace projekt {
    class Department {
        public static void sendMoneyDepartment (double amountOfMoney, String ID1, String ID2, bool isSender, String department) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand balance = new SqlCommand ("SELECT Saldo FROM Klient where ID = @ID1", connection);
                balance.Parameters.Add ("@ID1", SqlDbType.NVarChar).Value = ID1;
                //Console.WriteLine("balans " + (double)balance.ExecuteScalar());

                if ((double) balance.ExecuteScalar () - amountOfMoney < 0) {
                    Console.WriteLine ("Niewystarczająca ilość pieniędzy na koncie");
                    System.Environment.Exit (1);
                }

                if (isSender) {

                    SqlCommand updateSender = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amountOfMoney where ID = @ID1", connection);
                    updateSender.Parameters.Add ("@ID1", SqlDbType.NVarChar).Value = ID1;
                    updateSender.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateSender.ExecuteNonQuery ();
                    //Console.WriteLine("tutaj jest sender");
                    Console.WriteLine (department);
                }
                if (!isSender) {
                    SqlCommand updateReciver = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amountOfMoney where ID = @ID2", connection);
                    updateReciver.Parameters.Add ("@ID2", SqlDbType.NVarChar).Value = ID1;
                    updateReciver.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateReciver.ExecuteNonQuery ();
                    //Console.WriteLine("tutaj jest reciver");
                    Console.WriteLine (department);
                }

                String name = "Przelew";

                saveTransaction (amountOfMoney, ID1, ID2, department, isSender, name);
            }

        }

        public static void sendSameDepartment (double amountOfMoney, String ID1, String ID2, bool isSender, String department) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand balance = new SqlCommand ("SELECT Saldo FROM Klient where ID = @ID1", connection);
                balance.Parameters.Add ("@ID1", SqlDbType.NVarChar).Value = ID1;

                if ((double) balance.ExecuteScalar () - amountOfMoney < 0) {
                    Console.WriteLine ("Niewystarczająca ilość pieniędzy na koncie");
                    System.Environment.Exit (1);
                }

                if (isSender) {

                    SqlCommand updateSender = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amountOfMoney where ID = @ID1", connection);
                    updateSender.Parameters.Add ("@ID1", SqlDbType.NVarChar).Value = ID1;
                    updateSender.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateSender.ExecuteNonQuery ();
                    Console.WriteLine (department);
                }
                if (!isSender) {
                    SqlCommand updateReciver = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amountOfMoney where ID = @ID2", connection);
                    updateReciver.Parameters.Add ("@ID2", SqlDbType.NVarChar).Value = ID1;
                    updateReciver.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateReciver.ExecuteNonQuery ();
                    Console.WriteLine (department);
                }
            }

        }

        public static void saveTransaction (double amount, String sender, String reciver, String department, bool isSender, String name) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                //Console.WriteLine("D E P A R T A M E N T " + department);
                connection.Open ();
                String guid = Guid.NewGuid ().ToString ();
                SqlCommand transaction = new SqlCommand ("INSERT INTO Transakcje VALUES(@ID, @amount, @sender, @reciver, @date, @name)", connection);
                transaction.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                transaction.Parameters.Add ("@date", SqlDbType.DateTime).Value = DateTime.Now;
                transaction.Parameters.Add ("@name", SqlDbType.NVarChar).Value = name;
                transaction.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = guid;

                if (isSender) {
                    //Console.WriteLine("transaction SENDER");
                    transaction.Parameters.Add ("@sender", SqlDbType.NVarChar).Value = sender;
                    transaction.Parameters.Add ("@reciver", SqlDbType.NVarChar).Value = reciver;
                } else {
                    //Console.WriteLine("transaction RECIVER");
                    transaction.Parameters.Add ("@sender", SqlDbType.NVarChar).Value = reciver;
                    transaction.Parameters.Add ("@reciver", SqlDbType.NVarChar).Value = sender;
                }

                transaction.ExecuteNonQuery ();
            }
        }

        public static void transaction (double amount, String ID, String department, String name) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                String guid = Guid.NewGuid ().ToString ();
                SqlCommand transaction = new SqlCommand ("INSERT INTO Transakcje VALUES(@ID, @amount, @sender, @reciver, @date, @name)", connection);
                transaction.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                transaction.Parameters.Add ("@date", SqlDbType.DateTime).Value = DateTime.Now;
                transaction.Parameters.Add ("@name", SqlDbType.NVarChar).Value = name;
                transaction.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = guid;
                transaction.Parameters.Add ("@sender", SqlDbType.NVarChar).Value = ID;
                transaction.Parameters.Add ("@reciver", SqlDbType.NVarChar).Value = ID;

                transaction.ExecuteNonQuery ();
            }
        }

        public static void setSender (double amount, String ID, String department) {
            //Console.WriteLine("setSender");
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand updateAmount = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amount WHERE ID = @ID", connection);
                updateAmount.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                updateAmount.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                updateAmount.ExecuteNonQuery ();
                Console.WriteLine ("..");

            }
        }

        public static void setReciver (double amount, String ID, String department) {
            //Console.WriteLine("setReciver");
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand updateAmount = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amount WHERE ID = @ID", connection);
                updateAmount.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                updateAmount.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                updateAmount.ExecuteNonQuery ();
                Console.WriteLine ("..........");
            }
        }

        public static void withdrawMoneyDepartment (String department, String ID, double amount) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();

                SqlCommand withdraw = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amount WHERE ID = @ID", connection);
                withdraw.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                withdraw.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                withdraw.ExecuteNonQuery ();

                transaction (amount, ID, department, "Wypłata pieniędzy");
            }
        }

        public static void depositMoneyDepartment (String department, String ID, double amount) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();

                SqlCommand withdraw = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amount WHERE ID = @ID", connection);
                withdraw.Parameters.Add ("@ID", SqlDbType.NVarChar).Value = ID;
                withdraw.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                withdraw.ExecuteNonQuery ();

                transaction (amount, ID, department, "Wpłata pieniędzy");
            }
        }
    }
}