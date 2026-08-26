using GWS_Statistics.Data;
using GWS_Statistics.Helper;

namespace GWS_Statistics.Calculation
{
    public static class GWS_Calculation
    {
        #region Variablen
        private static ConsumptionType ConsType = ConsumptionType.Unknown;
        enum ConsumptionType
        {
            Unknown = 0,
            Gas = 1,
            Water = 2,
            Electric = 3
        }
        #endregion

        //#region CalcConsumptionDaily Gas
        ///// <summary>
        ///// Calculation of daily consumption
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="supplier"></param>
        ///// <param name="counterChanges"></param>
        ///// <param name="counters"></param>
        ///// <returns></returns>
        //public static List<IGasConsumption> CalcConsumptionDailyGas<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : ICounter where U : ICounterChange
        //{
        //    bool validDates;
        //    bool endOfCalc = false;
        //    bool dataAvailable;
        //    bool firstReading = true;
        //    int day_index = 0;
        //    int day_index_1 = 1;
        //    int daysBetween;
        //    //double[] cons = { 0, 0, 0 };
        //    double gasCons = 0;
        //    double? temperature;
        //    DateTime actDayPeriod = DateTime.Now;
        //    DateTime nextDayPeriod;
        //    double? actDayCons;
        //    double? nextDayCons;

        //    List<IGasConsumption> consumptions = [];

        //    if (supplier == null || counters == null || counters?.Count == 0)
        //    {
        //        return consumptions;
        //    }

        //    // prüfen ob übergebene Daten ungültig sind
        //    // und ob die Ableseperiode < 1 Tag ist
        //    validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //    daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //    // prüfen ob Anfangsdatum und Enddatum des Anbieters vorhanden ist, wenn nicht wird keine Berechnung durchgeführt
        //    if (!validDates || daysBetween < 2)
        //    {
        //        return consumptions;
        //    }

        //    dataAvailable = (counters != null && counters.Count > 0);

        //    // Wenn es keine Zählerstände gibt werden nur Anfangs-Zählerstand/Ende-Zählerstand berücksichtigt
        //    // hier muss der Zählerwechsel mit berücksichtigt werden, aber nur wenn der Ende-Zählerstand mit Zählerwechsel zusammen fällt
        //    if (!dataAvailable && supplier.Ende_Zaehlerstand != null && supplier.Start_Zaehlerstand != null)
        //    {
        //        if (supplier.Start_Zaehlerstand <= supplier.Ende_Zaehlerstand)
        //        {
        //            actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
        //            gasCons = (supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / daysBetween;


        //            temperature = 0.0d;
        //            for (int d = 0; d < daysBetween; d++)
        //            {
        //                // Verbrauchswert
        //                consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = gasCons, Temperature = temperature.GetValueOrDefault() });
        //                gasCons = gasCons >= 0.0d ? gasCons : 0.0d;

        //                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //            }

        //            return consumptions;
        //        }
        //        else // Zählerwechsel
        //        {
        //            // hier fehlt noch die Berücksichtigung des Zählerwechsels
        //            //braucht hier wahrscheinlich nicht berücksichtigt werden
        //        }
        //    }
        //    do
        //    {
        //        try
        //        {
        //            if (firstReading)  // nur beim ersten Durchlauf
        //            {
        //                firstReading = false;


        //                // Periode startet nicht am 01 des Monats, Tage mit Verbrauch = 0 bis zum Start der Periode eintragen
        //                if (counters?[0].Ablesetag.Day > 1)
        //                {
        //                    DateTime startDate = new DateTime(counters[0].Ablesetag.Year, counters[0].Ablesetag.Month, 1, 0, 0, 0);
        //                    for (int i = 0; i < counters?[0].Ablesetag.Day - 1; i++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = startDate, Consumption = 0.0d, Temperature = 0.0d });
        //                        startDate = startDate.AddDays(1);
        //                    }
        //                }

        //                if (counters?[0].Ablesetag > supplier.Zeitraum_Start) // erster Ablesetag > Zeitraum-Start
        //                {
        //                    actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
        //                    daysBetween = (counters[0].Ablesetag - actDayPeriod).Days;
        //                    gasCons = ((counters[0].Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / (double)daysBetween);

        //                    gasCons = gasCons >= 0.0d ? gasCons : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                    temperature = counters[day_index].Temperatur_aussen != null ? counters[day_index].Temperatur_aussen : 0;
        //                    for (int d = 0; d < daysBetween; d++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = gasCons, Temperature = temperature.GetValueOrDefault() });
        //                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                    }
        //                }
        //            }
        //            else // index > 0
        //            {
        //                actDayPeriod = counters?[day_index].Ablesetag ?? DateTime.Now;  // Datum Ablesetag
        //                actDayCons = counters?[day_index].Zaehlerstand; // Zählerstand Ablesetag

        //                if (day_index < counters?.Count || actDayPeriod < supplier.Zeitraum_Ende)
        //                {
        //                    nextDayPeriod = counters?[day_index_1].Ablesetag ?? DateTime.Now;
        //                    daysBetween = (nextDayPeriod - actDayPeriod).Days;
        //                    nextDayCons = counters?[day_index_1].Zaehlerstand;  // Zählerstand Tag+1
        //                }
        //                else
        //                {
        //                    daysBetween = 1;
        //                    nextDayCons = supplier.Ende_Zaehlerstand != null ? supplier.Ende_Zaehlerstand : actDayCons; // prüfen ob Ende_Zählerstand vorhanden ist, wenn nicht Ende-Zählerstand nehmen
        //                }

