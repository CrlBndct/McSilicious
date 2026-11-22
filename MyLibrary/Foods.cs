
namespace MyLibrary;

public class Food
{
    public static int[] ItemNumbers = { 1, 2, 3, 4, 5 };
    public static string[] FoodNames =
    {
        "Chicken Meal",
        "Burger",
        "Fries",
        "Soft Drink",
        "Sundae"
    };

    public static decimal[] Prices =
    {
        120.00m,
        60.00m,
        45.00m,
        30.00m,
        35.00m
    };

    
    public int itemNum;
    public string foodName;
    public decimal price;

    public Food(int itemNum, string foodName, decimal price)
    {
        this.itemNum = itemNum;
        this.foodName = foodName;
        this.price = price;
    }
}
