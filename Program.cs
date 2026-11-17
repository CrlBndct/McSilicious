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
        float priceSum = 0;
        Console.WriteLine($"{"Food ID", -5}{"Product", 10} {"Quantity", 15}");
        foreach (Food foods in ord.Distinct())
        {
                int qnty = 0;
                float sum = 0;
                for (int k = 0; k < ord.Count; k++)
                {
                    if (foods.itemNum == ord[k].itemNum)
                    {
                        qnty += 1;
                        sum += ord[k].price;
                    }
                }
                priceSum += sum;
            Console.WriteLine($"{foods.itemNum, -5} {foods.foodName, 10} {"x"+qnty, 15} {"PHP"+ sum, 10}");
        }
        Console.WriteLine();
        Console.WriteLine($"Total: PHP {priceSum}");
    }
}


class Program
{ 
    static List<Food> orders = new List<Food>();
    static void Customer()
    {
        Food[] foodMenu = {new Food(1, "fries", 59.00f), 
                           new Food(2, "Meal 1", 192.00f), 
                           new Food(3, "Meal 2", 200.00f), 
                           new Food(4, "Happy Meal", 235.00f),
                           new Food(5, "Cheese Burger", 89.00f)};
        bool running = true;

        Console.WriteLine("======= Customer =======");

        do
        {
            Console.WriteLine($"{"ID", -5} {"Food Name", -15} Price");
            for (int i = 0; i < foodMenu.Length; i++)
            {
                Console.WriteLine($"{"["+foodMenu[i].itemNum+"]", -5} {foodMenu[i].foodName, -15} PHP {foodMenu[i].price}");
            }
            Console.WriteLine("[v] view order");
            Console.WriteLine();

            Console.Write("Enter Food ID: ");
            string? itemId = Console.ReadLine();
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
            }else if (itemId.ToLower() == "v")
            {
               RestoMeth.IterOrders(orders);
               Console.WriteLine();
               Console.WriteLine("[y] to proceed \n[add] to add more items \n[rem] to remove items");
               Console.Write("Proceed to Checkout? ");
               string res = Console.ReadLine();
                if (res.ToLower() == "rem")
                {
                    RestoMeth.IterOrders(orders);
                    Console.WriteLine();

                    Console.Write("Select the food ID: ");
                    int id = int.Parse(Console.ReadLine());

                    Console.Write("Enter Quantity: ");
                    int qnt = int.Parse(Console.ReadLine());

                    int count = 0;

                    for (int x = 0; x < orders.Count;)
                    {
                        if (orders[x].itemNum == id)
                        {
                            orders.Remove(orders[x]);
                            count += 1;
                            if (count == qnt)
                            {
                                break;
                            }
                        }
                        else
                        {
                            x++;
                        }
                    }
                }
            }
            else if (!int.TryParse(itemId, out sel))
            {
                Console.WriteLine("Invalid Selection");
            }
            
        }while(running);
    }

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
                    Customer();
                    break;
            }

        } while (isRunning);
    }
}