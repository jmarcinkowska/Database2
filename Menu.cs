using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projekt
{
    class Menu
    {
        public static void FindClient()
        {
            String name;
            String surname;
            String ID;
            Console.Clear();
            Console.WriteLine("WprowadŸ dane:");
            Console.Write("ID klienta: ");
            ID = Console.ReadLine();
            Console.Write("Imiê: ");
            name = Console.ReadLine();
            Console.Write("Nazwisko: ");
            surname = Console.ReadLine();
        }
    }
}
