using System;
using System.Globalization;
using Risk_System_Test_Thiago_Carvalho_Exercicio_2.Interfaces;
using Risk_System_Test_Thiago_Carvalho_Exercicio_2.Models;

class Program
{
    static void Main()
    {
        string continueProcessing;

        do
        {
            Console.WriteLine("Enter the reference date (format MM/dd/yyyy):");
            DateTime referenceDate = ParseValidDate(Console.ReadLine());

            int n;
            bool isValidNumber = false;

            do
            {
                Console.WriteLine("Enter the number of trades:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out n) && n > 0)
                {
                    isValidNumber = true; 
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive integer.");
                }
            } while (!isValidNumber);

            for (int i = 0; i < n; i++)
            {
                bool validInput = false;

                while (!validInput)
                {
                    Console.WriteLine($"Enter trade {i + 1} in the format: value sector date politicallyExposed (e.g., Value(USD) Sector Date(format MM/dd/yyyy) and True or False for politically exposed)");
                    string inputLine = Console.ReadLine().Trim();

                    string[] tradeData = inputLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tradeData.Length != 4)
                    {
                        Console.WriteLine("Error: You must provide exactly four parameters: value, sector, date, and politically exposed status.");
                        continue; 
                    }
                    else
                    {
                        validInput = true; 

                        Console.WriteLine($"Received value: {tradeData[0]}, sector: {tradeData[1]}, date: {tradeData[2]}, politically exposed: {tradeData[3]}");

                        double value = ParseValidValue(tradeData[0]);

                        string sector = ValidateSector(tradeData[1]);

                        DateTime nextPaymentDate = ParseValidDate(tradeData[2]);

                        bool isPoliticallyExposed = bool.Parse(tradeData[3]);

                        ITrade trade = new Trade(value, sector, nextPaymentDate, isPoliticallyExposed);

                        string category = CategorizeTrade(trade, referenceDate);

                        Console.WriteLine(category);
                    }
                }
            }

            Console.WriteLine("Do you want to process more transactions? (yes/no)");
            continueProcessing = Console.ReadLine().ToLower();

        } while (continueProcessing == "yes");

        Console.WriteLine("Exiting the program...");
    }

    static string CategorizeTrade(ITrade trade, DateTime referenceDate)
    {
        if (trade.IsPoliticallyExposed)
        {
            return "PEP";
        }

        // Check if the value is below 1,000,000
        if (trade.Value < 1_000_000)
        {
            return "The trade value is below the calculated risk thresholds.";
        }

        // EXPIRED
        if (trade.NextPaymentDate < referenceDate.AddDays(-30))
        {
            return "EXPIRED";
        }

        // HIGHRISK
        if (trade.Value > 1_000_000 && trade.ClientSector.ToUpper() == "PRIVATE")
        {
            return "HIGHRISK";
        }

        // MEDIUMRISK
        if (trade.Value > 1_000_000 && trade.ClientSector.ToUpper() == "PUBLIC")
        {
            return "MEDIUMRISK";
        }

        return "NO CATEGORY";
    }

    static double ParseValidValue(string input)
    {
        double value;
        input = input.Replace(',', '.');

        if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value) && value > 0)
        {
            return value;
        }
        else
        {
            Console.WriteLine("Invalid value. Please enter a valid positive dollar amount.");
            return ParseValidValue(Console.ReadLine());
        }
    }

    static DateTime ParseValidDate(string input)
    {
        DateTime date;

        if (DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            Console.WriteLine("Invalid date. Please use the format MM/dd/yyyy.");
            return ParseValidDate(Console.ReadLine());
        }
    }

    static string ValidateSector(string sector)
    {
        sector = sector.Trim(); 
        if (sector.Equals("Private", StringComparison.OrdinalIgnoreCase) || sector.Equals("Public", StringComparison.OrdinalIgnoreCase))
        {
            return sector;
        }
        else
        {
            Console.WriteLine("This sector does not exist. Please try again with either 'Private' or 'Public'.");
            return ValidateSector(Console.ReadLine());
        }
    }
}