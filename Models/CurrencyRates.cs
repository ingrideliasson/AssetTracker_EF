using System.Text.Json.Serialization;

namespace AssetTracker.Models
{
    public class CurrencyRates // matches structure of the data retrieved from the API
    {
        public string BaseCode { get; set; } // base currency (USD) that the rates are compared against

        [JsonPropertyName("conversion_rates")] //matches the Json key at the API
        public Dictionary<string, decimal> ConversionRates { get; set; } // maps  currency code to exchange rate
    }
}