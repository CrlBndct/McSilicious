using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;

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



class RestoMeth
{
    public static void IterOrders(List<Food> ord)
    {
        Console.WriteLine($"{"Product", -15} {"Quantity", -10}");
        foreach (Food foods in ord.Distinct())
        {
                int qnty = 0;
                for (int k = 0; k < ord.Count; k++)
                {
                    if (foods.itemNum == ord[k].itemNum)
                    {
                        qnty += 1;
                    }
                }
            Console.WriteLine($"{foods.itemNum, -15} {"x"+qnty, -10}");
        }
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
            Console.WriteLine($"{"ID", -5} {"Food Name", -15} Price");
            for (int i = 0; i < foodMenu.Length; i++)
            {
                Console.WriteLine($"{"["+foodMenu[i].itemNum+"]", -5} {foodMenu[i].foodName, -15} Php {foodMenu[i].price}");
            }
            Console.WriteLine("[c] Confirm order");
            Console.WriteLine("[cc] Cancel order");

            Console.WriteLine();

            Console.Write("Enter Food ID: ");
            string itemId = Console.ReadLine();
            int sel;

            Console.WriteLine();

            if (int.TryParse(itemId, out sel))
            {
                if (sel > 0 || sel <= foodMenu.Length)
                {
                    orders.Add(foodMenu[sel - 1]);
                    // RestoMeth.IterOrders(orders);
                    // foreach (Food t in orders)
                    // {
                    //     Console.WriteLine($"{t.foodName}");
                    // }
                }
                else
                {
                    Console.WriteLine("Invalid number");
                    Console.WriteLine();
                }
            }else if (itemId == "c")
            {
                break;
                
            }else if (itemId == "cc")
            {
                orders.Clear();
                RestoMeth.IterOrders(orders);
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

        RestoMeth.IterOrders(orders);
    }

    static void Main(string[] args)
    {
        bool isRunning = true;

        do
        {
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


