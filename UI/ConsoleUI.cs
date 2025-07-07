using System.Reflection;
using AssetTracker.Models;
using AssetTracker.Services;
using AssetTracker.Helpers;
using System.Globalization;
using System.Drawing;
using AssetTracker.Infrastructure;
using System.Linq;
using System;
using System.Threading;



namespace AssetTracker.UI
{
    public class ConsoleUI
    {
        private readonly AssetManager _assetManager;
        private readonly Dictionary<string, decimal> _rates;

        public ConsoleUI(AssetManager assetManager, Dictionary<string, decimal> rates)
        {
            _assetManager = assetManager;
            _rates = rates;
        }

        public void Run()
        {

            while (true)
            {
                ShowAssets(); // Start by showing the assets (sample data at start of the program)
                Console.WriteLine("Welcome to AssetTracker! What would you like to do? Enter a number:");
                Console.WriteLine("1) Add an asset");
                Console.WriteLine("2) Update an asset");
                Console.WriteLine("3) Delete an asset");
                Console.WriteLine("4) Exit the program");


                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        UserAddAsset(_rates);
                        break;

                    case "2":
                        UserUpdateAsset();
                        break;
                    
                    case "3":
                        UserDeleteAsset();
                        break;

                    case "4":
                        Console.WriteLine("Exiting...");
                        return;
                        
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            }
        }

        public string PromptOffice() // Helper method to keep asking for an office location until it is valid
        {
            while (true)
            {
                Console.WriteLine("Enter an office location (London/Tokyo/Berlin/Malmö/Boston): ");
                string input = Console.ReadLine() ?? "";
                if (OfficeToCurrency.IsValidOffice(input))
                {
                    return input;
                }
                Console.WriteLine("Invalid office location. Please try again.");
            }
        }

        public decimal PromptPrice() // Helper method to keep asking for a valid price
        {
            while (true)
            {
                Console.WriteLine("Enter a price in USD (use comma for decimals): ");
                string input = Console.ReadLine() ?? "";
                if (decimal.TryParse(input, out var price) && price > 0)
                {
                    return price;
                }
                Console.WriteLine("Invalid input, please enter a positive number.");
            }
        }

        public DateTime PromptDate() // Helper method to keep asking for a valid date
        {
            while (true)
            {
                Console.WriteLine("Enter a purchase date (DD/MM/YYYY): ");
                string input = Console.ReadLine() ?? "";
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                else
                {
                    Console.Write("Invalid date format. Please use DD/MM/YYYY (e.g. 30/06/2025): ");
                }
            }
        }

        public int ValidateAssetId()
        {
            var assets = _assetManager.GetAllAssets();
            var validIds = assets.Select(a => a.Id).ToList();

            while (true)
            {
                string inputId = Console.ReadLine();

                if (int.TryParse(inputId, out int inputIndex) && validIds.Contains(inputIndex))
                {
                    return inputIndex;
                }

                else
                {
                    Console.WriteLine($"Invalid choice, please enter an ID of an asset present in the list");
                }
            }
        }


