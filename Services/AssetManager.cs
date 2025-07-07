using AssetTracker.Infrastructure;
using AssetTracker.Models;

namespace AssetTracker.Services
{
    public class AssetManager
    {
        private readonly AssetContext _context;

        public AssetManager(AssetContext context)
        {
            _context = context;
        }

        public List<Asset> GetAllAssets()
        {
            return _context.Assets.ToList();
        }

        public List<Asset> GetAssetsSorted()
        {
            return _context.Assets
                .OrderBy(a => a.Office)
                .ThenBy(a => a.PurchaseDate)
                .ToList();
    }

        public void AddAsset(Asset asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }

        public void DeleteAsset(int inputId)
        {
            Asset assetToDelete = _context.Assets.Find(inputId);
            _context.Assets.Remove(assetToDelete);
            _context.SaveChanges();
        }

        public void UpdateAsset(int assetId, string newType, string newBrand, string newModel, string newOffice, decimal? newPriceUSD, DateTime? newDate, Dictionary<string, decimal> rates)
        {
            var asset = _context.Assets.Find(assetId);

            bool officeChanged = false;
            bool priceChanged = false;

            if (!string.IsNullOrWhiteSpace(newType)) asset.Type = newType;
            if (!string.IsNullOrWhiteSpace(newBrand)) asset.Brand = newBrand;
            if (!string.IsNullOrWhiteSpace(newModel)) asset.Model = newModel;

            if (!string.IsNullOrWhiteSpace(newOffice))
            {
                asset.Office = newOffice;
                officeChanged = true;
            }

            if (newPriceUSD.HasValue)
            {
                asset.PriceUSD = new Price(newPriceUSD.Value, Currency.USD);
                priceChanged = true;
            }

            if (newDate.HasValue) asset.PurchaseDate = newDate.Value;

            // If price or office changed, recalculate local price
            if (officeChanged || priceChanged)
            {
                var localCurrency = OfficeToCurrency.GetCurrency(asset.Office);
                var localAmount = CurrencyConverter.Convert(asset.PriceUSD.Amount, localCurrency.Code, rates);
                asset.PriceLocal = new Price(localAmount, localCurrency);
            }

            _context.SaveChanges();
        }

    }
}