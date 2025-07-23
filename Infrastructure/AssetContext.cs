using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AssetTracker.Models;

public class AssetContext : DbContext
{
    public DbSet<Asset> Assets { get; set; }


    // Value converter to turn the Currency object into a string when saving, and vice versa when loading
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var currencyConverter = new ValueConverter<Currency, string>(
            v => v.Code, // How to store: Currency => string
            v => Currency.FromCode(v) // How to read: string => Currency
        );

        modelBuilder.Entity<Asset>().OwnsOne(a => a.PriceUSD, p =>
        {
            p.Property(pp => pp.Currency).HasConversion(currencyConverter);
        });
        modelBuilder.Entity<Asset>().OwnsOne(a => a.PriceLocal, p =>
        {
            p.Property(pp => pp.Currency).HasConversion(currencyConverter);
        });

        base.OnModelCreating(modelBuilder);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ASSET_TRACKER;User Id=sa;Password=YOURPASSWORD;TrustServerCertificate=True");
    }
}