using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using MyLibrary;
using System.IO;


class Program
{
    static void Main(string[] args)
    {
        bool isRunning = true;
        Console.WriteLine("======= McSilicious =======");
        do
        {
            Console.WriteLine("[1] Cashier \n[2] Customer \n[3] Done");

            Console.Write("Enter Role: ");
            int r = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (r)
            {
                case 2:
                    Customer.Meth();
                    break;
            }

        } while (isRunning);
    }
}