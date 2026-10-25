using GWS_Statistics.Data;
using GWS_Statistics.Helper;

namespace GWS_Statistics.Calculation
{
    public static class GWS_Calculation
    {
        #region CalcConsumptionDaily
        /// <summary>
        /// Berechnung des täglichen Verbrauch, Berücksichtigung eines Zählerwechsels
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="supplier"></param>
        /// <param name="counterChanges"></param>
        /// <param name="counters"></param>
        /// <returns>List<IConsumption></returns>
        /// </summary>
        public static List<IConsumption> CalcConsumptionDaily<T, U>(
         ISupplier? supplier,
         List<U>? counterChanges,
         List<T>? counters,
         double maxDrop) // Puffer zur Absicherung gegen Tippfehler beim Überlauf
         where T : ICounter
         where U : class, ICounterChange
        {
            var consumptions = new List<IConsumption>();

            // 1. Guard Clauses (Frühzeitiger Abbruch bei ungültigen Daten)
            if (supplier is null) return consumptions;

            bool validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
            if (!validDates) return consumptions;

            DateTime startPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
            DateTime endPeriod = supplier.Zeitraum_Ende.GetValueOrDefault().Date;

            // Enddatum darf nicht in der Zukunft liegen
            endPeriod = endPeriod < DateTime.Now.Date ? endPeriod : DateTime.Now.Date;

            int totalDays = (endPeriod - startPeriod).Days;
            if (totalDays < 2) return consumptions;

            // 2. Fall: Keine Zählerstände vorhanden -> Lineare Aufteilung des Gesamtzeitraums
            if (counters is null || counters.Count == 0)
            {
                if (supplier.Start_Zaehlerstand.HasValue && supplier.Ende_Zaehlerstand.HasValue)
                {
                    double totalCons = 0;

                    if (supplier.Ende_Zaehlerstand.Value >= supplier.Start_Zaehlerstand.Value)
                    {
                        totalCons = supplier.Ende_Zaehlerstand.Value - supplier.Start_Zaehlerstand.Value;
                    }
                    else
                    {
                        double overflowBasis = GetCounterOverflowBasisStatic(supplier.Start_Zaehlerstand.Value);
                        double calculatedOverflow = (overflowBasis - supplier.Start_Zaehlerstand.Value) + supplier.Ende_Zaehlerstand.Value;
                        if (calculatedOverflow <= maxDrop) totalCons = calculatedOverflow;
                    }

                    double dailyCons = Math.Max(0.0d, totalCons / totalDays);

                    for (int d = 0; d < totalDays; d++)
                    {
                        consumptions.Add(new ConsumptionData
                        {
                            SupplierId = supplier.Id,
                            Date = startPeriod.AddDays(d),
                            Consumption = dailyCons,
                            Temperature = 0.0d
                        });
                    }
                }
                return consumptions;
            }

            // 3. Fall: Zählerstände vorhanden -> Berechnung über die Intervalle
            // Intervall vor dem ersten Ablesetag (Zeitraum-Start bis erster Ablesetag)
            var firstCounter = counters[0];
            if (firstCounter.Ablesetag > startPeriod && supplier.Start_Zaehlerstand.HasValue)
            {
                int initialDays = (firstCounter.Ablesetag.Date - startPeriod).Days;
                if (initialDays > 0)
                {
                    double diff = 0;
                    double firstVal = firstCounter.Zaehlerstand.GetValueOrDefault();
                    double startVal = supplier.Start_Zaehlerstand.Value;

                    if (firstVal >= startVal)
                    {
                        diff = firstVal - startVal;
                    }
                    else
                    {
                        double overflowBasis = GetCounterOverflowBasisStatic(startVal);
                        double calculatedOverflow = (overflowBasis - startVal) + firstVal;
                        if (calculatedOverflow <= maxDrop) diff = calculatedOverflow;
                    }

                    double initialDailyCons = Math.Max(0.0d, diff / initialDays);
                    double temperature = firstCounter.Temperatur_aussen ?? 0.0d;

                    for (int d = 0; d < initialDays; d++)
                    {
                        consumptions.Add(new ConsumptionData
                        {
                            SupplierId = supplier.Id,
                            Date = startPeriod.AddDays(d),
                            Consumption = initialDailyCons,
                            Temperature = temperature
                        });
                    }
                }
            }

            // =========================================================================
            // 4. Hauptberechnung: Intervalle zwischen den einzelnen Zählerständen abarbeiten
            // =========================================================================
            double trackingCurrentCons = 0;
            bool isFirstRunInInterval = true;

            // Test
            //DateTime lulliDate = new(2026, 10, 20);

            for (int i = 0; i < counters.Count; i++)
            {
                DateTime currentDay = counters[i].Ablesetag.Date;

                //Test
                //if (counters[i].Ablesetag.Date >= lulliDate)
                //{
                //    var lulli = counters[i].Ablesetag.Date;
                //}
                // Holt den Zählerstand: Entweder den echten (beim allerersten Start) 
                // oder den sicher im C#-Arbeitsspeicher mitgeführten Folgewert nach einem Wechsel!
                double currentCons = isFirstRunInInterval ? counters[i].Zaehlerstand.GetValueOrDefault() : trackingCurrentCons;
                isFirstRunInInterval = false;

                double temperature = counters[i].Temperatur_aussen ?? 0.0d;

                DateTime nextDay;
                double nextCons;

                if (i < counters.Count - 1)
                {
                    nextDay = counters[i + 1].Ablesetag.Date;
                    nextCons = counters[i + 1].Zaehlerstand.GetValueOrDefault();

                    // Standardmäßig bereiten wir den Stand des nächsten Elements vor
                    trackingCurrentCons = nextCons;
                }
                else
                {
                    // Letzter Eintrag: Bis zum Ende des Lieferanten-Zeitraums rechnen
                    nextDay = endPeriod;
                    nextCons = supplier.Ende_Zaehlerstand ?? currentCons;
                    trackingCurrentCons = nextCons;
                }

                int intervalDays = (nextDay - currentDay).Days;
                if (intervalDays <= 0) continue;

                double intervalCons = 0;

                // Relevanten Zählerwechsel finden, der EXAKT in dieses Intervall fällt
                U? changeInInterval = null;
                if (counterChanges != null)
                {
                    changeInInterval = counterChanges
                        .FirstOrDefault(cc => cc.Id_Anbieter == supplier.Id
                                           && cc.Wechsel_Datum.Date >= currentDay
                                           && cc.Wechsel_Datum.Date <= nextDay);
                }

                // Zählerwechsel priorisiert behandeln, wenn Werte sinken ODER der Tausch-Tag exakt heute stattfindet
                if (changeInInterval != null && (nextCons < currentCons || changeInInterval.Wechsel_Datum.Date == currentDay))
                {
                    // FALL A: Ein Zählerwechsel fand innerhalb dieses Intervalls statt!
                    double oldLastValue = changeInInterval.Zaehlerstand_alt ?? 0;
                    double newStartValue = changeInInterval.Zaehlerstand_neu ?? 0;

                    // 1. Verbrauch des alten Zählers bis zum Ausbau
                    if (oldLastValue >= currentCons)
                    {
                        intervalCons += (oldLastValue - currentCons);
                    }
                    else
                    {
                        double overflowBasis = GetCounterOverflowBasisStatic(currentCons);
                        double calculatedOverflow = (overflowBasis - currentCons) + oldLastValue;
                        if (calculatedOverflow <= maxDrop) intervalCons += calculatedOverflow;
                    }

                    // 2. Verbrauch des neuen Zählers ab Einbau bis zum Intervallende
                    if (nextCons >= newStartValue)
                    {
                        intervalCons += (nextCons - newStartValue);
                    }
                    else
                    {
                        double overflowBasis = GetCounterOverflowBasisStatic(newStartValue);
                        double calculatedOverflow = (overflowBasis - newStartValue) + nextCons;
                        if (calculatedOverflow <= maxDrop) intervalCons += calculatedOverflow;
                    }

                    // DER ENTSCHEIDENDE FIX: Wir weisen den neuen Wert der getrackten C#-Variable zu!
                    // Das zwingt die Schleife in der nächsten Runde dazu, mit 'nextCons' (der neuen 0) weiterzurechnen.
                    trackingCurrentCons = nextCons;
                }
                else
                {
                    // FALL B: Kein Zählerwechsel im Intervall (Reiner Verbrauch oder normaler Überlauf)
                    if (nextCons >= currentCons)
                    {
                        intervalCons = nextCons - currentCons;
                    }
                    else
                    {
                        // Normaler Zähler-Überlauf (Roll-Over) mitten im Intervall
                        double overflowBasis = GetCounterOverflowBasisStatic(currentCons);
                        double calculatedOverflow = (overflowBasis - currentCons) + nextCons;

                        if (calculatedOverflow <= maxDrop)
                        {
                            intervalCons = calculatedOverflow;
                        }
                        else
                        {
                            // Tippfehler-Bremse: Wert nullen und alten Stand mitschleifen
                            intervalCons = 0;
                            trackingCurrentCons = currentCons;
                        }
                    }
                }

                // Lineare Aufteilung des Intervall-Verbrauchs auf die einzelnen Tage
                double dailyCons = Math.Max(0.0d, intervalCons / intervalDays);

                for (int d = 0; d < intervalDays; d++)
                {
                    consumptions.Add(new ConsumptionData
                    {
                        SupplierId = supplier.Id,
                        Date = currentDay.AddDays(d),
                        Consumption = dailyCons,
                        Temperature = temperature
                    });
                }

            }

            return consumptions;
        }

