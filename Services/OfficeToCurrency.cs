using AssetTracker.Models;

namespace AssetTracker.Services
{
    public static class OfficeToCurrency // Helper function to map office location to the right currency
    {
        public static bool IsValidOffice(string office) // Validate user input
        {
            return office.ToLower() switch
            {
                "london" => true,
                "tokyo" => true,
                "malmö" => true,
                "berlin" => true,
                "boston" => true,
                "toronto" => true,
                _ => false
            };
        }

        public static Currency GetCurrency(string office)
        {
            return office.ToLower() switch
            {
                "london" => Currency.GBP,
                "tokyo" => Currency.JPY,
                "malmö" => Currency.SEK,
                "berlin" => Currency.EUR,
                "boston" => Currency.USD,
                "toronto" => Currency.CAD,
                _ => Currency.USD

            };
        }
    }
}

