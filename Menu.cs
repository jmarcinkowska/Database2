using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projekt {
    class Menu {
        public static void FindClient () {
            String ID;
            Console.Clear ();
            Console.WriteLine ("Wprowadź dane:");
            Console.Write ("PESEL klienta: ");
            ID = Console.ReadLine ();

            List<Client> client = MainBank.searchClient (ID);
            if (client.Count == 0) {
                Console.WriteLine ("Nie znaleziono klienta o podanym ID");
            }
        }

        public static void sendMoney () {
            String SenderID;
            String ReciverID;
            double amountOfMoney;
            Console.Clear ();

            Console.WriteLine ("Podaj ID osoby wysyłającej: ");
            SenderID = Console.ReadLine ();
            if (!MainBank.checkClientID (SenderID)) {
                Console.WriteLine ("Nie ma osoby o podanym numerze ID");
                System.Environment.Exit (0);
            }
            Console.WriteLine ("Podaj ID odbiorcy: ");
            ReciverID = Console.ReadLine ();
            if (!MainBank.checkClientID (ReciverID)) {
                Console.WriteLine ("Nie ma osoby o podanym numerze ID");
                System.Environment.Exit (1);
            }
            Console.WriteLine ("Podaj kwote: ");
            double.TryParse (Console.ReadLine (), out amountOfMoney);

            if (MainBank.sendMoney (amountOfMoney, SenderID, ReciverID)) {
                Console.WriteLine ("Pomyślnie wykonany przelew");
            } else
                Console.WriteLine ("Nie udało się wykonać przelewu");
        }

        public static void Withdraw () {
            String ID;
            double amount;
            Console.Clear ();

            Console.WriteLine ("Podaj ID: ");
            ID = Console.ReadLine ();
            if (!MainBank.checkClientID (ID)) {
                Console.WriteLine ("Nie ma osoby o podanym numerze ID");
                System.Environment.Exit (0);
            }
            Console.WriteLine ("Podaj kwote: ");
            double.TryParse (Console.ReadLine (), out amount);

            MainBank.withdrawMoney (ID, amount);
        }

        public static void Deposit () {
            Console.Clear ();
            String ID;
            double amount;

            Console.WriteLine ("Podaj ID: ");
            ID = Console.ReadLine ();
            if (!MainBank.checkClientID (ID)) {
                Console.WriteLine ("Nie ma osoby o podanym numerze ID");
                System.Environment.Exit (0);
            }
            Console.WriteLine ("Podaj kwote: ");
            double.TryParse (Console.ReadLine (), out amount);

            MainBank.depositMoney (ID, amount);
        }

        public static void register () {
            String name;
            String surname;
            String pesel;
            String city;
            double amount;
            String department;
            String departmentName;

            Console.WriteLine ("Podaj imie: ");
            name = Console.ReadLine ();
            Console.WriteLine ("Podaj nazwisko: ");
            surname = Console.ReadLine ();
            Console.WriteLine ("Podaj PESEL: ");
            pesel = Console.ReadLine ();
            Console.WriteLine ("Podaj Miasto: ");
            city = Console.ReadLine ();
            Console.WriteLine ("Podaj kwotę pieniędzy, którą chcesz wpłacic: ");
            double.TryParse (Console.ReadLine (), out amount);
            Console.WriteLine ("Wybierz oddzial do ktorego chcesz zostać zapisany\n[1] - Oddzial w Krakowie \n[2] - Oddzial w Warszawie: ");
            department = Console.ReadLine ();
            switch (department) {
                case "1":
                    department = "OddzialKrakow";
                    break;
                case "2":
                    department = "OddzialWarszawa";
                    break;
                default:
                    Console.WriteLine ("Nie ma podanego oddzialu");
                    break;
            }

            MainBank.register (name, surname, city, pesel, amount, department);

        }
    }
}