        //                if (nextDayCons.GetValueOrDefault() >= actDayCons.GetValueOrDefault()) // prüfen ob der Verbrauch Tag+1 > Verbrauch Tag ist
        //                {
        //                    gasCons = daysBetween > 0 ? ((nextDayCons - actDayCons).GetValueOrDefault() / daysBetween) : (nextDayCons - actDayCons).GetValueOrDefault();
        //                }
        //                else // Zählerstand Tag+1 < Zählerstand Tag => Zählerwechsel
        //                {
        //                    if (counterChanges != null) // gibt es einen Zählerwechsel?
        //                    {
        //                        var counterChangeDate = counterChanges
        //                           .Where(s => s.Wechsel_Datum == counters?[day_index_1].Ablesetag)
        //                           .Select(s => s).FirstOrDefault();

        //                        if (counterChangeDate != null)
        //                        {
        //                            gasCons = (counterChangeDate.Zaehlerstand_alt - counters?[day_index].Zaehlerstand + counters?[day_index_1].Zaehlerstand - counterChangeDate.Zaehlerstand_neu).GetValueOrDefault();
        //                        }
        //                        else
        //                        {
        //                            //consumption = nextDayCons.GetValueOrDefault() / daysBetween;
        //                            gasCons = 0.0d;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        gasCons = 0.0d;
        //                    }
        //                }

        //                //consumption = consumption >= 0.0d ? consumption : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                temperature = counters?[day_index].Temperatur_aussen;

        //                if (daysBetween == 1)
        //                {
        //                    consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = gasCons, Temperature = temperature });
        //                }
        //                else
        //                // wenn es zwischen 2 Ablesungen mehr als 1 Tag Unterschied gibt, müssen Datum/Verbrauch hochgerechnet werden
        //                {
        //                    for (int d = 0; d < daysBetween; d++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = gasCons, Temperature = temperature });
        //                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                    }
        //                }
        //                day_index += 1;     // Tag
        //                day_index_1 += 1;   // Tag+1
        //            }

        //            if (day_index_1 >= counters?.Count)   // Ende der Liste erreicht, Zählerstand-Ende berücksichtigen
        //            {
        //                if (supplier.Ende_Zaehlerstand != null)
        //                {
        //                    if (supplier.Zeitraum_Ende != null && supplier.Zeitraum_Ende >= counters[counters.Count - 1].Ablesetag)
        //                    {
        //                        daysBetween = (supplier.Zeitraum_Ende - counters[^1].Ablesetag).GetValueOrDefault().Days;
        //                        gasCons = ((supplier.Ende_Zaehlerstand - counters[^1].Zaehlerstand).GetValueOrDefault());

        //                        gasCons = gasCons >= 0.0d ? gasCons : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                        temperature = counters[^1].Temperatur_aussen;

        //                        if (daysBetween < 1)
        //                        {
        //                            consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = gasCons, Temperature = temperature });
        //                        }
        //                        else if (daysBetween == 1)
        //                        {
        //                            consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = gasCons / 2, Temperature = temperature });
        //                        }
        //                        else
        //                        {
        //                            for (int d = 0; d < daysBetween; d++)
        //                            {
        //                                consumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = gasCons / daysBetween, Temperature = temperature });
        //                                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                            }
        //                        }
        //                    }
        //                }
        //                endOfCalc = true;


        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"Fehler: {ex.Message}");
        //        }
        //    }
        //    while (!endOfCalc);

        //    return consumptions;
        //}
        //#endregion

        //#region CalcConsumptionMonthly Gas
        ///// <summary>
        ///// Calculation of monthly consumption
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="supplier"></param>
        ///// <param name="consumptions"></param>
        ///// <returns></returns>
        //public static List<IGasConsumption> CalcConsumptionMonthlyGas<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : ICounter where U : ICounterChange
        //{
        //    DateTime? startDate;
        //    DateTime? endDate;
        //    bool validDates;
        //    bool end = false;
        //    int daysBetween;
        //    double? monthlyCons;
        //    double? monthlyAverageTemp;

        //    List<IGasConsumption> monthlyConsumptions = [];

        //    List<IConsumption> consumptions = CalcConsumptionDaily(supplier, counterChanges, counters);

        //    if (supplier == null || counters?.Count == 0)
        //    {
        //        return monthlyConsumptions;
        //    }

        //    validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //    daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //    if (!validDates || daysBetween < 2)
        //    {
        //        return monthlyConsumptions;
        //    }

        //    startDate = new DateTime(supplier.Zeitraum_Start.GetValueOrDefault().Year, supplier.Zeitraum_Start.GetValueOrDefault().Month, 1);
        //    endDate = supplier.Zeitraum_Ende;

        //    // alle Einträge erfassen
        //    while (!end)
        //    {
        //        var consDaily = consumptions
        //                    .Where(s => s.Date.Month == startDate.GetValueOrDefault().Month && s.Date.Year == startDate.GetValueOrDefault().Year)
        //                    .Select(i => new
        //                    {
        //                        Cons = i.Consumption,
        //                        Temp = i.Temperature
        //                    });

        //        monthlyCons = consDaily.Select(s => s.Cons).Sum();
        //        monthlyAverageTemp = consDaily.Select(s => s.Temp).Average();

        //        monthlyConsumptions.Add(new ConsumptionDataGas { Date = startDate.GetValueOrDefault(), Consumption = monthlyCons, Temperature = monthlyAverageTemp });

        //        startDate = startDate.GetValueOrDefault().AddMonths(1); // Datum um einen Monat erhöhen

        //        end = (startDate >= supplier.Zeitraum_Ende || startDate > DateTime.Now);
        //    }

        //    return monthlyConsumptions;
        //}
        //#endregion

