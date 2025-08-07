using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spending.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CurrencyInfo
    {
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ISOCurrencySymbol { get; set; }
        public string Country { get; set; }
        public string CultureName { get; set; }
    }
    public static class CurrencyHelper
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
