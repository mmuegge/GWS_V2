using GWS_Api.Models;
using GWS_Api.Models.Gas;

namespace GWS_Api.Repositories.Gas
{
    public class Mock_GasRepository : IGasRepository
    {
        #region Variablendeklaration
        int id_Tarif = 0;
        int id_Counter = 0;
        int id_Payment = 0;
        int id_Cost = 0;
        int id_BoilerData = 0;
        int id_CounterChange = 0;

        private static readonly List<GasTarif> gasTarifList =
        [
            new ()
            {
                Id=11,
                Anbieter="GasAnbieter1",
                Tarif="Standard-Gas1",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=101,
                Ende_Zaehlerstand=20,
                Arbeitspreis=2.0,
                Grundpreis=100,
                   Zaehlermiete=1.2,
                Brennwert=2.0,
                Heizleistung=100,
                Zustandszahl=5,
                Bemerkung="Gas-Anbieter1"
            },
            new ()
            {
                Id=12,
                Anbieter="GasAnbieter2",
                Tarif="Standard-Gas2",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=102,
                Ende_Zaehlerstand=20,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Brennwert=2.0,
                Heizleistung=100,
                Zustandszahl=5,
                Bemerkung="Gas-Anbieter2"
            },
            new ()
            {
                Id=13,
                Anbieter="GasAnbieter3",
                Tarif="Standard-Gas3",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=103,
                Ende_Zaehlerstand=20,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Brennwert=2.0,
                Heizleistung=100,
                Zustandszahl=5,
                Bemerkung="Gas-Anbieter3"
            },
            new ()
            {
                Id=14,
                Anbieter="GasAnbieter4",
                Tarif="Standard-Gas4",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=104,
                Ende_Zaehlerstand=20,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Brennwert=2.0,
                Heizleistung=100,
                Zustandszahl=5,
                Bemerkung="Gas-Anbieter4"
            },
        ];

        private static readonly List<GasCounter> gasCounterList =
        [
            new ()
            {
                ID_Tag=1,
                ID_Anbieter=11,
                Ablesetag =new DateTime(2021,7,21),
                Zaehlerstand=1000,
                Temperatur_aussen=20,
                Temperatur_innen=25,
                Bemerkungen="Gas-Anbieter1"
            },
            new ()
            {
                ID_Tag=2,
                ID_Anbieter=12,
                Ablesetag =new DateTime(2021,7,22),
                Zaehlerstand=2000,
                Temperatur_aussen=21,
                Temperatur_innen=26,
                Bemerkungen="Gas-Anbieter2"
            },
            new ()
            {
                ID_Tag=3,
                ID_Anbieter=12,
                Ablesetag =new DateTime(2021,7,23),
                Zaehlerstand=2001,
                Temperatur_aussen=22,
                Temperatur_innen=23,
                Bemerkungen="Gas-Anbieter3"
            },
        ];

        private static readonly List<GasPayment> gasPaymentList =
        [
            new ()
            {
                ID_Zahlung=1,
                ID_Anbieter=11,
                Anbieter="GasAnbieter1",
                Datum=new DateTime(2021,5,1),
                Zahlungsart="Bonus",
                Zahlungen=91.0,
                Bemerkungen="Zahlung 1"
            },
            new ()
            {
                ID_Zahlung=2,
                ID_Anbieter=12,
                Anbieter="GasAnbieter2",
                Datum=new DateTime(2021,2,1),
                Zahlungsart="Abschlag",
                Zahlungen=55.0,
                Bemerkungen="Zahlung 2"
            },
            new ()
            {
                ID_Zahlung=3,
                ID_Anbieter=13,
                Anbieter="GasAnbieter3",
                Datum=new DateTime(2020,12,1),
                Zahlungsart="Abschlag",
                Zahlungen=77.0,
                Bemerkungen="Zahlung 3"
            },
            new ()
            {
                ID_Zahlung=4,
                ID_Anbieter=13,
                Anbieter="GasAnbieter1",
                Datum=new DateTime(2020,12,1),
                Zahlungsart="Bonus",
                Zahlungen=91.0,
                Bemerkungen="Zahlung 2"
            },
        ];

        private static readonly List<PaymentMethod> paymentMethodList =
        [
            new ()
            {
                ID_Zahlungsart=1,
                Zahlungsart="Abschlag",
            },
            new ()
            {
                ID_Zahlungsart=2,
                Zahlungsart="Bonus",
            },
            new ()
            {
                ID_Zahlungsart=3,
                Zahlungsart="Nachzahlung",
            },
            new ()
            {
                ID_Zahlungsart=4,
                Zahlungsart="Endabrechnung",
            },
        ];

