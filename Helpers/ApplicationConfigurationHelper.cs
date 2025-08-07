using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Spending.Services;
using System.Globalization;
namespace Spending.Helpers
{
    internal static class ApplicationCultureInfoHelper
    {
        public static void Update(string value)
        {
            using(var streamwriter = new System.IO.StreamWriter("CultureInfo.txt", false))
            {
                streamwriter.WriteLine(value);
            }
            LocalizationService.Instance.Refresh();
            CultureInfo.CurrentCulture = new CultureInfo(value);
            CultureInfo.CurrentUICulture = new CultureInfo(value);
        }
        public static string Get()
        {
            try
            {
                using (var streamreader = new System.IO.StreamReader("CultureInfo.txt"))
                {
                    return streamreader.ReadLine();
                }
            }
            catch
            {
                return "en-US";
            }
        }
    }
}
