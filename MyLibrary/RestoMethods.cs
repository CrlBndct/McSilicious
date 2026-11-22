using System.IO;
using System.Security;
namespace MyLibrary;

class RestoMeth
{
    public static void IterOrders(List<Food> ord)
    {
        float priceSum = 0;
        Console.WriteLine($"{"Food ID",-10} {"Product",-25} {"Quantity",-10} {"Total",-10}");
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
            Console.WriteLine($"{foods.itemNum,-10} {foods.foodName,-25} {"x" + qnty,-10} {"PHP" + sum,-10}");
        }
        Console.WriteLine();
        Console.WriteLine($"Total: PHP {priceSum}");
    }

    public static void createReceipt(List<Food> ord)
    {
        string folderPath = "C:\\Users\\Benedict Dagle\\OneDrive\\Documents\\COMPROG\\CSHARP\\CollegeProjects\\McSilicious\\Receipts";
        int randomNum = Random.Shared.Next(1, 99);
        string newfile = (randomNum > 9) ? $"{randomNum}.txt" : $"0{randomNum}.txt";
        string fullPath = Path.Combine(folderPath, newfile);

        void checkFile(string path, int num)
        {
            if (!File.Exists(fullPath))
            {
                using (StreamWriter write = new StreamWriter(fullPath))
                {
                    write.WriteLine($"======== Receipt No. {num}========");
                    write.WriteLine();
                    write.WriteLine($"{"Food",-25} {"Price",-10}");
                    foreach (Food o in ord)
                    {
                        write.WriteLine($"{o.foodName,-25} {"PHP" + o.price,-10}");
                    }

                    float sum = 0;
                    for (int x = 0; x < ord.Count; x++)
                    {
                        sum += ord[x].price;
                    }
                    write.WriteLine($"{ "Total:",-25} {"PHP" + sum,-10}");
                }
            }
            else
            {
                randomNum = Random.Shared.Next(1, 99);
                newfile = (randomNum > 9) ? $"{randomNum}.txt" : $"0{randomNum}.txt";
                fullPath = Path.Combine(folderPath, newfile);

                checkFile(fullPath, randomNum);
            }
        }

        if (!(ord.Count <= 0))
        {
            checkFile(fullPath, randomNum);
        }
    }
}