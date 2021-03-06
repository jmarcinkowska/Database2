﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projekt
{

    class Program
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("\n              MINI BANK                \n");
            Console.WriteLine("Wybierz jedną z opcji:\n");
            Console.WriteLine(" [0] - wyjście");
            Console.WriteLine(" [1] - znajdź klienta");
            Console.WriteLine(" [2] - zarejestruj klineta");
            Console.WriteLine(" [3] - wypłać pieniądze");
            Console.WriteLine(" [4] - wpłać pieniądze");
            Console.WriteLine(" [5] - wykonaj przelew");
            Console.WriteLine(" [6] - pokaż transakcje\n");

        }

        static void Main(string[] args)
        {

            DatabaseConnection.connectToDatabase("CentralnyBank");
            DatabaseConnection.connectToDatabase("OddzialKrakow");
            DatabaseConnection.connectToDatabase("OddzialWarszawa");


            ConsoleKeyInfo key;

            do
            {
                DisplayMenu();
                key = Console.ReadKey(false);
                switch (key.KeyChar.ToString())
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        Menu.FindClient();
                        break;
                    case "2":
                        Menu.register();
                        break;
                    case "3":
                        Menu.withdraw();
                        break;
                    case "4":
                        Menu.deposit();
                        break;
                    case "5":
                        Menu.send();
                        break;
                    case "6":
                        Menu.showTransactions();
                        break;
                    default:
                        Console.WriteLine("Niepoprawnie wprowadzone dane");
                        break;
                }

            } while (key.KeyChar.ToString() != "0");


        }


    }
}
