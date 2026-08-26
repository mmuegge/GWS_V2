namespace GWS_Statistics.Helper
{
    public static class HelperMethods
    {
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
    }
}