        #endregion

        #region CalcConsumptionMonthly
        /// <summary>
        /// Berechnung des monatlichen Verbrauchs
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="supplier"></param>
        /// <param name="consumptions"></param>
        /// <returns>List<IConsumption></returns>
        public static List<IConsumption> CalcConsumptionMonthly<T, U>(
            ISupplier? supplier,
            List<U>? counterChanges,
            List<T>? counters,
            double maxDrop)
            where T : ICounter
            where U : class, ICounterChange // 2. KORREKTUR: 'class' hinzugefügt, um mit CalcConsumptionDaily kompatibel zu sein
        {
            if (supplier == null || counters == null || counters.Count == 0)
            {
                return [];
            }

            var start = supplier.Zeitraum_Start.GetValueOrDefault();
            var end = supplier.Zeitraum_Ende.GetValueOrDefault();

            if (!HelperMethods.CheckValidDates(start, end) || (end - start).Days < 2)
            {
                return [];
            }

            // 3. KORREKTUR: maxDrop wird hier an die tägliche Berechnung weitergereicht
            List<IConsumption> consumptions = CalcConsumptionDaily(supplier, counterChanges, counters, maxDrop);
            if (consumptions.Count == 0)
            {
                return [];
            }

            // 1. Gruppierung in einem einzigen Durchlauf (O(N))
            // 2. Filtern nach dem relevanten Zeitraum direkt in der Abfrage
            return [.. consumptions
                .Where(c => c.Date >= new DateTime(start.Year, start.Month, 1) && c.Date <= end && c.Date <= DateTime.Now)
                .GroupBy(c => new { c.Date.Year, c.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => (IConsumption)new ConsumptionData
                {
                    // Erster Tag des jeweiligen Monats
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Consumption = g.Sum(c => c.Consumption),
                    Temperature = g.Average(c => c.Temperature)
                })];
        }
        #endregion

        #region CalcConsumptionYearly
        /// <summary>
        /// Berechnen des jährlichen Verbrauches aller Anbieter, Achtung: es wird nur ein Zählerwechsel pro Anbieter berücksichtigt
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="suppliers"></param>
        /// <returns>List<IConsumption></returns>
        public static List<IConsumption> CalcConsumptionYearly<T, U>(
                List<T>? suppliers,
                List<U>? counterChangesAllSuppliers,
                double maxDrop) // NEU: Dynamisches Limit für die jeweilige Energieform
                where T : ISupplier
                where U : ICounterChange
        {
            if (suppliers == null || suppliers.Count == 0)
            {
                return [];
            }

            // 1. Alle Zählerwechsel hocheffizient nach Anbieter-ID gruppieren
            var counterChangesLookUp = counterChangesAllSuppliers?
                .GroupBy(cc => cc.Id_Anbieter)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                ) ?? [];

            // Sortierung der Anbieter im Speicher (In-Place)
            suppliers.Sort((x, y) => x.Id.CompareTo(y.Id));

            List<IConsumption> yearlyConsumptions = [];

            foreach (var supplier in suppliers)
            {
                var start = supplier.Zeitraum_Start.GetValueOrDefault();
                var end = supplier.Zeitraum_Ende.GetValueOrDefault();

                if (!HelperMethods.CheckValidDates(start, end) || (end - start).Days < 2)
                {
                    continue;
                }

                if (supplier.Start_Zaehlerstand == null || supplier.Ende_Zaehlerstand == null)
                {
                    continue;
                }

                // 2. Holt die Wechsel-Liste NUR für diesen spezifischen Zeitraum heraus
                List<U> relevantChanges = [];
                if (counterChangesLookUp.TryGetValue(supplier.Id, out var allChangesForSupplier))
                {
                    relevantChanges = allChangesForSupplier
                        .Where(cc => cc.Wechsel_Datum >= start && cc.Wechsel_Datum <= end)
                        .OrderBy(cc => cc.Wechsel_Datum)
                        .ToList();
                }

                // 3. Berechnung starten
                double yearlyCons = 0;
                double actualCounterStartValue = supplier.Start_Zaehlerstand.Value;

                // Schleife über alle Wechsel innerhalb dieses spezifischen Intervalls
                foreach (var change in relevantChanges)
                {
                    double oldLastCounterValue = change.Zaehlerstand_alt ?? 0;
                    double newCounterValue = change.Zaehlerstand_neu ?? 0;

                    if (oldLastCounterValue >= actualCounterStartValue)
                    {
                        // Normaler Verlauf des alten Zählers bis zum Ausbau
                        yearlyCons += (oldLastCounterValue - actualCounterStartValue);
                    }
                    else
                    {
                        // ABSICHERUNG: Zählerüberlauf beim alten Zähler vor dem Ausbau
                        // (Nutzt hier die dynamische Überlaufbasis basierend auf dem letzten Stand)
                        double overflowBasis = GetCounterOverflowBasisStatic(actualCounterStartValue);
                        double calculatedOverflowConsumption = (overflowBasis - actualCounterStartValue) + oldLastCounterValue;

                        // Nur aufaddieren, wenn der Sprung plausibel ist
                        if (calculatedOverflowConsumption <= maxDrop)
                        {
                            yearlyCons += calculatedOverflowConsumption;
                        }
                    }

                    // Der Startwert für den nächsten Teilabschnitt ist der neue Stand des eingebauten Zählers
                    actualCounterStartValue = newCounterValue;
                }

                // 4. Den Verbrauch des letzten (oder einzigen) Zählers bis zum Intervall-Endstand addieren
                if (supplier.Ende_Zaehlerstand.Value >= actualCounterStartValue)
                {
                    yearlyCons += (supplier.Ende_Zaehlerstand.Value - actualCounterStartValue);
                }
                else
                {
                    // ABSICHERUNG: Auch am Ende des Intervalls könnte ein Überlauf stattgefunden haben
                    double overflowBasis = GetCounterOverflowBasisStatic(actualCounterStartValue);
                    double calculatedOverflowConsumption = (overflowBasis - actualCounterStartValue) + supplier.Ende_Zaehlerstand.Value;

                    if (calculatedOverflowConsumption <= maxDrop)
                    {
                        yearlyCons += calculatedOverflowConsumption;
                    }
                }

                if (yearlyCons > 2.0d)
                {
                    yearlyConsumptions.Add(new ConsumptionData
                    {
                        SupplierId = supplier.Id,
                        Date = start,
                        Consumption = yearlyCons,
                        Temperature = 0.0d
                    });
                }
            }

            return yearlyConsumptions;
        }

