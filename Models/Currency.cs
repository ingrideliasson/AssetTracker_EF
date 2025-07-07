using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssetTracker.Models
{
    public class Currency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }

        private Currency(string code, string symbol) //can not be instantiated outside the class
        {
            Code = code;
            Symbol = symbol;
        }

        // To help EF convert from string to Currency

        public static Currency FromCode(string code)
        {
            return code switch
            {
                "USD" => USD,
                "EUR" => EUR,
                "GBP" => GBP,
                "JPY" => JPY,
                "SEK" => SEK,
                "CAD" => CAD,
                _ => null
            };
        }

        public static readonly Currency USD = new Currency("USD", "$"); //predefined static instances
        public static readonly Currency EUR = new Currency("EUR", "€"); //use like: Currency.EUR
        public static readonly Currency GBP = new Currency("GBP", "£");
        public static readonly Currency JPY = new Currency("JPY", "¥");
        public static readonly Currency SEK = new Currency("SEK", "kr");
        public static readonly Currency CAD = new Currency("CAD", "$");


        //static method to map office location to currency
    }
}