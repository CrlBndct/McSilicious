namespace MyLibrary;

public class Cashier
{
    public static void Meth()
    {
        bool isCashierRunning = true;

        while (isCashierRunning)
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("         CASHIER VIEW");
            Console.WriteLine("==============================");
            Console.WriteLine("[1] Order Lookup");
            Console.WriteLine("[2] Sales Report");
            Console.WriteLine("[3] Delete Order");
            Console.WriteLine("[0] Return to Main Menu");
            Console.WriteLine("==============================");

            Console.Write("Enter choice: ");
            string? input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    RestoMeth.FindOrder();
                    break;

                case "2":
                    RestoMeth.ShowSales();
                    break;

                case "3":
                    RestoMeth.DeleteOrder();
                    break;

                case "0":
                    isCashierRunning = false;
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