using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projekt {
    enum menu {
        exit = 0,
        findClient = 1,
        withdrawMoney = 2,
        depositMoney = 3
    }

    class Program {
        static void Main (string[] args) {

            DatabaseConnection.connectToDatabase ("CentralnyBank");
            DatabaseConnection.connectToDatabase ("OddzialKrakow");
            DatabaseConnection.connectToDatabase ("OddzialWarszawa");

            Console.WriteLine ("               MINI BANK                ");
            Console.WriteLine ("Wybierz jedną z opcji");
            Console.WriteLine (" [0] - wyjście");
            Console.WriteLine (" [1] - znajdź klienta");
            Console.WriteLine (" [2] - wypłać pieniądze");
            Console.WriteLine (" [3] - wpłać pieniądze");

            String key = Console.ReadLine ();

            switch (key) {
                case "0":
                    Environment.Exit (0);
                    break;
                case "1":
                    Menu.FindClient ();
                    break;
                default:
                    Console.WriteLine ("Niepoprawnie wprowadzone dane");
                    break;
            }

        }

    }
}