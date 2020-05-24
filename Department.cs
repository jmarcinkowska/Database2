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
                Console.WriteLine ("balans " + (double) balance.ExecuteScalar ());

                if ((double) balance.ExecuteScalar () - amountOfMoney < 0) {
                    Console.WriteLine ("Niewystarczająca ilość pieniędzy na koncie");
                    System.Environment.Exit (1);
                }

                //jezeli transfer jest między różnymi oddziałami
                if (isSender) {

                    SqlCommand updateSender = new SqlCommand ("UPDATE Klient SET Saldo = Saldo - @amountOfMoney where ID = @ID1", connection);
                    updateSender.Parameters.Add ("@ID1", SqlDbType.NVarChar).Value = ID1;
                    updateSender.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateSender.ExecuteNonQuery ();
                    Console.WriteLine ("tutaj jest sender");
                    Console.WriteLine (department);
                }
                if (!isSender) {
                    SqlCommand updateReciver = new SqlCommand ("UPDATE Klient SET Saldo = Saldo + @amountOfMoney where ID = @ID2", connection);
                    updateReciver.Parameters.Add ("@ID2", SqlDbType.NVarChar).Value = ID1;
                    updateReciver.Parameters.Add ("@amountOfMoney", SqlDbType.Float).Value = amountOfMoney;
                    updateReciver.ExecuteNonQuery ();
                    Console.WriteLine ("tutaj jest reciver");
                    Console.WriteLine (department);
                }

                saveTransaction (amountOfMoney, ID1, ID2, department, isSender);
            }

        }

        public static void saveTransaction (double amount, String sender, String reciver, String department, bool isSender) {
            string sqlconnection = String.Format (DatabaseConnection.mainConnection, department);
            using (SqlConnection connection = new SqlConnection (sqlconnection)) {
                connection.Open ();
                SqlCommand transaction = new SqlCommand ("INSERT INTO Transakcje VALUES(@amount, @sender, @reciver, @date)", connection);
                transaction.Parameters.Add ("@amount", SqlDbType.Float).Value = amount;
                transaction.Parameters.Add ("@date", SqlDbType.DateTime).Value = DateTime.Now;

                if (isSender) {
                    transaction.Parameters.Add ("@sender", SqlDbType.NVarChar).Value = sender;
                    transaction.Parameters.Add ("@reciver", SqlDbType.NVarChar).Value = reciver;
                } else {
                    transaction.Parameters.Add ("@sender", SqlDbType.NVarChar).Value = reciver;
                    transaction.Parameters.Add ("@reciver", SqlDbType.NVarChar).Value = sender;
                }

                transaction.ExecuteNonQuery ();
            }
        }
    }
}