        //#region CalcConsumptionYearly Gas
        ///// <summary>
        ///// Berechnen des jährlichen Verbrauches aller Anbieter, Achtung: es wird nur ein Zählerwechsel pro Anbieter berücksichtigt
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="suppliers"></param>
        ///// <returns></returns>
        //public static List<IGasConsumption> CalcConsumptionYearlyGas<T, U>(List<T>? suppliers, List<U>? counterChanges) where T : ISupplier where U : ICounterChange
        //{
        //    bool validDates = false;
        //    int daysBetween;
        //    double? yearlyCons = 0.0d;
        //    //double cons = 0.0d;

        //    List<IGasConsumption> yearlyConsumptions = [];
        //    if (suppliers?.Count == 0)
        //    {
        //        return yearlyConsumptions;
        //    }

        //    suppliers?.Sort((x, y) => x.Id.CompareTo(y.Id)); // nach Anbieter-Id sortieren
        //    foreach (var supplier in suppliers!)
        //    {
        //        validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //        daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //        if (!validDates || daysBetween < 2)
        //        {
        //            continue;
        //        }
        //        if (supplier.Start_Zaehlerstand == null || supplier.Ende_Zaehlerstand == null)
        //        {
        //            continue;
        //        }
        //        if (supplier.Start_Zaehlerstand >= supplier.Ende_Zaehlerstand) // gab es einen Zählerwechsel?
        //        {
        //            if (counterChanges != null && counterChanges.Count > 0)
        //            {
        //                // Achtung hier wird nur ein Zählerwechsel berücksichtigt, nur der mit dem höchsten alten Zählerstand
        //                var counterChangeSupplier = counterChanges
        //                                                .Where(s => s.Id_Anbieter == supplier.Id).MaxBy(x => x.Zaehlerstand_alt);
        //                //.Select(s => s).FirstOrDefault();

        //                if (counterChangeSupplier != null && counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
        //                {
        //                    if (counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
        //                    {
        //                        yearlyCons = counterChangeSupplier.Zaehlerstand_alt - supplier.Start_Zaehlerstand + supplier.Ende_Zaehlerstand - counterChangeSupplier.Zaehlerstand_neu;
        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            yearlyCons = supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand;
        //        }


        //        if (yearlyCons > 2.0d)
        //        {
        //            yearlyConsumptions.Add(new ConsumptionDataGas { SupplierId = supplier.Id, Date = supplier.Zeitraum_Start.GetValueOrDefault(), Consumption = yearlyCons, Temperature = 0.0d });
        //        }
        //    }
        //    return yearlyConsumptions;
        //}
        //#endregion

        //#region CalcConsumptionDaily Water
        ///// <summary>
        ///// Calculation of daily consumption
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="supplier"></param>
        ///// <param name="counterChanges"></param>
        ///// <param name="counters"></param>
        ///// <returns></returns>
        //public static List<IWaterConsumption> CalcConsumptionDailyWater<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : IWaterCounter where U : ICounterChange
        //{
        //    bool validDates;
        //    bool endOfCalc = false;
        //    bool dataAvailable;
        //    bool firstReading = true;
        //    int day_index = 0;
        //    int day_index_1 = 1;
        //    int daysBetween;
        //    double[] waterCons = [0, 0];
        //    double? temperature;
        //    DateTime actDayPeriod = DateTime.Now;
        //    DateTime nextDayPeriod;
        //    double? actDayCons;
        //    double? nextDayCons;

        //    List<IWaterConsumption> consumptions = [];

        //    if (supplier == null || counters == null || counters?.Count == 0)
        //    {
        //        return consumptions;
        //    }

        //    // prüfen ob übergebene Daten ungültig sind
        //    // und ob die Ableseperiode < 1 Tag ist
        //    validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //    daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //    // prüfen ob Anfangsdatum und Enddatum des Anbieters vorhanden ist, wenn nicht wird keine Berechnung durchgeführt
        //    if (!validDates || daysBetween < 2)
        //    {
        //        return consumptions;
        //    }

        //    dataAvailable = (counters != null && counters.Count > 0);

        //    // Wenn es keine Zählerstände gibt werden nur Anfangs-Zählerstand/Ende-Zählerstand berücksichtigt
        //    // hier muss der Zählerwechsel mit berücksichtigt werden, aber nur wenn der Ende-Zählerstand mit Zählerwechsel zusammen fällt
        //    if (!dataAvailable && supplier.Ende_Zaehlerstand != null && supplier.Start_Zaehlerstand != null)
        //    {
        //        if (supplier.Start_Zaehlerstand <= supplier.Ende_Zaehlerstand)
        //        {
        //            actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
        //            waterCons[0] = (supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / daysBetween;


        //            temperature = 0.0d;
        //            for (int d = 0; d < daysBetween; d++)
        //            {
        //                // Verbrauchswert
        //                consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = waterCons[0], Temperature = temperature.GetValueOrDefault() });
        //                waterCons[0] = waterCons[0] >= 0.0d ? waterCons[0] : 0.0d;

        //                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //            }

        //            return consumptions;
        //        }
        //        else // Zählerwechsel
        //        {
        //            // hier fehlt noch die Berücksichtigung des Zählerwechsels
        //            //braucht hier wahrscheinlich nicht berücksichtigt werden
        //        }
        //    }
        //    do
        //    {
        //        try
        //        {
        //            if (firstReading)  // nur beim ersten Durchlauf
        //            {
        //                firstReading = false;

        //                // Periode startet nicht am 01 des Monats, Tage mit Verbrauch = 0 bis zum Start der Periode eintragen
        //                if (counters?[0].Ablesetag.Day > 1)
        //                {
        //                    DateTime startDate = new(counters[0].Ablesetag.Year, counters[0].Ablesetag.Month, 1, 0, 0, 0);
        //                    for (int i = 0; i < counters?[0].Ablesetag.Day - 1; i++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = startDate, Consumption = 0.0d, ConsumptionOutside = 0.0d, Temperature = 0.0d });
        //                        startDate = startDate.AddDays(1);
        //                    }
        //                }

