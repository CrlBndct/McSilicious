using System;
using System.Runtime.CompilerServices;

class Food
{
    public int itemNum;
    public string foodName;
    public float price;
    public Food(int itemNum, string foodName, float price)
    {
        this.itemNum = itemNum;
        this.foodName = foodName;
        this.price = price;
    }
}










class Program
{ 
    
    static List<Food> orders = new List<Food>();
    static void Customer()
    {
        Food[] foodMenu = {new Food(1, "fries", 59), 
                           new Food(2, "Meal 1", 192), 
                           new Food(3, "Meal 2", 200), 
                           new Food(4, "Happy Meal", 235),
                           new Food(5, "Cheese Burger", 89)};
        bool running = true;

        do
        {
            Console.Clear();

            for (int i = 0; i < foodMenu.Length; i++)
            {
                Console.WriteLine($"[{foodMenu[i].itemNum}] {foodMenu[i].foodName, -15} Php {foodMenu[i].price}");
            }

            Console.Write("Enter Food ID: ");
            string itemId = Console.ReadLine();
            int sel;

            if (int.TryParse(itemId, out sel))
            {
                if (sel <= 0 || sel > foodMenu.Length)
                {
                    Console.WriteLine("Invalid number");
                    Console.WriteLine();
                }
                else
                {
                    orders.Add(foodMenu[sel - 1]);
                    foreach (Food t in orders)
                    {
                        Console.WriteLine(t.foodName);
                    }
                    Console.WriteLine();
                }
            }
            else if (!int.TryParse(itemId, out sel))
            {
                Console.WriteLine("Invalid Selection");
            }

            // if (orders.Count != 0)
            // {
            //     Console.WriteLine("[confirm] [cancel]");
            // }
            
        }while(running);
    }














    static void Main(string[] args)
    {
        bool isRunning = true;

        do
        {
            Console.Clear();
            Console.WriteLine("[1] Cashier \n[2] Customer \n[3] Done");

            Console.Write("Enter Role: ");
            int r = int.Parse(Console.ReadLine());

            switch (r)
            {
                case 2:
                    Customer();
                    break;
            }
        } while (isRunning);
    }
}


