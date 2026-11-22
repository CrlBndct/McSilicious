
namespace MyLibrary;

public class Customer
{
    public static void Meth()
    {
        int[] cartQuantities = new int[Food.FoodNames.Length];
        bool isOrdering = true;

        while (isOrdering)
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("        CUSTOMER VIEW");
            Console.WriteLine("==============================");
            Console.WriteLine();

            DisplayMenu();

            Console.WriteLine();
            Console.WriteLine("[V] View Order");
            Console.WriteLine("[C] Cancel Order");
            Console.WriteLine("[0] Return to Main Menu");
            Console.WriteLine();

            Console.Write("Enter Food ID: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be blank.");
                Pause();
                continue;
            }

            if (input.Equals("v", StringComparison.OrdinalIgnoreCase))
            {
                ReviewOrder(cartQuantities);
            }
            else if (input.Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                Array.Clear(cartQuantities);

                Console.WriteLine("Order cancelled and cart cleared.");
                Pause();
            }
            else if (input == "0")
            {
                isOrdering = false;
            }
            else if (int.TryParse(input, out int itemId))
            {
                AddItem(cartQuantities, itemId);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
                Pause();
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine($"{"ID",-5} {"Food Name",-20} {"Price",-12}");
        Console.WriteLine("----------------------------------------");

        for (int i = 0; i < Food.FoodNames.Length; i++)
        {
            Console.WriteLine(
                $"{Food.ItemNumbers[i],-5} " +
                $"{Food.FoodNames[i],-20} " +
                $"PHP {Food.Prices[i]:F2}");
        }
    }

    static void AddItem(int[] cartQuantities, int itemId)
    {
        int index = itemId - 1;

        if (index < 0 || index >= Food.FoodNames.Length)
        {
            Console.WriteLine("Invalid food ID.");
            Pause();
            return;
        }

        Console.Write("Enter quantity (1-99): ");
        string? quantityInput = Console.ReadLine();

        if (!int.TryParse(quantityInput, out int quantity))
        {
            Console.WriteLine("Invalid quantity.");
            Pause();
            return;
        }

        if (quantity < 1 || quantity > 99)
        {
            Console.WriteLine("Quantity must be between 1 and 99.");
            Pause();
            return;
        }

        if (cartQuantities[index] + quantity > 99)
        {
            Console.WriteLine("Combined quantity cannot exceed 99.");
            Pause();
            return;
        }

        cartQuantities[index] += quantity;

        Console.WriteLine();
        Console.WriteLine($"{Food.FoodNames[index]} added to your order.");
        Console.WriteLine($"Current quantity: {cartQuantities[index]}");

        Pause();
    }

    static void ReviewOrder(int[] cartQuantities)
    {
        bool hasItems = false;

        for (int i = 0; i < cartQuantities.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                hasItems = true;
                break;
            }
        }

        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("         ORDER REVIEW");
        Console.WriteLine("==============================");

        if (!hasItems)
        {
            Console.WriteLine("Your cart is empty.");
            Pause();
            return;
        }

        decimal total = RestoMeth.CalculateTotal(cartQuantities);

        Console.WriteLine();
        Console.WriteLine(
            $"{"Food",-20} {"Price",-12} {"Qty",-5} {"Subtotal",-12}");

        Console.WriteLine(
            "--------------------------------------------------------");

        for (int i = 0; i < cartQuantities.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                decimal subtotal =
                    Food.Prices[i] * cartQuantities[i];

                Console.WriteLine(
                    $"{Food.FoodNames[i],-20} " +
                    $"PHP {Food.Prices[i],-8:F2} " +
                    $"{cartQuantities[i],-5} " +
                    $"PHP {subtotal,-10:F2}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Order Total: PHP {total:F2}");
        Console.WriteLine();

        Console.WriteLine("[A] Add More Items");
        Console.WriteLine("[R] Remove Items");
        Console.WriteLine("[Y] Proceed to Checkout");
        Console.WriteLine("[C] Cancel Order");
        Console.WriteLine("[0] Return");
        Console.WriteLine();

        Console.Write("Enter choice: ");
        string? choice = Console.ReadLine()?.Trim().ToLower();

        switch (choice)
        {
            case "a":
               
                break;

            case "r":
                RemoveItem(cartQuantities);
                break;

            case "y":
                if (RestoMeth.Checkout(cartQuantities))
                {
                    Array.Clear(cartQuantities);
                }
                break;

            case "c":
                Array.Clear(cartQuantities);

                Console.WriteLine("Order cancelled and cart cleared.");
                Pause();
                break;

            case "0":
                // Return to customer menu
                break;

            default:
                Console.WriteLine("Invalid choice.");
                Pause();
                break;
        }
    }

    static void RemoveItem(int[] cartQuantities)
    {
        Console.WriteLine();
        Console.Write("Enter food ID to remove: ");

        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int itemId))
        {
            Console.WriteLine("Invalid food ID.");
            Pause();
            return;
        }

        int index = itemId - 1;

        if (index < 0 || index >= Food.FoodNames.Length)
        {
            Console.WriteLine("Invalid food ID.");
            Pause();
            return;
        }

        if (cartQuantities[index] == 0)
        {
            Console.WriteLine("That item is not in your cart.");
            Pause();
            return;
        }

        Console.Write("Enter quantity to remove: ");
        string? quantityInput = Console.ReadLine();

        if (!int.TryParse(quantityInput, out int quantity))
        {
            Console.WriteLine("Invalid quantity.");
            Pause();
            return;
        }

        if (quantity <= 0 || quantity > cartQuantities[index])
        {
            Console.WriteLine("Invalid removal quantity.");
            Pause();
            return;
        }

        cartQuantities[index] -= quantity;

        Console.WriteLine("Item quantity updated.");
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
