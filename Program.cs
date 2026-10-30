using System;

class Food
{
    int itemNum;
    string foodName;
    float price;


    public Food(int itemNum, string foodName, float price)
    {
        this.itemNum = itemNum;
        this.foodName = foodName;
        this.price = price;
    }
}

class Program
{ 

    static void Customer()
    {
        Food[] foodMenu = {new Food(1, "fries", 59), new Food(1, "Meal 1", 192), new Food(1, "Meal 2", 200) };
        
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
                case 1:
                    Customer();
                    break;
            }



        } while (isRunning);
    }
}

