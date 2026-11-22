namespace MyLibrary;

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

    public static void createReceipt()
    {
        
    }
}