        public void UserAddAsset(Dictionary<string, decimal> rates)
        {
            Console.WriteLine("Enter an asset type (e.g. 'Smartphone'): ");
            string inputType = Console.ReadLine() ?? "";
            Console.WriteLine("Enter a brand (e.g. 'Apple'): ");
            string inputBrand = Console.ReadLine() ?? "";
            Console.WriteLine("Enter a model (e.g. 'Iphone 14'): ");
            string inputModel = Console.ReadLine() ?? "";
            decimal inputPriceUSD = PromptPrice();
            string inputOffice = PromptOffice();
            DateTime inputDate = PromptDate();

            Currency targetCurrency = OfficeToCurrency.GetCurrency(inputOffice); // Get target currency from office
            decimal convertedAmount = CurrencyConverter.Convert(inputPriceUSD, targetCurrency.Code, rates); // Convert USD to target currency

            Price priceLocal = new Price(convertedAmount, targetCurrency);
            Price priceUSD = new Price(inputPriceUSD, Currency.USD);

            Asset asset = AssetFactory.Create(inputType, inputBrand, inputModel, inputOffice, inputPriceUSD, inputDate, rates); // Use the helper method to add an asset in a simpler way
            _assetManager.AddAsset(asset);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAsset successfully added!");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        public void UserUpdateAsset()
        {
            ShowAssetsId();
            Console.WriteLine("Which asset would you like to update?");

            int assetId = ValidateAssetId();

            var asset = _assetManager.GetAllAssets().FirstOrDefault(a => a.Id == assetId);

            Console.WriteLine($"Current type: {asset.Type}. Enter a new type or press Enter to keep:");
            string newType = Console.ReadLine();

            Console.WriteLine($"Current brand: {asset.Brand}. Enter a new brand or press Enter to keep:");
            string newBrand = Console.ReadLine();

            Console.WriteLine($"Current model: {asset.Model}. Enter a new model or press Enter to keep:");
            string newModel = Console.ReadLine();

            Console.WriteLine($"Current office: {asset.Office}. Enter a new office or press Enter to keep:");
            string newOffice = "";

            while (true)
            {
                string officeInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(officeInput))
                {
                    // User pressed Enter, keep current office
                    newOffice = "";
                    break;
                }
                else if (OfficeToCurrency.IsValidOffice(officeInput))
                {
                    newOffice = officeInput;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid office location. Please enter a valid office or press Enter to keep the current one:");
                }
            }

            Console.WriteLine($"Current USD price: {asset.PriceUSD.Amount}. Enter a new price or press Enter to keep:");
            string priceInput = Console.ReadLine();

            Console.WriteLine($"Current purchase date: {asset.PurchaseDate:dd/MM/yyyy}. Enter a new date (DD/MM/YYYY) or press Enter to keep:");
            string dateInput = Console.ReadLine();

            decimal? newPrice = null;
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out var parsedPrice))
                newPrice = parsedPrice;

            DateTime? newDate = null;
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParseExact(dateInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                newDate = parsedDate;

            _assetManager.UpdateAsset(assetId, newType, newBrand, newModel, newOffice, newPrice, newDate, _rates);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAsset successfully updated!");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        public void UserDeleteAsset()
        {
            ShowAssetsId();
            Console.WriteLine("Which asset would you like to delete?");
            int inputId = ValidateAssetId(); //check if input is a number and is on the list
            _assetManager.DeleteAsset(inputId);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAsset successfully removed!");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        public void ShowAssetsId()
        {
            var assets = _assetManager.GetAllAssets();

            Console.WriteLine("{0,-6} {1,-12} {2,-12} {3,-15} {4,-12}", "Id", "Type", "Brand", "Model", "Office");
            Console.WriteLine(new string('-', 65));

            foreach (var asset in assets)
            {
                Console.WriteLine("{0,-6} {1,-12} {2,-12} {3,-15} {4,-12}",
                    asset.Id, asset.Type, asset.Brand, asset.Model, asset.Office);
            }
        }

        public void ShowAssets()
        {
            Console.Clear();
            Console.WriteLine("Here are all the current assets, sorted by Office and Date:");
            Console.WriteLine();
            var assets = _assetManager.GetAssetsSorted();

            // print header
            Console.WriteLine("{0,-10} | {1,-10} | {2,-15} | {3,-10} | {4,10} | {5,12} | {6,-14}",
            "Type", "Brand", "Model", "Office", "USD Price", "Local Price", "Purchase Date");
            Console.WriteLine(new string('-', 100));

            foreach (var asset in assets)
            {
                (string assetColor, bool isExpired) = GetExpirationDate.CheckDate(asset.PurchaseDate); // Check expiration date of asset and which color to print in

                // Add emoji if expired
                string purchaseDateDisplay = asset.PurchaseDate.ToString("yyyy-MM-dd");
                if (isExpired)
                {
                    purchaseDateDisplay += " ❌"; // Expired symbol
                }

                // Build the output string
                string output = string.Format(
                "{0,-10} | {1,-10} | {2,-15} | {3,-10} | {4,10:C} | {5,12:C} | {6,-14}",
                asset.Type,
                asset.Brand,
                asset.Model,
                asset.Office,
                asset.PriceUSD?.ToString() ?? "N/A", // this line is for some reason needed to keep the program from crashing
                asset.PriceLocal?.ToString() ?? "N/A",
                purchaseDateDisplay
            );

                // Set console color
                if (isExpired || assetColor == "red")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (assetColor == "yellow")
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.WriteLine(output);
                Console.WriteLine();
                Console.ResetColor();
            }

            // add logic to show total, number of types etc
        }
    }
}

