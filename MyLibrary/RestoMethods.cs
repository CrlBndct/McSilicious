
namespace MyLibrary;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public class RestoMeth
{
    private static readonly string RecordFolder = "records";
    private static readonly string RecordFile = Path.Combine(RecordFolder, "orders.txt");

    private static readonly string ReceiptFolder =
        @"C:\Users\Benedict Dagle\OneDrive\Documents\COMPROG\CSHARP\CollegeProjects\McSilicious\Receipts";

    public static decimal CalculateTotal(int[] cartQuantities)
    {
        decimal total = 0;

        for (int i = 0; i < cartQuantities.Length; i++)
        {
            total += Food.Prices[i] * cartQuantities[i];
        }

        return total;
    }

    public static bool Checkout(int[] cartQuantities)
    {
        decimal total = CalculateTotal(cartQuantities);

        if (total <= 0)
        {
            Console.WriteLine("Your cart is empty.");
            Pause();
            return false;
        }

        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("           PAYMENT");
        Console.WriteLine("==============================");
        Console.WriteLine($"Order Total: PHP {total:F2}");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter cash amount or C to cancel: ");
            string? input = Console.ReadLine()?.Trim();

            if (input == null)
                continue;

            if (input.Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Payment cancelled.");
                Pause();
                return false;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Cash amount cannot be blank.");
                continue;
            }

            if (!decimal.TryParse(
                input,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out decimal cash))
            {
                Console.WriteLine("Invalid cash amount.");
                continue;
            }

            if (cash < 0)
            {
                Console.WriteLine("Cash cannot be negative.");
                continue;
            }

            if (decimal.Round(cash, 2) != cash)
            {
                Console.WriteLine("Cash must have at most two decimal places.");
                continue;
            }

            if (cash < total)
            {
                decimal needed = total - cash;

                Console.WriteLine("Insufficient cash.");
                Console.WriteLine($"PHP {needed:F2} still needed.");
                Console.WriteLine("Enter the full payment amount again.");
                Console.WriteLine("Or enter C to cancel.");
                Console.WriteLine();

                continue;
            }

            decimal change = cash - total;
            string orderId = GenerateOrderId();

            if (!SaveOrder(orderId, cartQuantities, total, cash, change))
            {
                Console.WriteLine();
                Console.WriteLine("The transaction could not be saved.");
                Console.WriteLine("The sale was not completed.");
                Pause();
                return false;
            }

            bool receiptFileSaved = SaveReceiptFile(
                orderId,
                cartQuantities,
                total,
                cash,
                change
            );

            DisplayReceipt(orderId, cartQuantities, total, cash, change);

            if (!receiptFileSaved)
            {
                Console.WriteLine();
                Console.WriteLine("Warning: The transaction was saved,");
                Console.WriteLine("but the TXT receipt could not be created.");
                Pause();
            }

            return true;
        }
    }

    public static string GenerateOrderId()
    {
        int highestId = 0;

        List<OrderRecord> records = ReadOrders();

        foreach (OrderRecord record in records)
        {
            if (record.OrderId.StartsWith("ORD", StringComparison.OrdinalIgnoreCase))
            {
                string numberPart = record.OrderId.Substring(3);

                if (int.TryParse(numberPart, out int number) &&
                    number > highestId)
                {
                    highestId = number;
                }
            }
        }

        return $"ORD{highestId + 1:D4}";
    }

    public static bool SaveOrder(
        string orderId,
        int[] cartQuantities,
        decimal total,
        decimal cash,
        decimal change)
    {
        try
        {
            Directory.CreateDirectory(RecordFolder);

            List<string> lines = new List<string>();

            lines.Add(
                $"ORDER|{orderId}|PAID|" +
                $"{total.ToString("F2", CultureInfo.InvariantCulture)}|" +
                $"{cash.ToString("F2", CultureInfo.InvariantCulture)}|" +
                $"{change.ToString("F2", CultureInfo.InvariantCulture)}"
            );

            for (int i = 0; i < cartQuantities.Length; i++)
            {
                if (cartQuantities[i] > 0)
                {
                    decimal subtotal = Food.Prices[i] * cartQuantities[i];

                    lines.Add(
                        $"ITEM|{Food.FoodNames[i]}|" +
                        $"{Food.Prices[i].ToString("F2", CultureInfo.InvariantCulture)}|" +
                        $"{cartQuantities[i]}|" +
                        $"{subtotal.ToString("F2", CultureInfo.InvariantCulture)}"
                    );
                }
            }

            lines.Add("END");
            lines.Add("");

            File.AppendAllLines(RecordFile, lines);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Save error: {ex.Message}");
            return false;
        }
    }

    public static bool SaveReceiptFile(
        string orderId,
        int[] cartQuantities,
        decimal total,
        decimal cash,
        decimal change)
    {
        try
        {
            Directory.CreateDirectory(ReceiptFolder);

            string receiptPath = Path.Combine(
                ReceiptFolder,
                $"{orderId}.txt"
            );

            string receiptContent = GenerateReceiptText(
                orderId,
                cartQuantities,
                total,
                cash,
                change
            );

            File.WriteAllText(receiptPath, receiptContent);

            Console.WriteLine();
            Console.WriteLine("TXT receipt saved to:");
            Console.WriteLine(receiptPath);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Receipt file error: {ex.Message}");
            return false;
        }
    }

    public static string GenerateReceiptText(
        string orderId,
        int[] cartQuantities,
        decimal total,
        decimal cash,
        decimal change)
    {
        StringBuilder receipt = new StringBuilder();

        receipt.AppendLine("==============================");
        receipt.AppendLine("        McSilicious");
        receipt.AppendLine("          RECEIPT");
        receipt.AppendLine("==============================");
        receipt.AppendLine($"Order ID: {orderId}");
        receipt.AppendLine("Status: PAID");
        receipt.AppendLine();

        receipt.AppendLine(
            $"{"Food",-20} {"Price",-10} {"Qty",-5} {"Subtotal",-12}"
        );

        receipt.AppendLine(
            "--------------------------------------------------------"
        );

        for (int i = 0; i < cartQuantities.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                decimal subtotal = Food.Prices[i] * cartQuantities[i];

                receipt.AppendLine(
                    $"{Food.FoodNames[i],-20} " +
                    $"PHP {Food.Prices[i],-6:F2} " +
                    $"{cartQuantities[i],-5} " +
                    $"PHP {subtotal,-10:F2}"
                );
            }
        }

        receipt.AppendLine();
        receipt.AppendLine($"Total: PHP {total:F2}");
        receipt.AppendLine($"Cash: PHP {cash:F2}");
        receipt.AppendLine($"Change: PHP {change:F2}");
        receipt.AppendLine("==============================");
        receipt.AppendLine("     Thank you for ordering!");
        receipt.AppendLine("==============================");

        return receipt.ToString();
    }

    public static List<OrderRecord> ReadOrders()
    {
        List<OrderRecord> orders = new List<OrderRecord>();

        try
        {
            if (!File.Exists(RecordFile))
                return orders;

            string[] lines = File.ReadAllLines(RecordFile);

            OrderRecord? currentOrder = null;

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');

                if (parts.Length == 6 && parts[0] == "ORDER")
                {
                    currentOrder = new OrderRecord
                    {
                        OrderId = parts[1],
                        Status = parts[2],
                        Total = decimal.Parse(
                            parts[3],
                            CultureInfo.InvariantCulture
                        ),
                        Cash = decimal.Parse(
                            parts[4],
                            CultureInfo.InvariantCulture
                        ),
                        Change = decimal.Parse(
                            parts[5],
                            CultureInfo.InvariantCulture
                        )
                    };
                }
                else if (parts.Length == 5 &&
                         parts[0] == "ITEM" &&
                         currentOrder != null)
                {
                    currentOrder.Items.Add(new OrderItem
                    {
                        FoodName = parts[1],
                        UnitPrice = decimal.Parse(
                            parts[2],
                            CultureInfo.InvariantCulture
                        ),
                        Quantity = int.Parse(parts[3]),
                        Subtotal = decimal.Parse(
                            parts[4],
                            CultureInfo.InvariantCulture
                        )
                    });
                }
                else if (line == "END" && currentOrder != null)
                {
                    orders.Add(currentOrder);
                    currentOrder = null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Record reading error: {ex.Message}");
        }

        return orders;
    }

    public static void FindOrder()
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("        ORDER LOOKUP");
        Console.WriteLine("==============================");
        Console.WriteLine();

        Console.Write("Enter Order ID: ");
        string? input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Order ID cannot be blank.");
            Pause();
            return;
        }

        List<OrderRecord> orders = ReadOrders();

        OrderRecord? found = orders.FirstOrDefault(
            o => o.OrderId.Equals(
                input,
                StringComparison.OrdinalIgnoreCase
            ) && o.Status == "PAID"
        );

        if (found == null)
        {
            Console.WriteLine("Order not found.");
            Pause();
            return;
        }

        DisplaySavedReceipt(found);
    }

    public static void ShowSales()
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("         SALES REPORT");
        Console.WriteLine("==============================");
        Console.WriteLine();

        List<OrderRecord> orders = ReadOrders()
            .Where(o => o.Status == "PAID")
            .ToList();

        if (orders.Count == 0)
        {
            Console.WriteLine("Completed Orders: 0");
            Console.WriteLine("Total Sales: PHP 0.00");
            Console.WriteLine();
            Console.WriteLine("No sales yet.");

            Pause();
            return;
        }

        decimal totalSales = 0;

        foreach (OrderRecord order in orders)
        {
            totalSales += order.Total;
        }

        Console.WriteLine($"Completed Orders: {orders.Count}");
        Console.WriteLine($"Total Sales: PHP {totalSales:F2}");

        Pause();
    }

    public static void DeleteOrder()
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("         DELETE ORDER");
        Console.WriteLine("==============================");
        Console.WriteLine();

        Console.Write("Enter Order ID to delete: ");
        string? input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Order ID cannot be blank.");
            Pause();
            return;
        }

        List<OrderRecord> orders = ReadOrders();

        OrderRecord? found = orders.FirstOrDefault(
            o => o.OrderId.Equals(
                input,
                StringComparison.OrdinalIgnoreCase
            ) && o.Status == "PAID"
        );

        if (found == null)
        {
            Console.WriteLine("Order not found.");
            Pause();
            return;
        }

        DisplaySavedReceipt(found);

        Console.WriteLine();
        Console.WriteLine("Are you sure you want to delete this order?");
        Console.WriteLine("[Y] Yes");
        Console.WriteLine("[N] No");
        Console.WriteLine();

        Console.Write("Enter choice: ");
        string? confirmation = Console.ReadLine()?.Trim();

        if (!string.Equals(
            confirmation,
            "Y",
            StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Deletion cancelled.");
            Pause();
            return;
        }

        try
        {
            orders.Remove(found);

            List<string> lines = new List<string>();

            foreach (OrderRecord order in orders)
            {
                lines.Add(
                    $"ORDER|{order.OrderId}|{order.Status}|" +
                    $"{order.Total.ToString("F2", CultureInfo.InvariantCulture)}|" +
                    $"{order.Cash.ToString("F2", CultureInfo.InvariantCulture)}|" +
                    $"{order.Change.ToString("F2", CultureInfo.InvariantCulture)}"
                );

                foreach (OrderItem item in order.Items)
                {
                    lines.Add(
                        $"ITEM|{item.FoodName}|" +
                        $"{item.UnitPrice.ToString("F2", CultureInfo.InvariantCulture)}|" +
                        $"{item.Quantity}|" +
                        $"{item.Subtotal.ToString("F2", CultureInfo.InvariantCulture)}"
                    );
                }

                lines.Add("END");
                lines.Add("");
            }

            Directory.CreateDirectory(RecordFolder);

            File.WriteAllLines(RecordFile, lines);

            string receiptPath = Path.Combine(
                ReceiptFolder,
                $"{found.OrderId}.txt"
            );

            if (File.Exists(receiptPath))
            {
                File.Delete(receiptPath);
            }

            Console.WriteLine();
            Console.WriteLine($"Order {found.OrderId} deleted successfully.");
            Console.WriteLine("The order record and receipt were removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("An error occurred while deleting the order.");
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    public static void DisplayReceipt(
        string orderId,
        int[] cartQuantities,
        decimal total,
        decimal cash,
        decimal change)
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("        McSilicious");
        Console.WriteLine("          RECEIPT");
        Console.WriteLine("==============================");
        Console.WriteLine($"Order ID: {orderId}");
        Console.WriteLine("Status: PAID");
        Console.WriteLine();

        DisplayReceiptItems(cartQuantities);

        Console.WriteLine();
        Console.WriteLine($"Total: PHP {total:F2}");
        Console.WriteLine($"Cash: PHP {cash:F2}");
        Console.WriteLine($"Change: PHP {change:F2}");
        Console.WriteLine("==============================");
        Console.WriteLine("     Thank you for ordering!");
        Console.WriteLine("==============================");

        Pause();
    }

    public static void DisplaySavedReceipt(OrderRecord order)
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("        McSilicious");
        Console.WriteLine("          RECEIPT");
        Console.WriteLine("==============================");
        Console.WriteLine($"Order ID: {order.OrderId}");
        Console.WriteLine($"Status: {order.Status}");
        Console.WriteLine();

        Console.WriteLine(
            $"{"Food",-20} {"Price",-10} {"Qty",-5} {"Subtotal",-12}"
        );

        foreach (OrderItem item in order.Items)
        {
            Console.WriteLine(
                $"{item.FoodName,-20} " +
                $"PHP {item.UnitPrice,-6:F2} " +
                $"{item.Quantity,-5} " +
                $"PHP {item.Subtotal,-10:F2}"
            );
        }

        Console.WriteLine();
        Console.WriteLine($"Total: PHP {order.Total:F2}");
        Console.WriteLine($"Cash: PHP {order.Cash:F2}");
        Console.WriteLine($"Change: PHP {order.Change:F2}");
        Console.WriteLine("==============================");

        Pause();
    }

    static void DisplayReceiptItems(int[] cartQuantities)
    {
        Console.WriteLine(
            $"{"Food",-20} {"Price",-10} {"Qty",-5} {"Subtotal",-12}"
        );

        for (int i = 0; i < cartQuantities.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                decimal subtotal = Food.Prices[i] * cartQuantities[i];

                Console.WriteLine(
                    $"{Food.FoodNames[i],-20} " +
                    $"PHP {Food.Prices[i],-6:F2} " +
                    $"{cartQuantities[i],-5} " +
                    $"PHP {subtotal,-10:F2}"
                );
            }
        }
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

public class OrderRecord
{
    public string OrderId = "";
    public string Status = "";
    public decimal Total;
    public decimal Cash;
    public decimal Change;

    public List<OrderItem> Items = new List<OrderItem>();
}

public class OrderItem
{
    public string FoodName = "";
    public decimal UnitPrice;
    public int Quantity;
    public decimal Subtotal;
}
