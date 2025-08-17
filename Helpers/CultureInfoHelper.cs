using JuanNotTheHuman.Spending.Services;
using System.Globalization;
using System.IO;
namespace JuanNotTheHuman.Spending.Helpers
{
    /**
     * <summary>
     * A helper class for managing culture information.
     * </summary>
     */
    internal static class CultureInfoHelper
    {
        /**
         * <summary>
         * Updates the current culture information and saves it to a file.
         * </summary>
         * <param name="value">The culture information to set.</param>
         */
        public static void Update(string value)
        {
            using (var streamwriter = new StreamWriter("CultureInfo.txt", false))
            {
                streamwriter.WriteLine(value);
            }
            LocalizationService.Instance.Refresh();
            CultureInfo.CurrentCulture = new CultureInfo(value);
            CultureInfo.CurrentUICulture = new CultureInfo(value);
        }
        /**
         * <summary>
         * Retrieves the current culture information from a file or defaults to "en-US".
         * </summary>
         * <returns>The culture information as a string.</returns>
         */
        public static string Get()
        {
            if (File.Exists("CultureInfo.txt"))
            {
                using (var streamreader = new StreamReader("CultureInfo.txt"))
                {
                    return streamreader.ReadLine();
                }
            }
            else
            {
                using (var streamwriter = new StreamWriter("CultureInfo.txt", false))
                {
                    streamwriter.WriteLine("en-US");
                }
                return "en-US";
            }
        }
    }
}
