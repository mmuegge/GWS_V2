using System.Globalization;

namespace GWS_Statistics.Helper
{
    public static class HelperMethods
    {
        public static readonly CultureInfo DisplayCulture = new ("de-DE");

        /// <summary>
        /// Prüfen zweiter Daten auf Gültigkeit
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool CheckValidDates(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null)
            {
                return false;
            }
            if ((date1 < DateTime.MinValue) || (date1 > DateTime.MaxValue))
            {
                return false;
            }
            if ((date2 < DateTime.MinValue) || (date2 > DateTime.MaxValue))
            {
                return false;
            }
            if (date1 >= date2)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetDouble(object value, out double result)
        {
            switch (value)
            {
                case double d: result = d; return true;
                case float f: result = f; return true;
                case decimal m: result = (double)m; return true;
                case int i: result = i; return true; // Optional: Schützt vor Integer-Werten
                case string str:
                    if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result)) return true;
                    if (double.TryParse(str, NumberStyles.Any, DisplayCulture, out result)) return true;
                    break;
                default:
                    // Fallback für sonstige Objekte (z.B. andere Zahlentypen)
                    var s = value.ToString();
                    if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result)) return true;
                    if (double.TryParse(s, NumberStyles.Any, DisplayCulture, out result)) return true;
                    break;
            }

            result = 0;
            return false;
        }
    }


}
