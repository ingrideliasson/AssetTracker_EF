using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using AssetTracker.Models;
using Microsoft.EntityFrameworkCore;



namespace AssetTracker.Infrastructure
{
    public class LiveCurrencyFetcher //encapsulates logic for fetching live currency data
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<CurrencyRates> FetchRatesAsync()
        {
            string url = "https://v6.exchangerate-api.com/v6/7487a77b96ec35ae5855d1c9/latest/USD";

            var response = await client.GetAsync(url); //sends GET request to API using the http client, response is an object of type HttpResponseMessage
            response.EnsureSuccessStatusCode(); //checks if the request was succesful (status 200), otherwise throws an exception

            string json = await response.Content.ReadAsStringAsync(); // read content to string
            var rates = JsonSerializer.Deserialize<CurrencyRates>(json, new JsonSerializerOptions // convert json to CurrencyRate object
            {
                PropertyNameCaseInsensitive = true // ignores case differences between CurrencyRates object and json
            });

            if (rates == null)
            {
                throw new JsonException("Failed to deserialize currency rates from API response.");
            }
            return rates;

        }

    }
}



