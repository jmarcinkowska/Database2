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
            Console.Write ("ID klienta: ");
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
            amountOfMoney = double.Parse (Console.ReadLine ());

            if (MainBank.sendMoney (amountOfMoney, SenderID, ReciverID)) {
                Console.WriteLine ("Pomyślnie wykonany przelew");
            } else
                Console.WriteLine ("Nie udało się wykonać przelewu");
        }
    }
}