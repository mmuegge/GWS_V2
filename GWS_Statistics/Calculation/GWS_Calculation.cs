using GWS_Statistics.Data;
using GWS_Statistics.Helper;

namespace GWS_Statistics.Calculation
{
    public static class GWS_Calculation
    {
        #region CalcConsumptionDaily
        /// <summary>
        /// Calculation of daily consumption
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="supplier"></param>
        /// <param name="counterChanges"></param>
        /// <param name="counters"></param>
        /// <returns></returns>
        /// </summary>
        public static List<IConsumption> CalcConsumptionDaily<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters)
        where T : ICounter
        where U : ICounterChange
        {
            var consumptions = new List<IConsumption>();

            // 1. Guard Clauses (Frühzeitiger Abbruch bei ungültigen Daten)
            if (supplier is null) return consumptions;

            bool validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
            if (!validDates) return consumptions;

            DateTime startPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
            DateTime endPeriod = supplier.Zeitraum_Ende.GetValueOrDefault().Date;

            endPeriod = endPeriod < DateTime.Now.Date ? endPeriod : DateTime.Now.Date; // Enddatum darf nicht in der Zukunft liegen

            int totalDays = (endPeriod - startPeriod).Days;

            if (totalDays < 2) return consumptions;

            // 2. Fall: Keine Zählerstände vorhanden -> Lineare Aufteilung des Gesamtzeitraums
            if (counters is null || counters.Count == 0)
            {
                if (supplier.Start_Zaehlerstand.HasValue && supplier.Ende_Zaehlerstand.HasValue
                    && supplier.Start_Zaehlerstand <= supplier.Ende_Zaehlerstand)
                {
                    double totalCons = (supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand).Value;
                    double dailyCons = Math.Max(0.0d, totalCons / totalDays);
                    double temperature = 0.0d;

                    for (int d = 0; d < totalDays; d++)
                    {
                        consumptions.Add(new ConsumptionData
                        {
                            SupplierId = supplier.Id,
                            Date = startPeriod.AddDays(d),
                            Consumption = dailyCons,
                            Temperature = temperature
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
                    double diff = (firstCounter.Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault();
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

            // Intervalle zwischen den einzelnen Zählerständen abarbeiten
            for (int i = 0; i < counters.Count; i++)
            {
                DateTime currentDay = counters[i].Ablesetag.Date;
                double currentCons = counters[i].Zaehlerstand.GetValueOrDefault();
                double temperature = counters[i].Temperatur_aussen ?? 0.0d;

                DateTime nextDay;
                double nextCons;

                if (i < counters.Count - 1)
                {
                    nextDay = counters[i + 1].Ablesetag.Date;
                    nextCons = counters[i + 1].Zaehlerstand.GetValueOrDefault();
                }
                else
                {
                    // Letzter Eintrag: Bis zum Ende des Lieferanten-Zeitraums rechnen
                    nextDay = endPeriod;
                    nextCons = supplier.Ende_Zaehlerstand ?? currentCons;
                }

                int intervalDays = (nextDay - currentDay).Days;
                if (intervalDays <= 0) continue;

                double dailyCons;
                if (nextCons >= currentCons)
                {
                    dailyCons = (nextCons - currentCons) / intervalDays;
                }
                else
                {
                    // Zählerwechsel-Logik greift hier
                    dailyCons = 0.0d;
                    if (counterChanges != null)
                    {
                        // TODO: Zählerwechsel-Berechnung implementieren
                    }
                }

                dailyCons = Math.Max(0.0d, dailyCons);

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
        /// Calculation of monthly consumption
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="supplier"></param>
        /// <param name="consumptions"></param>
        /// <returns></returns>
        public static List<IConsumption> CalcConsumptionMonthly<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters)
            where T : ICounter
            where U : ICounterChange
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

                List<IConsumption> consumptions = CalcConsumptionDaily(supplier, counterChanges, counters);
                if (consumptions.Count == 0)
                {
                    return [];
                }

                // 1. Gruppierung in einem einzigen Durchlauf (O(N))
                // 2. Filtern nach dem relevanten Zeitraum direkt in der Abfrage
                return consumptions
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
                    })
                    .ToList();
            }
            #endregion

        #region CalcConsumptionYearly
        /// <summary>
        /// Berechnen des jährlichen Verbrauches aller Anbieter, Achtung: es wird nur ein Zählerwechsel pro Anbieter berücksichtigt
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="suppliers"></param>
        /// <returns></returns>
        public static List<IConsumption> CalcConsumptionYearly<T, U>(List<T>? suppliers, List<U>? counterChanges)
            where T : ISupplier
            where U : ICounterChange
            {
                if (suppliers == null || suppliers.Count == 0)
                {
                    return [];
                }

                // 1. Performance-Boost: Zählerwechsel einmalig nach Anbieter-ID indexieren (O(M))
                // Holt direkt pro Anbieter den Wechsel mit dem höchsten alten Zählerstand
                var counterChangesLookUp = counterChanges?
                    .Where(cc => cc.Zaehlerstand_alt != null && cc.Zaehlerstand_neu != null)
                    .GroupBy(cc => cc.Id_Anbieter)
                    .ToDictionary(
                        g => g.Key,
                        g => g.MaxBy(cc => cc.Zaehlerstand_alt)
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

                    double? yearlyCons = 0.0d;

                    if (supplier.Start_Zaehlerstand >= supplier.Ende_Zaehlerstand) // Zählerwechsel vermutet
                    {
                        // O(1) statt O(M) durch das Dictionary
                        if (counterChangesLookUp.TryGetValue(supplier.Id, out var change))
                        {
                            yearlyCons = (change?.Zaehlerstand_alt - supplier.Start_Zaehlerstand)
                                        + (supplier.Ende_Zaehlerstand - change?.Zaehlerstand_neu);
                        }
                        else
                        {
                            continue; // Kein passender Zählerwechsel hinterlegt -> ignorieren
                        }
                    }
                    else // Normaler Verbrauch ohne Wechsel
                    {
                        yearlyCons = supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand;
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

            #endregion

        #region CalcPaymentsConsumptions
        /// <summary>
        /// Berechnung Zahlungen/Verbrauch
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="paymentList"></param>
        /// <param name="counters"></param>
        /// <returns></returns>
        public static List<IPaymentConsumption> CalcPaymentsConsumptions<T, U>(ISupplier supplier, List<U>? counterChanges, List<GWS_PaymentModel>? paymentList, List<T>? counters)
            where T : ICounter
            where U : ICounterChange
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

            // .Where(x => x.Zahlungsart == "Abschlag" || x.Zahlungsart == "Nachzahlung" || x.Zahlungsart == "Rückvergütung" || x.Zahlungsart == "Endabrechnung")

            // 1. Performance-Boost: Zahlungen einmalig nach Jahr/Monat gruppieren und summieren (O(M))
                var paymentsLookUp = paymentList?
                        .Where(x => x.Zahlungsart != "Bonus") // alle Zahlungen außer Bonus berücksichtigen
                        .GroupBy(x => new { x.Datum.Year, x.Datum.Month })
                        .ToDictionary(
                            g => g.Key,
                            g => g.Sum(x => x.Zahlungen ?? 0.0d)
                        ) ?? [];

                // 2. Verbräuche holen und direkt nach Jahr/Monat indexieren für eine sichere Zuordnung
                List<IConsumption> consumptionsMonthly = GWS_Calculation.CalcConsumptionMonthly(supplier, counterChanges, counters);
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
        /// Klasser Verbrauch
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
