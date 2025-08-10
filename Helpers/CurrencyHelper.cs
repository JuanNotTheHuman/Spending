using System.Collections.Generic;

namespace JuanNotTheHuman.Spending.Helpers
{
    /**
     * <summary>
     * Information about a currency, including its name, symbol, ISO code, country, and culture name.
     * </summary>
     */
    internal class CurrencyInfo
    {
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ISOCurrencySymbol { get; set; }
        public string Country { get; set; }
        public string CultureName { get; set; }
    }
    /**
     * <summary>
     * A helper class to provide a list of common currencies with their details.
     * </summary>
     */
    internal static class CurrencyHelper
    {
        public static List<CurrencyInfo> GetAllCurrencies()
        {
            return new List<CurrencyInfo>
            {
                new CurrencyInfo{
                    CurrencyName = "US Dollar",
                    CurrencySymbol = "$",
                    ISOCurrencySymbol = "USD",
                    Country = "United States",
                    CultureName = "en-US"
                },
                new CurrencyInfo{
                    CurrencyName = "Euro",
                    CurrencySymbol = "€",
                    ISOCurrencySymbol = "EUR",
                    Country = "European Union",
                    CultureName = "fr-FR"
                },
                new CurrencyInfo{
                    CurrencyName = "British Pound",
                    CurrencySymbol = "£",
                    ISOCurrencySymbol = "GBP",
                    Country = "United Kingdom",
                    CultureName = "en-GB"
                },
                new CurrencyInfo{
                    CurrencyName = "Japanese Yen",
                    CurrencySymbol = "¥",
                    ISOCurrencySymbol = "JPY",
                    Country = "Japan",
                    CultureName = "ja-JP"
                },
                new CurrencyInfo{
                    CurrencyName = "Canadian Dollar",
                    CurrencySymbol = "$",
                    ISOCurrencySymbol = "CAD",
                    Country = "Canada",
                    CultureName = "en-CA"
                },
                new CurrencyInfo{
                    CurrencyName = "Australian Dollar",
                    CurrencySymbol = "$",
                    ISOCurrencySymbol = "AUD",
                    Country = "Australia",
                    CultureName = "en-AU"
                },
                new CurrencyInfo{
                    CurrencyName = "Polish Zloty",
                    CurrencySymbol = "zł",
                    ISOCurrencySymbol = "PLN",
                    Country = "Poland",
                    CultureName = "pl-PL"
                },
            };
        }
    }

}
