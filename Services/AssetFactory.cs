using AssetTracker.Models;
using AssetTracker.Helpers;


namespace AssetTracker.Services
{

    public static class AssetFactory
    {
        // Helper method to simplify adding new assets
        public static Asset Create(string type, string brand, string model, string office, decimal priceUsdAmount, DateTime purchaseDate, Dictionary<string, decimal> rates)
        {
            var localCurrency = OfficeToCurrency.GetCurrency(office);
            var localAmount = CurrencyConverter.Convert(priceUsdAmount, localCurrency.Code, rates);
            var priceUsd = new Price(priceUsdAmount, Currency.USD);
            var priceLocal = new Price(localAmount, localCurrency);

            return new Asset(type, brand, model, office, priceUsd, priceLocal, purchaseDate);
        }
    }
}

