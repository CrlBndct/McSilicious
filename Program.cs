
using MyLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("        McSilicious");
            Console.WriteLine("==============================");
            Console.WriteLine("[1] Customer View");
            Console.WriteLine("[2] Cashier View");
            Console.WriteLine("[3] Exit");
            Console.WriteLine("==============================");

            Console.Write("Enter Role: ");
            string? input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Customer.Meth();
                    break;

                case "2":
                    Cashier.Meth();
                    break;

                case "3":
                    isRunning = false;
                    Console.WriteLine();
                    Console.WriteLine("Thank you for using McSilicious!");
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    RestoMeth.Pause();
                    break;
            }
        }
    }
}
