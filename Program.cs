using System.Runtime.CompilerServices;
using AssetTracker.Services;
using AssetTracker.UI;
using AssetTracker.Infrastructure;
using AssetTracker.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {

        using var context = new AssetContext();

        context.Database.Migrate();; // Creates the database if it does not exist and applies any pending migrations

        CurrencyRates liveRates = await LiveCurrencyFetcher.FetchRatesAsync(); // Fetch rates
        var rates = liveRates.ConversionRates; // Dictionary mapping from currency codes to rates

        SeedData.Initialize(context, rates); // Seed the database with initial data if it is empty

        var manager = new AssetManager(context);
        var ui = new ConsoleUI(manager, rates);
        ui.Run();
    }
}