        //                if (counters?[0].Ablesetag > supplier.Zeitraum_Start) // erster Ablesetag > Zeitraum-Start
        //                {
        //                    actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
        //                    daysBetween = (counters[0].Ablesetag - actDayPeriod).Days;

        //                    waterCons[0] = ((counters[0].Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / (double)daysBetween);
        //                    waterCons[0] = waterCons[0] >= 0.0d ? waterCons[0] : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                    waterCons[1] = ((counters[1].Zaehlerstand_aussen - supplier.Start_Zaehlerstand).GetValueOrDefault() / (double)daysBetween);
        //                    waterCons[1] = waterCons[1] >= 0.0d ? waterCons[1] : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                    temperature = counters[day_index].Temperatur_aussen != null ? counters[day_index].Temperatur_aussen : 0;
        //                    for (int d = 0; d < daysBetween; d++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = waterCons[0], Temperature = temperature.GetValueOrDefault() });
        //                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                    }
        //                }
        //            }
        //            else // index > 0
        //            {
        //                actDayPeriod = counters?[day_index].Ablesetag ?? DateTime.Now;  // Datum Ablesetag
        //                actDayCons = counters?[day_index].Zaehlerstand; // Zählerstand Ablesetag

        //                if (day_index < counters?.Count || actDayPeriod < supplier.Zeitraum_Ende)
        //                {
        //                    nextDayPeriod = counters?[day_index_1].Ablesetag ?? DateTime.Now;
        //                    daysBetween = (nextDayPeriod - actDayPeriod).Days;
        //                    nextDayCons = counters?[day_index_1].Zaehlerstand;  // Zählerstand Tag+1
        //                }
        //                else
        //                {
        //                    daysBetween = 1;
        //                    nextDayCons = supplier.Ende_Zaehlerstand != null ? supplier.Ende_Zaehlerstand : actDayCons; // prüfen ob Ende_Zählerstand vorhanden ist, wenn nicht Ende-Zählerstand nehmen
        //                }

        //                if (nextDayCons.GetValueOrDefault() >= actDayCons.GetValueOrDefault()) // prüfen ob der Verbrauch Tag+1 > Verbrauch Tag ist
        //                {
        //                    waterCons[0] = daysBetween > 0 ? ((nextDayCons - actDayCons).GetValueOrDefault() / daysBetween) : (nextDayCons - actDayCons).GetValueOrDefault();
        //                }
        //                else // Zählerstand Tag+1 < Zählerstand Tag => Zählerwechsel
        //                {
        //                    if (counterChanges != null) // gibt es einen Zählerwechsel?
        //                    {
        //                        var counterChangeDate = counterChanges
        //                           .Where(s => s.Wechsel_Datum == counters?[day_index_1].Ablesetag)
        //                           .Select(s => s).FirstOrDefault();

        //                        if (counterChangeDate != null)
        //                        {
        //                            waterCons[0] = (counterChangeDate.Zaehlerstand_alt - counters?[day_index].Zaehlerstand + counters?[day_index_1].Zaehlerstand - counterChangeDate.Zaehlerstand_neu).GetValueOrDefault();
        //                        }
        //                        else
        //                        {
        //                            //consumption = nextDayCons.GetValueOrDefault() / daysBetween;
        //                            waterCons[0] = 0.0d;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        waterCons[0] = 0.0d;
        //                    }
        //                }

        //                //consumption = consumption >= 0.0d ? consumption : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                temperature = counters?[day_index].Temperatur_aussen;

        //                if (daysBetween == 1)
        //                {
        //                    consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = waterCons[0], Temperature = temperature });
        //                }
        //                else
        //                // wenn es zwischen 2 Ablesungen mehr als 1 Tag Unterschied gibt, müssen Datum/Verbrauch hochgerechnet werden
        //                {
        //                    for (int d = 0; d < daysBetween; d++)
        //                    {
        //                        consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = waterCons[0], Temperature = temperature });
        //                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                    }
        //                }
        //                day_index += 1;     // Tag
        //                day_index_1 += 1;   // Tag+1
        //            }

        //            if (day_index_1 >= counters?.Count)   // Ende der Liste erreicht, Zählerstand-Ende berücksichtigen
        //            {
        //                if (supplier.Ende_Zaehlerstand != null)
        //                {
        //                    if (supplier.Zeitraum_Ende != null && supplier.Zeitraum_Ende >= counters[counters.Count - 1].Ablesetag)
        //                    {
        //                        daysBetween = (supplier.Zeitraum_Ende - counters[^1].Ablesetag).GetValueOrDefault().Days;
        //                        waterCons[0] = ((supplier.Ende_Zaehlerstand - counters[^1].Zaehlerstand).GetValueOrDefault());

        //                        waterCons[0] = waterCons[0] >= 0.0d ? waterCons[0] : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

        //                        temperature = counters[^1].Temperatur_aussen;

