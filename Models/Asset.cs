
using System.Drawing;
using System.Dynamic;

namespace AssetTracker.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public Price PriceUSD { get; set; }
        public Price PriceLocal { get; set; }
        public DateTime PurchaseDate { get; set; }

        // Parameterless constructor for EF Core
        public Asset()
        {
            Type = string.Empty;
            Brand = string.Empty;
            Model = string.Empty;
            Office = string.Empty;
            PriceUSD = new Price();
            PriceLocal = new Price();
            PurchaseDate = DateTime.MinValue;
        }

        public Asset(string type, string brand, string model, string office, Price priceUSD, Price priceLocal, DateTime purchaseDate)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            PriceUSD = priceUSD;
            PriceLocal = priceLocal;
            PurchaseDate = purchaseDate;
        }


        public override string ToString()
        {
            // try removing this and see what happens
            return $"{Type}\t{Brand}\t{Model}\t{PriceUSD.Amount} {PriceUSD.Currency.Symbol}\t{PurchaseDate.ToShortDateString()}";
        }
    }
}

