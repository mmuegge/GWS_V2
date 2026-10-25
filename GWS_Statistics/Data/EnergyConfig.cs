namespace GWS_Statistics.Data
{
    public static class EnergyConfig
    {
        // ==========================================
        // 1. TAGES-LIMITS (Für CalcConsumptionDaily / CalcIntervalConsumption)
        // ==========================================
        // Maximal plausibler Verbrauch für ein kurzes Tagesintervall (inkl. Urlaubspuffer)
        public const double DAILY_MAX_DROP_ELECTRIC = 50.0; // kWh
        public const double DAILY_MAX_DROP_GAS = 50.0;      // m³
        public const double DAILY_MAX_DROP_WATER = 10.0;     // m³

        // ==========================================
        // 2. TAUSCH-LIMITS (Für CalculatePeriodConsumptionWithChanges)
        // ==========================================
        // Maximal plausibler Verbrauch über größere Tausch-Zeiträume (Wochen/Monate)
        public const double CHANGE_MAX_DROP_ELECTRIC = 5000.0; // kWh
        public const double CHANGE_MAX_DROP_GAS = 2000.0;      // m³
        public const double CHANGE_MAX_DROP_WATER = 200.0;     // m³
    }
}