        //                        if (daysBetween < 1)
        //                        {
        //                            consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = waterCons[0], Temperature = temperature });
        //                        }
        //                        else if (daysBetween == 1)
        //                        {
        //                            consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = waterCons[0] / 2, Temperature = temperature });
        //                        }
        //                        else
        //                        {
        //                            for (int d = 0; d < daysBetween; d++)
        //                            {
        //                                consumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = waterCons[0] / daysBetween, Temperature = temperature });
        //                                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
        //                            }
        //                        }
        //                    }
        //                }
        //                endOfCalc = true;


        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"Fehler: {ex.Message}");
        //        }
        //    }
        //    while (!endOfCalc);

        //    return consumptions;
        //}
        //#endregion

        //#region CalcConsumptionMonthly Water
        ///// <summary>
        ///// Calculation of monthly consumption
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="supplier"></param>
        ///// <param name="consumptions"></param>
        ///// <returns></returns>
        //public static List<IWaterConsumption> CalcConsumptionMonthlyWater<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : IWaterCounter where U : ICounterChange
        //{
        //    DateTime? startDate;
        //    DateTime? endDate;
        //    bool validDates;
        //    bool end = false;
        //    int daysBetween;
        //    double? monthlyCons;
        //    double? monthlyAverageTemp;

        //    List<IWaterConsumption> monthlyConsumptions = [];

        //    List<IWaterConsumption> consumptions = CalcConsumptionDailyWater(supplier, counterChanges, counters);

        //    if (supplier == null || counters?.Count == 0)
        //    {
        //        return monthlyConsumptions;
        //    }

        //    validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //    daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //    if (!validDates || daysBetween < 2)
        //    {
        //        return monthlyConsumptions;
        //    }

        //    startDate = new DateTime(supplier.Zeitraum_Start.GetValueOrDefault().Year, supplier.Zeitraum_Start.GetValueOrDefault().Month, 1);
        //    endDate = supplier.Zeitraum_Ende;

        //    // alle Einträge erfassen
        //    while (!end)
        //    {
        //        var consDaily = consumptions
        //                    .Where(s => s.Date.Month == startDate.GetValueOrDefault().Month && s.Date.Year == startDate.GetValueOrDefault().Year)
        //                    .Select(i => new
        //                    {
        //                        Cons = i.Consumption,
        //                        Temp = i.Temperature
        //                    });

        //        monthlyCons = consDaily.Select(s => s.Cons).Sum();
        //        monthlyAverageTemp = consDaily.Select(s => s.Temp).Average();

        //        monthlyConsumptions.Add(new ConsumptionDataWater { Date = startDate.GetValueOrDefault(), Consumption = monthlyCons, Temperature = monthlyAverageTemp });

        //        startDate = startDate.GetValueOrDefault().AddMonths(1); // Datum um einen Monat erhöhen

        //        end = (startDate >= supplier.Zeitraum_Ende || startDate > DateTime.Now);
        //    }

        //    return monthlyConsumptions;
        //}
        //#endregion

        //#region CalcConsumptionYearly Water
        ///// <summary>
        ///// Berechnen des jährlichen Verbrauches aller Anbieter, Achtung: es wird nur ein Zählerwechsel pro Anbieter berücksichtigt
        ///// </summary>
        ///// <typeparam name="T, U"></typeparam>
        ///// <param name="suppliers"></param>
        ///// <returns></returns>
        //public static List<IWaterConsumption> CalcConsumptionYearlyWater<T, U>(List<T>? suppliers, List<U>? counterChanges) where T : ISupplier where U : ICounterChange
        //{
        //    bool validDates = false;
        //    int daysBetween;
        //    double? yearlyCons = 0.0d;
        //    //double cons = 0.0d;

        //    List<IWaterConsumption> yearlyConsumptions = [];
        //    if (suppliers?.Count == 0)
        //    {
        //        return yearlyConsumptions;
        //    }

        //    suppliers?.Sort((x, y) => x.Id.CompareTo(y.Id)); // nach Anbieter-Id sortieren
        //    foreach (var supplier in suppliers!)
        //    {
        //        validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
        //        daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
        //        if (!validDates || daysBetween < 2)
        //        {
        //            continue;
        //        }
        //        if (supplier.Start_Zaehlerstand == null || supplier.Ende_Zaehlerstand == null)
        //        {
        //            continue;
        //        }
        //        if (supplier.Start_Zaehlerstand >= supplier.Ende_Zaehlerstand) // gab es einen Zählerwechsel?
        //        {
        //            if (counterChanges != null && counterChanges.Count > 0)
        //            {
        //                // Achtung hier wird nur ein Zählerwechsel berücksichtigt, nur der mit dem höchsten alten Zählerstand
        //                var counterChangeSupplier = counterChanges
        //                                                .Where(s => s.Id_Anbieter == supplier.Id).MaxBy(x => x.Zaehlerstand_alt);
        //                //.Select(s => s).FirstOrDefault();

        //                if (counterChangeSupplier != null && counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
        //                {
        //                    if (counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
        //                    {
        //                        yearlyCons = counterChangeSupplier.Zaehlerstand_alt - supplier.Start_Zaehlerstand + supplier.Ende_Zaehlerstand - counterChangeSupplier.Zaehlerstand_neu;
        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            yearlyCons = supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand;
        //        }


        //        if (yearlyCons > 2.0d)
        //        {
        //            yearlyConsumptions.Add(new ConsumptionDataWater { SupplierId = supplier.Id, Date = supplier.Zeitraum_Start.GetValueOrDefault(), Consumption = yearlyCons, Temperature = 0.0d });
        //        }
        //    }
        //    return yearlyConsumptions;
        //}
        //#endregion

        #region CalcConsumptionDaily
        /// <summary>
        /// Calculation of daily consumption
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="supplier"></param>
        /// <param name="counterChanges"></param>
        /// <param name="counters"></param>
        /// <returns></returns>
        public static List<IConsumption> CalcConsumptionDaily<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : ICounter where U : ICounterChange
        {
            bool validDates;
            bool endOfCalc = false;
            bool dataAvailable;
            bool firstReading = true;
            int day_index = 0;
            int day_index_1 = 1;
            int daysBetween;
            double[] cons = { 0, 0, 0 };
            double? temperature;
            DateTime actDayPeriod = DateTime.Now;
            DateTime nextDayPeriod;
            double? actDayCons;
            double? nextDayCons;
            //ICounterChange counterChange;

            Gas_SupplierModel? GasSupplier = null;
            Water_SupplierModel? WaterSupplier = null;
            Electric_SupplierModel? ElectricSupplier = null;

            List<IConsumption> consumptions = [];

            if (supplier == null || counters == null || counters?.Count == 0)
            {
                return consumptions;
            }

            Type suppElementType = supplier!.GetType();

            // Auslesen des Typen des List-Elements Counters
            Type consElementType = counters!.GetType().GetGenericArguments().Single();

            switch (consElementType.Name)  // Abfrage des Namens des List-Elements
            {
                case "Water_CounterModel":
                    ConsType = ConsumptionType.Water;
                    WaterSupplier = (Water_SupplierModel)supplier;
                    break;

                case "Gas_CounterModel":
                    ConsType = ConsumptionType.Gas;
                    GasSupplier = (Gas_SupplierModel)supplier;
                    break;

                case "Electric_CounterModel":
                    ConsType = ConsumptionType.Electric;
                    ElectricSupplier = (Electric_SupplierModel)supplier;
                    break;

                default:
                    ConsType = ConsumptionType.Unknown;
                    break;
            }

            // prüfen ob übergebene Daten ungültig sind
            // und ob die Ableseperiode < 1 Tag ist
            validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
            daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
            // prüfen ob Anfangsdatum und Enddatum des Anbieters vorhanden ist, wenn nicht wird keine Berechnung durchgeführt
            if (!validDates || daysBetween < 2)
            {
                return consumptions;
            }

            dataAvailable = (counters != null && counters.Count > 0);

            // Wenn es keine Zählerstände gibt werden nur Anfangs-Zählerstand/Ende-Zählerstand berücksichtigt
            // hier muss der Zählerwechsel mit berücksichtigt werden, aber nur wenn der Ende-Zählerstand mit Zählerwechsel zusammen fällt
            if (!dataAvailable && supplier.Ende_Zaehlerstand != null && supplier.Start_Zaehlerstand != null)
            {
                if (supplier.Start_Zaehlerstand <= supplier.Ende_Zaehlerstand)
                {
                    actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
                    cons[0] = (supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / daysBetween;


                    temperature = 0.0d;
                    for (int d = 0; d < daysBetween; d++)
                    {
                        // Verbrauchswert 1
                        consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0], Temperature = temperature.GetValueOrDefault() });
                        cons[0] = cons[0] >= 0.0d ? cons[0] : 0.0d;

                        consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0], Temperature = temperature.GetValueOrDefault() });
                        cons[0] = cons[0] >= 0.0d ? cons[0] : 0.0d;

                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
                    }

                    return consumptions;
                }
                else // Zählerwechsel
                {
                    // hier fehlt noch die Berücksichtigung des Zählerwechsels
                    //braucht hier wahrscheinlich nicht berücksichtigt werden
                }
            }
            do
            {
                try
                {
                    if (firstReading)  // nur beim ersten Durchlauf
                    {
                        firstReading = false;


                        // Periode startet nicht am 01 des Monats, Tage mit Verbrauch = 0 bis zum Start der Periode eintragen
                        if (counters?[0].Ablesetag.Day > 1)
                        {
                            DateTime startDate = new DateTime(counters[0].Ablesetag.Year, counters[0].Ablesetag.Month, 1, 0, 0, 0);
                            for (int i = 0; i < counters?[0].Ablesetag.Day - 1; i++)
                            {
                                consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = startDate, Consumption = 0.0d, Temperature = 0.0d });
                                startDate = startDate.AddDays(1);
                            }
                        }

                        if (counters?[0].Ablesetag > supplier.Zeitraum_Start) // erster Ablesetag > Zeitraum-Start
                        {
                            actDayPeriod = supplier.Zeitraum_Start.GetValueOrDefault().Date;
                            daysBetween = (counters[0].Ablesetag - actDayPeriod).Days;
                            cons[0] = ((counters[0].Zaehlerstand - supplier.Start_Zaehlerstand).GetValueOrDefault() / (double)daysBetween);

                            cons[0] = cons[0] >= 0.0d ? cons[0] : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

                            temperature = counters[day_index].Temperatur_aussen != null ? counters[day_index].Temperatur_aussen : 0;
                            for (int d = 0; d < daysBetween; d++)
                            {
                                consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0], Temperature = temperature.GetValueOrDefault() });
                                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
                            }
                        }
                    }
                    else // index > 0
                    {
                        actDayPeriod = counters?[day_index].Ablesetag ?? DateTime.Now;  // Datum Ablesetag
                        actDayCons = counters?[day_index].Zaehlerstand; // Zählerstand Ablesetag

                        if (day_index < counters?.Count || actDayPeriod < supplier.Zeitraum_Ende)
                        {
                            nextDayPeriod = counters?[day_index_1].Ablesetag ?? DateTime.Now;
                            daysBetween = (nextDayPeriod - actDayPeriod).Days;
                            nextDayCons = counters?[day_index_1].Zaehlerstand;  // Zählerstand Tag+1
                        }
                        else
                        {
                            daysBetween = 1;
                            nextDayCons = supplier.Ende_Zaehlerstand != null ? supplier.Ende_Zaehlerstand : actDayCons; // prüfen ob Ende_Zählerstand vorhanden ist, wenn nicht Ende-Zählerstand nehmen
                        }

                        if (nextDayCons.GetValueOrDefault() >= actDayCons.GetValueOrDefault()) // prüfen ob der Verbrauch Tag+1 > Verbrauch Tag ist
                        {
                            cons[0] = daysBetween > 0 ? ((nextDayCons - actDayCons).GetValueOrDefault() / daysBetween) : (nextDayCons - actDayCons).GetValueOrDefault();
                        }
                        else // Zählerstand Tag+1 < Zählerstand Tag => Zählerwechsel
                        {
                            if (counterChanges != null) // gibt es einen Zählerwechsel?
                            {
                                var counterChangeDate = counterChanges
                                   .Where(s => s.Wechsel_Datum == counters?[day_index_1].Ablesetag)
                                   .Select(s => s).FirstOrDefault();

                                if (counterChangeDate != null)
                                {
                                    cons[0] = (counterChangeDate.Zaehlerstand_alt - counters?[day_index].Zaehlerstand + counters?[day_index_1].Zaehlerstand - counterChangeDate.Zaehlerstand_neu).GetValueOrDefault();
                                }
                                else
                                {
                                    //consumption = nextDayCons.GetValueOrDefault() / daysBetween;
                                    cons[0] = 0.0d;
                                }
                            }
                            else
                            {
                                cons[0] = 0.0d;
                            }
                        }

                        //consumption = consumption >= 0.0d ? consumption : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

                        temperature = counters?[day_index].Temperatur_aussen;

                        if (daysBetween == 1)
                        {
                            consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0], Temperature = temperature });
                        }
                        else
                        // wenn es zwischen 2 Ablesungen mehr als 1 Tag Unterschied gibt, müssen Datum/Verbrauch hochgerechnet werden
                        {
                            for (int d = 0; d < daysBetween; d++)
                            {
                                consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0], Temperature = temperature });
                                actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
                            }
                        }
                        day_index += 1;     // Tag
                        day_index_1 += 1;   // Tag+1
                    }

                    if (day_index_1 >= counters?.Count)   // Ende der Liste erreicht, Zählerstand-Ende berücksichtigen
                    {
                        if (supplier.Ende_Zaehlerstand != null)
                        {
                            if (supplier.Zeitraum_Ende != null && supplier.Zeitraum_Ende >= counters[counters.Count - 1].Ablesetag)
                            {
                                daysBetween = (supplier.Zeitraum_Ende - counters[^1].Ablesetag).GetValueOrDefault().Days;
                                cons[0] = ((supplier.Ende_Zaehlerstand - counters[^1].Zaehlerstand).GetValueOrDefault());

                                cons[0] = cons[0] >= 0.0d ? cons[0] : 0.0d; // prüfen ob Zählerstand >= 0 wenn nein Verbrauch=0.0

                                temperature = counters[^1].Temperatur_aussen;

                                if (daysBetween < 1)
                                {
                                    consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = cons[0], Temperature = temperature });
                                }
                                else if (daysBetween == 1)
                                {
                                    consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = supplier.Zeitraum_Ende.GetValueOrDefault().Date, Consumption = cons[0] / 2, Temperature = temperature });
                                }
                                else
                                {
                                    for (int d = 0; d < daysBetween; d++)
                                    {
                                        consumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = actDayPeriod, Consumption = cons[0] / daysBetween, Temperature = temperature });
                                        actDayPeriod = actDayPeriod.AddDays(1); // Datum um einen Tag erhöhen
                                    }
                                }
                            }
                        }
                        endOfCalc = true;


                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Fehler: {ex.Message}");
                }
            }
            while (!endOfCalc);

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
        public static List<IConsumption> CalcConsumptionMonthly<T, U>(ISupplier? supplier, List<U>? counterChanges, List<T>? counters) where T : ICounter where U : ICounterChange
        {
            DateTime? startDate;
            DateTime? endDate;
            bool validDates;
            bool end = false;
            int daysBetween;
            double? monthlyCons;
            double? monthlyAverageTemp;

            List<IConsumption> monthlyConsumptions = [];

            List<IConsumption> consumptions = CalcConsumptionDaily(supplier, counterChanges, counters);

            if (supplier == null || counters?.Count == 0)
            {
                return monthlyConsumptions;
            }

            validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
            daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
            if (!validDates || daysBetween < 2)
            {
                return monthlyConsumptions;
            }

            startDate = new DateTime(supplier.Zeitraum_Start.GetValueOrDefault().Year, supplier.Zeitraum_Start.GetValueOrDefault().Month, 1);
            endDate = supplier.Zeitraum_Ende;

            // alle Einträge erfassen
            while (!end)
            {
                var consDaily = consumptions
                            .Where(s => s.Date.Month == startDate.GetValueOrDefault().Month && s.Date.Year == startDate.GetValueOrDefault().Year)
                            .Select(i => new
                            {
                                Cons = i.Consumption,
                                Temp = i.Temperature
                            });

                monthlyCons = consDaily.Select(s => s.Cons).Sum();
                monthlyAverageTemp = consDaily.Select(s => s.Temp).Average();

                monthlyConsumptions.Add(new ConsumptionData { Date = startDate.GetValueOrDefault(), Consumption = monthlyCons, Temperature = monthlyAverageTemp });

                startDate = startDate.GetValueOrDefault().AddMonths(1); // Datum um einen Monat erhöhen

                end = (startDate >= supplier.Zeitraum_Ende || startDate > DateTime.Now);
            }

            return monthlyConsumptions;
        }
        #endregion

        #region CalcConsumptionYearly
        /// <summary>
        /// Berechnen des jährlichen Verbrauches aller Anbieter, Achtung: es wird nur ein Zählerwechsel pro Anbieter berücksichtigt
        /// </summary>
        /// <typeparam name="T, U"></typeparam>
        /// <param name="suppliers"></param>
        /// <returns></returns>
        public static List<IConsumption> CalcConsumptionYearly<T, U>(List<T>? suppliers, List<U>? counterChanges) where T : ISupplier where U : ICounterChange
        {
            bool validDates = false;
            int daysBetween;
            double? yearlyCons = 0.0d;
            //double cons = 0.0d;

            List<IConsumption> yearlyConsumptions = [];
            if (suppliers?.Count == 0)
            {
                return yearlyConsumptions;
            }

            suppliers?.Sort((x, y) => x.Id.CompareTo(y.Id)); // nach Anbieter-Id sortieren
            foreach (var supplier in suppliers!)
            {
                validDates = HelperMethods.CheckValidDates(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
                daysBetween = validDates ? (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days : 0;  // Anzahl Tage zwischen Zeitraum-Start u. Zeitraum-Ende
                if (!validDates || daysBetween < 2)
                {
                    continue;
                }
                if (supplier.Start_Zaehlerstand == null || supplier.Ende_Zaehlerstand == null)
                {
                    continue;
                }
                if (supplier.Start_Zaehlerstand >= supplier.Ende_Zaehlerstand) // gab es einen Zählerwechsel?
                {
                    if (counterChanges != null && counterChanges.Count > 0)
                    {
                        // Achtung hier wird nur ein Zählerwechsel berücksichtigt, nur der mit dem höchsten alten Zählerstand
                        var counterChangeSupplier = counterChanges
                                                        .Where(s => s.Id_Anbieter == supplier.Id).MaxBy(x => x.Zaehlerstand_alt);
                        //.Select(s => s).FirstOrDefault();

                        if (counterChangeSupplier != null && counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
                        {
                            if (counterChangeSupplier.Zaehlerstand_alt != null && counterChangeSupplier.Zaehlerstand_neu != null)
                            {
                                yearlyCons = counterChangeSupplier.Zaehlerstand_alt - supplier.Start_Zaehlerstand + supplier.Ende_Zaehlerstand - counterChangeSupplier.Zaehlerstand_neu;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    yearlyCons = supplier.Ende_Zaehlerstand - supplier.Start_Zaehlerstand;
                }


                if (yearlyCons > 2.0d)
                {
                    yearlyConsumptions.Add(new ConsumptionData { SupplierId = supplier.Id, Date = supplier.Zeitraum_Start.GetValueOrDefault(), Consumption = yearlyCons, Temperature = 0.0d });
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
        public static List<IPaymentConsumption> CalcPaymentsConsumptions<T, U>(ISupplier supplier, List<U>? counterChanges, List<GWS_PaymentModel> paymentList, List<T>? counters) where T : ICounter where U : ICounterChange
        {
            int amountOfMonth;
            DateTime startDate;
            List<IPaymentConsumption> payments_consumptions = [];

            // Berechnung der Monate in der ausgewählten Periode (wenn Periode länger als volle ganze Monate --> + 1 Monat)
            amountOfMonth = (supplier.Zeitraum_Ende - supplier.Zeitraum_Start).GetValueOrDefault().Days > 365 ?
                                MonthBetween(supplier.Zeitraum_Start, supplier.Zeitraum_Ende) + 1 :
                                MonthBetween(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);

            //amountOfMonth = MonthBetween(supplier.Zeitraum_Start, supplier.Zeitraum_Ende);
            // prüfen ob Anzahl Monate in der Periode < 1
            if (amountOfMonth < 1)
            {
                return payments_consumptions;
            }
            // Startdatum erzeugen
            startDate = new DateTime(supplier.Zeitraum_Start.GetValueOrDefault().Year, supplier.Zeitraum_Start.GetValueOrDefault().Month, 1);

            // Abschlagszahlungen/Rückvergütung/Nachzahlungen auslesen und den Monaten zuordnen
            if (paymentList != null && paymentList?.Count > 0)
            {
                for (int i = 1; i <= amountOfMonth; i++)
                {
                    var payments = paymentList.Where(x => (x.Zahlungsart == "Abschlag" || x.Zahlungsart == "Nachzahlung" || x.Zahlungsart == "Rückvergütung") && x.Datum.Year == startDate.Year && x.Datum.Month == startDate.Month).Select(x => x.Zahlungen).Sum();
                    payments_consumptions.Add(new GWS_PaymentConsumption { Date = startDate, Payment = payments ?? 0.0d, Consumption = 0.0d });
                    startDate = startDate.AddMonths(1);
                }
            }
            // Verbrauchszahlen auslesen und dem Verbrauch zuordnen
            List<IConsumption> consumptionsMonthly = GWS_Calculation.CalcConsumptionMonthly(supplier, counterChanges, counters); // monatlichen Verbrauch berechnen
            if (consumptionsMonthly != null && consumptionsMonthly.Count > 0)
            {
                for (int i = 0; i < amountOfMonth; i++)
                {
                    if (i < consumptionsMonthly.Count)
                    {
                        payments_consumptions[i].Consumption = consumptionsMonthly[i].Consumption ?? 0.0d;
                    }
                    else
                    {
                        payments_consumptions[i].Consumption = 0.0d;
                    }
                }
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

        //#region ConsumptionDataGas
        ///// <summary>
        ///// Klasser Verbrauch
        ///// </summary>
        //public class ConsumptionDataGas : IGasConsumption
        //{
        //    public int SupplierId { get; set; }
        //    public DateTime Date { get; set; }
        //    public double? Consumption { get; set; }
        //    public double? Temperature { get; set; }
        //}
        //#endregion

        //#region ConsumptionDataWater
        ///// <summary>
        ///// Klasser Verbrauch
        ///// </summary>
        //public class ConsumptionDataWater : IWaterConsumption
        //{
        //    public int SupplierId { get; set; }
        //    public DateTime Date { get; set; }
        //    public double? Consumption { get; set; }
        //    public double? ConsumptionOutside { get; set; }
        //    public double? Temperature { get; set; }
        //}
        //#endregion

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