        private static readonly List<GasCost> gasCostsList =
        [
           new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="GasAnbieter1",
                Gueltig_Ab=new DateTime(2000,1,1),
                Grundpreis=19.99d,
                Arbeitspreis=32.00d,
                Zaehlermiete=10.0d,
                Bemerkung="test1"
            },
            new ()
            {
                Id=2,
                Id_Anbieter=2,
                Anbieter="GasAnbieter2",
                Gueltig_Ab=new DateTime(2020,7,27),
                Grundpreis=15.99d,
                Arbeitspreis=45.00d,
                Zaehlermiete=12.0d,
                Bemerkung="test2"
            },
             new ()
             {
                Id=3,
                Id_Anbieter=3,
                Anbieter="GasAnbieter3",
                Gueltig_Ab=new DateTime(2022,8,12),
                Grundpreis=16.99d,
                Arbeitspreis=15.00d,
                Zaehlermiete=9.0d,
                Bemerkung="test3"
             }
        ];

        private static readonly List<GasBoiler> gasBoilerDataList =
            [
            new ()
            {
                Id=1,
                Verbrauchsjahr=new DateTime(2023, 1, 1),
                Gesamt_Verbrauch=10926.0d,
                Heizung_Verbrauch=8000.0d,
                Warmwasser_Verbrauch=2926.0d,
                Strom_Verbrauch=166.0d,
                Bemerkung="aus Therme ausgelesen"
            },
            new ()
            {
                Id=2,
                Verbrauchsjahr=new DateTime(2024, 1, 1),
                Gesamt_Verbrauch=10996.0d,
                Heizung_Verbrauch=8253.0d,
                Warmwasser_Verbrauch=2743.0d,
                Strom_Verbrauch=158.0d,
                Bemerkung="aus Therme ausgelesen"
            },
            new ()
            {
                Id=3,
                Verbrauchsjahr=new DateTime(2025, 1, 1),
                Gesamt_Verbrauch=2926.0d,
                Heizung_Verbrauch=2543.0d,
                Warmwasser_Verbrauch=383.0d,
                Strom_Verbrauch=29.7d,
                Bemerkung="aus Therme ausgelesen"
            }
            ];