        // Kleine statische Hilfsmethode für die statische Klasse, damit GetCounterOverflowBasis funktioniert:
        private static double GetCounterOverflowBasisStatic(double wert)
        {
            if (wert < 10000.0) return 10000.0;
            if (wert < 100000.0) return 100000.0;
            if (wert < 1000000.0) return 1000000.0;
            return 10000000.0;
        }
        #endregion

        #region CalcPaymentsConsumptions
        /// <summary>
        /// Berechnung Zahlungen/Verbrauch
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="paymentList"></param>
        /// <param name="counters"></param>
        /// <returns></returns>
        public static List<IPaymentConsumption> CalcPaymentsConsumptions<T, U>(ISupplier supplier, List<U>? counterChanges, List<GWS_PaymentModel>? paymentList, List<T>? counters, double maxDrop)
            where T : ICounter
            where U : class, ICounterChange
            {
                if (supplier == null) return [];

                var start = supplier.Zeitraum_Start.GetValueOrDefault();
                var end = supplier.Zeitraum_Ende.GetValueOrDefault();

                // Berechnung der Anzahl Monate
                int amountOfMonth = (end - start).Days > 365
                    ? MonthBetween(start, end) + 1
                    : MonthBetween(start, end);

                if (amountOfMonth < 1)
                {
                    return [];
                }

            // 1. Performance-Boost: Zahlungen einmalig nach Jahr/Monat gruppieren und summieren (O(M))
                var paymentsLookUp = paymentList?
                        .Where(x => x.Zahlungsart != "Bonus") // alle Zahlungen außer Bonus berücksichtigen
                        .GroupBy(x => new { x.Datum.Year, x.Datum.Month })
                        .ToDictionary(
                            g => g.Key,
                            g => g.Sum(x => x.Zahlungen ?? 0.0d)
                        ) ?? [];

                // 2. Verbräuche holen und direkt nach Jahr/Monat indexieren für eine sichere Zuordnung
                List<IConsumption> consumptionsMonthly = GWS_Calculation.CalcConsumptionMonthly(supplier, counterChanges, counters, maxDrop);
                var consumptionsLookUp = consumptionsMonthly?
                    .ToDictionary(
                        c => new { c.Date.Year, c.Date.Month },
                        c => c.Consumption ?? 0.0d
                    ) ?? [];

                List<IPaymentConsumption> payments_consumptions = new(amountOfMonth);
                DateTime currentDate = new DateTime(start.Year, start.Month, 1);

                // 3. Generierung der Monate und O(1) Abfrage der Daten
                for (int i = 0; i < amountOfMonth; i++)
                {
                    var periodKey = new { currentDate.Year, currentDate.Month };

                    // Werte aus den Dictionaries lesen (0.0d falls nicht vorhanden)
                    paymentsLookUp.TryGetValue(periodKey, out double payment);
                    consumptionsLookUp.TryGetValue(periodKey, out double consumption);

                    payments_consumptions.Add(new GWS_PaymentConsumption
                    {
                        Date = currentDate,
                        Payment = payment,
                        Consumption = consumption
                    });

                    currentDate = currentDate.AddMonths(1);
                }

                return payments_consumptions;
            }
        #endregion

        #region MonthBetween
        /// <summary>
        /// Berechnung der Monate zwisch 2 Daten
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static int MonthBetween(DateTime? startDate, DateTime? endDate)
        {
            TimeSpan ts;

            if (!startDate.HasValue || !endDate.HasValue)
            {
                return 0;
            }

            ts = endDate.GetValueOrDefault() - startDate.GetValueOrDefault();
            return (int)Math.Round(ts.TotalDays / 30.42, 0);
        }
        #endregion

        #region ConsumptionData
        /// <summary>
        /// Klasse Verbrauch
        /// </summary>
        public class ConsumptionData : IConsumption
        {
            public int SupplierId { get; set; }
            public DateTime Date { get; set; }
            public double? Consumption { get; set; }
            public double? Consumption2 { get; set; }
            public double? Consumption3 { get; set; }
            public double? Temperature { get; set; }
        }
        #endregion
    }
}
