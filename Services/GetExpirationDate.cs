using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AssetTracker.Helpers
{
    public static class GetExpirationDate // Helper function to check the expiration date of assets
    {
        public static (string, bool) CheckDate(DateTime purchaseDate)
        {
            string color = "";
            bool expired = false;

            DateTime expirationDate = purchaseDate.AddYears(3);
            DateTime today = DateTime.Now;

            TimeSpan difference = expirationDate - today;

            if (expirationDate <= today)
            {
                color = "red";
                expired = true;
                return (color, expired);
            }
            else if (difference.TotalDays <= 180) // less than 6 months
            {
                if (difference.TotalDays <= 90) // less than 3 months
                {
                    color = "red";
                    return (color, expired);
                }

                color = "yellow";
                return (color, expired);
            }
            else // expiration date is more than 6 months away
            {
                return (color, expired); 
            }
        }
    }
}