        private static readonly List<Efficiency> gasEnergieEffList =
           [
           new ()
            {
                Id=1,
                Energieklasse="A+",
                Energiebedarf=30,
                Farbcode="#006400",
                Bemerkung="kWh/m2/a"
            },
            new ()
            {
                Id=2,
                Energieklasse="A",
                Energiebedarf=50,
                Farbcode="#00e600",
                Bemerkung="kWh/m2/a"
            },
            new ()
            {
               Id=3,
                Energieklasse="B",
                Energiebedarf=75,
                Farbcode="#00e600",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=4,
                Energieklasse="C",
                Energiebedarf=100,
                Farbcode="#e6e600",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=5,
                Energieklasse="D",
                Energiebedarf=130,
                Farbcode="#e6ac00",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=6,
                Energieklasse="E",
                Energiebedarf=160,
                Farbcode="#e67300",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=7,
                Energieklasse="F",
                Energiebedarf=200,
                Farbcode="#e63900",
                Bemerkung="kWh/m2/a"
            },
              new ()
            {
               Id=8,
                Energieklasse="G",
                Energiebedarf=250,
                Farbcode="#e60000",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=9,
                Energieklasse="H",
                Energiebedarf=250,
                Farbcode="#ff0000",
                Bemerkung="kWh/m2/a"
            },
           ];
        private static readonly List<GasCounterChange> gasCounterChangeList =
    [
        new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="GasAnbieter1",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Bemerkung="test1"
            },
            new ()
            {
                Id=2,
                Id_Anbieter=2,
                Anbieter="GasAnbieter2",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Bemerkung="test2"
            },
             new ()
             {
                Id=3,
                Id_Anbieter=3,
                Anbieter="GasAnbieter3",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Bemerkung="test3"
             }
    ];

        #endregion
        public Mock_GasRepository()
        {
            id_Tarif = gasTarifList.Count;
            id_Counter = gasCounterList.Count;
            id_Payment = gasPaymentList.Count;
            id_Cost = gasCostsList.Count;
            id_BoilerData = gasBoilerDataList.Count;
            id_CounterChange = gasCounterChangeList.Count;
        }

        #region ADD
        public async Task<GasCounter?> AddCounterAsync(GasCounter counter)
        {
            counter.ID_Tag = ++id_Counter;
            gasCounterList.Add(counter);
            // Name des Anbieters[id]
            var tarif = gasTarifList.Find(m => m.Id == counter.ID_Anbieter);
            if (tarif != null)
            {
                counter.Anbieter = tarif.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<GasTarif?> AddSupplierAsync(GasTarif supplier)
        {
            supplier.Id = ++id_Tarif;
            gasTarifList.Add(supplier);
            return await Task.FromResult(supplier);
        }
        public async Task<GasPayment?> AddPaymentAsync(GasPayment payment)
        {
            payment.ID_Zahlung = ++id_Payment;
            gasPaymentList.Add(payment);
            return await Task.FromResult(payment);
        }
        public async Task<GasCost?> AddCostAsync(GasCost costs)
        {
            costs.Id = ++id_Cost;
            gasCostsList.Add(costs);
            return await Task.FromResult(costs);
        }
        public async Task<GasBoiler?> AddBoilerDataAsync(GasBoiler boilerData)
        {
            boilerData.Id = ++id_BoilerData;
            gasBoilerDataList.Add(boilerData);
            return await Task.FromResult(boilerData);
        }
        public async Task<GasCounterChange?> AddCounterChangeAsync(GasCounterChange counterChange)
        {
            counterChange.Id = ++id_CounterChange;
            gasCounterChangeList.Add(counterChange);
            return await Task.FromResult(counterChange);
        }
        //public async Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff)
        //{
        //    eff.Id = ++id_EnergieEff;
        //    energieEffList.Add(eff);
        //    return await Task.FromResult(eff);
        //}
        #endregion

        #region DELETE
        public Task DeleteCounterAsync(GasCounter counter)
        {
            var result = gasCounterList.Find(m => m.ID_Tag == counter.ID_Tag);
            if (result != null)
            {
                gasCounterList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteSupplierAsync(GasTarif supplier)
        {
            var result = gasTarifList.Find(m => m.Id == supplier.Id);
            if (result != null)
            {
                gasTarifList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeletePaymentAsync(GasPayment payment)
        {
            var result = gasPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                gasPaymentList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCostAsync(GasCost costs)
        {
            var result = gasCostsList.Find(m => m.Id == costs.Id);
            if (result != null)
            {
                gasCostsList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteBoilerDataAsync(GasBoiler boilerData)
        {
            var result = gasBoilerDataList.Find(m => m.Id == boilerData.Id);
            if (result != null)
            {
                gasBoilerDataList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCounterChangeAsync(GasCounterChange counterChange)
        {
            var result = gasCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                gasCounterChangeList.Remove(result);
            }
            return Task.FromResult(result);
        }
        //public Task DeleteEnergieEfficiencyAsync(Efficiency eff)
        //{
        //    var result = energieEffList.Find(m => m.Id == eff.Id);
        //    if (result != null)
        //    {
        //        energieEffList.Remove(result);
        //    }
        //    return Task.FromResult(result);
        //}
        #endregion

        #region GET
        public async Task<GasCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = gasCounterList.Find(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = gasTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<IEnumerable<GasCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = gasCounterList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var supplier = gasTarifList.FindLast(m => m.Id == supplierId);
                foreach (var counter in result)
                {
                    counter.Anbieter = supplier!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }

        public async Task<GasCounter?> GetCounterByIdAsync(int id)
        {
            var counter = gasCounterList.Find(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = gasTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter!);
        }

        public async Task<IEnumerable<GasCounter?>> GetCountersAsync()
        {
            var counters = gasCounterList;
            var tarifList = gasTarifList;

            foreach (var counter in counters)
            {
                var tarif = tarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }

            return await Task.FromResult(counters);
        }
        public async Task<GasTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await Task.FromResult(gasTarifList.Find(m => m.Id == supplierId));
        }
        public async Task<IEnumerable<GasTarif?>> GetSuppliersAsync()
        {
            return await Task.FromResult(gasTarifList);
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsAsync()
        {
            return await Task.FromResult(gasPaymentList);
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = gasPaymentList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var payments = gasPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var payment in result)
                {
                    payment.Anbieter = payments!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }
        public async Task<GasPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = gasPaymentList.Find(m => m.ID_Zahlung == paymentId);

            return await Task.FromResult(payment);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await Task.FromResult(paymentMethodList);
        }
        public async Task<IEnumerable<GasCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = gasCostsList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var costs = gasPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var cost in result)
                {
                    cost.Anbieter = costs!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        public async Task<IEnumerable<GasCost?>> GetCostsAsync()
        {
            return await Task.FromResult(gasCostsList);
        }
        public async Task<GasCost?> GetCostByIdAsync(int costId)
        {
            var cost = gasCostsList.Find(m => m.Id == costId);

            return await Task.FromResult(cost);
        }
        public async Task<IEnumerable<GasBoiler?>> GetBoilerDataAsync()
        {
            return await Task.FromResult(gasBoilerDataList);
        }
        public async Task<GasBoiler?> GetBoilerDataByIdAsync(int id)
        {
            var boilerData = gasBoilerDataList.Find(m => m.Id == id);

            return await Task.FromResult(boilerData);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesAsync()
        {
            return await Task.FromResult(gasCounterChangeList);
        }
        public async Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterChange = gasCounterChangeList.Find(m => m.Id == counterChangeId);

            return await Task.FromResult(counterChange);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = gasCounterChangeList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var counterChanges = gasCounterChangeList.FindLast(m => m.Id_Anbieter == supplierId);
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = counterChanges!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        //public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync()
        //{
        //    return await Task.FromResult(energieEffList);
        //}
        //public async Task<Efficiency> GetEnergieEfficiencyByIdAsync(int id)
        //{
        //    var eff = energieEffList.Find(m => m.Id == id);

        //    return await Task.FromResult(eff);
        //}
        #endregion

        #region UPDATE
        public async Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand)
        {
            var result = gasCounterList.Find(m => m.ID_Tag == zaehlerstand.ID_Tag);
            if (result != null)
            {
                result.ID_Tag = zaehlerstand.ID_Tag;
                result.ID_Anbieter = zaehlerstand.ID_Anbieter;
                result.Ablesetag = zaehlerstand.Ablesetag;
                result.Zaehlerstand = zaehlerstand.Zaehlerstand;
                result.Temperatur_aussen = zaehlerstand.Temperatur_aussen;
                result.Temperatur_innen = zaehlerstand.Temperatur_innen;
                result.Bemerkungen = zaehlerstand.Bemerkungen;
            }
            return await Task.FromResult(result);
        }
        public async Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier)
        {
            var result = gasTarifList.Find(m => m.Id == supplier.Id);
            if (result != null)
            {
                result.Id = supplier.Id;
                result.Anbieter = supplier.Anbieter;
                result.Tarif = supplier.Tarif;
                result.Zaehlernummer = supplier.Zaehlernummer;
                result.Kuendigungsfrist = supplier.Kuendigungsfrist;
                result.Zeitraum_Start = supplier.Zeitraum_Start;
                result.Zeitraum_Ende = supplier.Zeitraum_Ende;
                result.Start_Zaehlerstand = supplier.Start_Zaehlerstand;
                result.Ende_Zaehlerstand = supplier.Ende_Zaehlerstand;
                result.Arbeitspreis = supplier.Arbeitspreis;
                result.Grundpreis = supplier.Grundpreis;
                result.Brennwert = supplier.Brennwert;
                result.Heizleistung = supplier.Heizleistung;
                result.Zustandszahl = supplier.Zustandszahl;
                result.Bemerkung = supplier.Bemerkung;
            }
            return await Task.FromResult(result);
        }
        public async Task<GasCost?> UpdateCostAsync(GasCost costs)
        {
            var result = gasCostsList.Find(m => m.Id == costs.Id);
            if (result != null)
            {
                result.Id = costs.Id;
                result.Id_Anbieter = costs.Id_Anbieter;
                result.Anbieter = costs.Anbieter;
                result.Grundpreis = costs.Grundpreis;
                result.Arbeitspreis = costs.Arbeitspreis;
                result.Zaehlermiete = costs.Zaehlermiete;
                result.Bemerkung = costs.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        public async Task<GasPayment?> UpdatePaymentAsync(GasPayment payment) // not used
        {
            var result = gasPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                result.ID_Zahlung = payment.ID_Zahlung;
                result.ID_Anbieter = payment.ID_Anbieter;
                result.Anbieter = payment.Anbieter;
                result.Datum = payment.Datum;
                result.Zahlungsart = payment.Zahlungsart;
                result.Bemerkungen = payment.Bemerkungen;
            }

            return await Task.FromResult(result);
        }
        public async Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData) // not used
        {
            var result = gasBoilerDataList.Find(m => m.Id == boilerData.Id);
            if (result != null)
            {
                result.Id = boilerData.Id;
                result.Verbrauchsjahr = boilerData.Verbrauchsjahr;
                result.Gesamt_Verbrauch = boilerData.Gesamt_Verbrauch;
                result.Heizung_Verbrauch = boilerData.Heizung_Verbrauch;
                result.Warmwasser_Verbrauch = boilerData.Warmwasser_Verbrauch;
                result.Strom_Verbrauch = boilerData.Strom_Verbrauch;
                result.Bemerkung = boilerData.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        public async Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange)
        {
            var result = gasCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Bemerkung = counterChange.Bemerkung;
            }

            return await Task.FromResult(result!);
        }
        //public async Task<Efficiency> UpdateEnergieEfficiencyAsync(Efficiency eff) // not used
        //{
        //    var result = energieEffList.Find(m => m.Id == eff.Id);
        //    if (result != null)
        //    {
        //        result.Id = eff.Id;
        //        result.Energieklasse = eff.Energieklasse;
        //        result.Energiebedarf = eff.Energiebedarf;
        //        result.Farbcode = eff.Farbcode;
        //        result.Bemerkung = eff.Bemerkung;
        //    }

        //    return await Task.FromResult(result);
        //}
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveChangesAsync()
        {
            var result = true;
            return Task.FromResult(result);
        }
        #endregion
    }
}