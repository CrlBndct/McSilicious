namespace MyLibrary;

public class Customer
{
    static List<Food> orders = new List<Food>();
    public static void Meth()
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
                    Console.Write("Quantity: ");
                    string quantityStr = Console.ReadLine();
                    int quantity;
                    if (int.TryParse(quantityStr, out quantity))
                    {
                        if (quantity < 100 && (orders.Count + quantity) < 100)
                        {
                            int q = 0;

                            while (q < quantity)
                            {
                                orders.Add(foodMenu[sel - 1]);
                                q++;
                            }
                            Console.WriteLine(orders.Count);
                        }
                        else if(quantity >= 100)
                        {
                            Console.WriteLine("Quantity exceeds maximum");
                        }
                        else
                        {
                            Console.WriteLine("Invalid");
                        }
                    }
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
               Console.WriteLine("[y] to proceed \n[add] to add more items \n[rem] to remove items \n[cancel] Cancel Order");
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
                    if (!(qnt <= 0))
                    {
                        int count = 0;

                        for (int x = orders.Count - 1; x >= 0 ; x-- )
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
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Number");
                    }  
                }else if (res.ToLower() == "cancel")
                {
                    orders.Clear();
                }else if (res.ToLower() == "add")
                {
                    running = true;
                }else if (res.ToLower() == "y")
                {
                    running = false;
                    RestoMeth.createReceipt(orders);
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            else if (!int.TryParse(itemId, out sel))
            {
                Console.WriteLine("Invalid Selection");
            }
            
        }while(running);
    }
}
