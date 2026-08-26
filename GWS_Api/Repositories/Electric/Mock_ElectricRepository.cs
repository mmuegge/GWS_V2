using GWS_Api.Models;
using GWS_Api.Models.Electric;

namespace GWS_Api.Repositories.Electric
{
    public class Mock_ElectricRepository : IElectricRepository
    {
        #region Variablendeklaration
        int id_Tarif = 0;
        int id_Counter = 0;
        int id_Payment = 0;
        int id_Costs = 0;
        int id_CounterChange = 0;

        private static readonly List<ElectricTarif> electricTarifList =
        [
            new ()
            {
                Id=11,
                Anbieter="ElectricAnbieter1",
                Tarif="Standard-Electric1",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_280=30,
                Ende_Zaehlerstand_280=40,
                Start_Zaehlerstand_Enfluri=50,
                Ende_Zaehlerstand_Enfluri=60,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Anzahl_Personen=2,
                Bemerkung="Electric-Anbieter1"
            },
            new ()
            {
                Id=12,
                Anbieter="ElectricAnbieter2",
                Tarif="Standard-Electric2",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_280=30,
                Ende_Zaehlerstand_280=40,
                Start_Zaehlerstand_Enfluri=50,
                Ende_Zaehlerstand_Enfluri=60,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Anzahl_Personen=2,
                Bemerkung="Electric-Anbieter2"
            },
            new ()
            {
                Id=13,
                Anbieter="ElectricAnbieter3",
                Tarif="Standard-Electric3",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_280=30,
                Ende_Zaehlerstand_280=40,
                Start_Zaehlerstand_Enfluri=50,
                Ende_Zaehlerstand_Enfluri=60,
                Arbeitspreis=2.0,
                Grundpreis=100,
                Zaehlermiete=1.2,
                Anzahl_Personen=2,
                Bemerkung="Electric-Anbieter3"
            },
        ];

        private static readonly List<ElectricCounter> electricCounterList =
        [
            new ()
            {
                ID_Tag=1,
                ID_Anbieter=11,
                Ablesetag =new DateTime(2021,7,21),
                Zaehlerstand=1000,
                Temperatur_aussen=20,
                Temperatur_innen=25,
                Bemerkungen="Electric-Anbieter1"
            },
            new ()
            {
                ID_Tag=2,
                ID_Anbieter=12,
                Ablesetag =new DateTime(2021,7,22),
                Zaehlerstand=2000,
                Temperatur_aussen=21,
                Temperatur_innen=26,
                Bemerkungen="Electric-Anbieter2"
            },
            new ()
            {
                ID_Tag=3,
                ID_Anbieter=13,
                Ablesetag =new DateTime(2021,7,23),
                Zaehlerstand=2001,
                Temperatur_aussen=22,
                Temperatur_innen=23,
                Bemerkungen="Electric-Anbieter3"
            },
        ];

        private static readonly List<ElectricPayment> electricPaymentList =
        [
            new ()
            {
                ID_Zahlung=1,
                ID_Anbieter=11,
                Anbieter="StromsAnbieter1",
                Datum=new DateTime(2021,5,1),
                Zahlungsart="Bonus",
                Zahlungen=91.0,
                Bemerkungen="Zahlung 1"
            },
            new ()
            {
                ID_Zahlung=2,
                ID_Anbieter=12,
                Anbieter="StromAnbieter2",
                Datum=new DateTime(2021,2,1),
                Zahlungsart="Abschlag",
                Zahlungen=55.0,
                Bemerkungen="Zahlung 2"
            },
            new ()
            {
                ID_Zahlung=3,
                ID_Anbieter=13,
                Anbieter="StromAnbieter3",
                Datum=new DateTime(2020,12,1),
                Zahlungsart="Abschlag",
                Zahlungen=77.0,
                Bemerkungen="Zahlung 3"
            },
            new ()
            {
                ID_Zahlung=4,
                ID_Anbieter=13,
                Anbieter="StromAnbieter3",
                Datum=new DateTime(2020,12,1),
                Zahlungsart="Bonus",
                Zahlungen=91.0,
                Bemerkungen="Zahlung 2"
            },
        ];
        private static readonly List<PaymentMethod> electricPaymentMethodList =
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
        private static readonly List<ElectricCost> electricCostsList =
        [
            new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="StromAnbieter1",
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
                Anbieter="StromAnbieter2",
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
                Anbieter="StromAnbieter3",
                Gueltig_Ab=new DateTime(2022,8,12),
                Grundpreis=16.99d,
                Arbeitspreis=15.00d,
                Zaehlermiete=9.0d,
                Bemerkung="test3"
             }
        ];
        private static readonly List<ElectricCounterChange> electricCounterChangeList =
      [
          new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="StromAnbieter1",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_280_alt=200.0d,
                Zaehlerstand_280_neu=350.0d,
                Zaehlerstand_Enfluri_alt=2000.0d,
                Zaehlerstand_Enfluri_neu=0.0d,
                Bemerkung="test1"
            },
            new ()
            {
                Id=2,
                Id_Anbieter=2,
                Anbieter="StromAnbieter2",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_280_alt=200.0d,
                Zaehlerstand_280_neu=350.0d,
                Zaehlerstand_Enfluri_alt=2000.0d,
                Zaehlerstand_Enfluri_neu=0.0d,
                Bemerkung="test2"
            },
             new ()
             {
                Id=3,
                Id_Anbieter=3,
                Anbieter="StromAnbieter3",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_280_alt=200.0d,
                Zaehlerstand_280_neu=350.0d,
                Zaehlerstand_Enfluri_alt=2000.0d,
                Zaehlerstand_Enfluri_neu=0.0d,
                Bemerkung="test3"
             }
      ];
        #endregion
        public Mock_ElectricRepository()
        {
            id_Tarif = electricTarifList.Count;
            id_Counter = electricCounterList.Count;
            id_Payment = electricPaymentList.Count;
            id_Costs = electricCostsList.Count;
            id_CounterChange = electricCounterChangeList.Count;
        }

        #region ADD
        public async Task<ElectricCounter?> AddCounterAsync(ElectricCounter counter)
        {
            counter.ID_Tag = ++id_Counter;
            electricCounterList.Add(counter);
            // Name des Anbieters[id]
            var tarif = electricTarifList.Find(m => m.Id == counter.ID_Anbieter);
            if (tarif != null)
            {
                counter.Anbieter = tarif.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<ElectricTarif?> AddSupplierAsync(ElectricTarif supplier)
        {
            supplier.Id = ++id_Tarif;
            electricTarifList.Add(supplier);
            return await Task.FromResult(supplier);
        }
        public async Task<ElectricPayment?> AddPaymentAsync(ElectricPayment payment)
        {
            payment.ID_Zahlung = ++id_Payment;
            electricPaymentList.Add(payment);
            return await Task.FromResult(payment);
        }
        public async Task<ElectricCost?> AddCostAsync(ElectricCost costs)
        {
            costs.Id = ++id_Costs;
            electricCostsList.Add(costs);
            return await Task.FromResult(costs);
        }
        public async Task<ElectricCounterChange?> AddCounterChangeAsync(ElectricCounterChange counterChange)
        {
            counterChange.Id = ++id_CounterChange;
            electricCounterChangeList.Add(counterChange);
            return await Task.FromResult(counterChange);
        }
        #endregion

        #region DELETE
        public Task DeleteCounterAsync(ElectricCounter counter)
        {
            var result = electricCounterList.Find(m => m.ID_Tag == counter.ID_Tag);
            if (result != null)
            {
                electricCounterList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteSupplierAsync(ElectricTarif supplier)
        {
            var result = electricTarifList.Find(m => m.Id == supplier.Id);
            if (result != null)
            {
                electricTarifList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeletePaymentAsync(ElectricPayment payment)
        {
            var result = electricPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                electricPaymentList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCostAsync(ElectricCost costs)
        {
            var result = electricCostsList.Find(m => m.Id == costs.Id);
            if (result != null)
            {
                electricCostsList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCounterChangeAsync(ElectricCounterChange counterChange)
        {
            var result = electricCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                electricCounterChangeList.Remove(result);
            }
            return Task.FromResult(result);
        }
        #endregion

        #region GET
        public async Task<ElectricCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = electricCounterList.Find(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = electricTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter!);
        }
        public async Task<IEnumerable<ElectricCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = electricCounterList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var supplier = electricTarifList.FindLast(m => m.Id == supplierId);
                foreach (var counter in result)
                {
                    counter.Anbieter = supplier!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }

        public async Task<ElectricCounter?> GetCounterByIdAsync(int id)
        {
            var counter = electricCounterList.Find(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = electricTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter!);
        }

        public async Task<IEnumerable<ElectricCounter?>> GetCountersAsync()
        {
            var counters = electricCounterList;
            var tarifList = electricTarifList;
            ElectricTarif tarif = new();
            foreach (var counter in counters)
            {
                tarif = tarifList.FindLast(m => m.Id == counter.ID_Anbieter)!;
                counter.Anbieter = tarif.Anbieter;
            }

            return await Task.FromResult(counters);
        }
        public async Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await Task.FromResult(electricTarifList.Find(m => m.Id == supplierId)!);
        }
        public async Task<IEnumerable<ElectricTarif?>> GetSuppliersAsync()
        {
            return await Task.FromResult(electricTarifList);
        }
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsAsync()
        {
            return await Task.FromResult(electricPaymentList);
        }
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = electricPaymentList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var payments = electricPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var payment in result)
                {
                    payment.Anbieter = payments!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }
        public async Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = electricPaymentList.Find(m => m.ID_Zahlung == paymentId);

            return await Task.FromResult(payment!);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await Task.FromResult(electricPaymentMethodList);
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = electricCostsList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var costs = electricPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var cost in result)
                {
                    cost.Anbieter = costs!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsAsync()
        {
            return await Task.FromResult(electricCostsList);
        }
        public async Task<ElectricCost?> GetCostByIdAsync(int costId)
        {
            var cost = electricCostsList.Find(m => m.Id == costId);

            return await Task.FromResult(cost!);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesAsync()
        {
            return await Task.FromResult(electricCounterChangeList);
        }
        public async Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterChange = electricCounterChangeList.Find(m => m.Id == counterChangeId);

            return await Task.FromResult(counterChange!);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = electricCounterChangeList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var counterChanges = electricCounterChangeList.FindLast(m => m.Id_Anbieter == supplierId);
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = counterChanges!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        #endregion

        #region UPDATE
        public async Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand)
        {
            var result = electricCounterList.Find(m => m.ID_Tag == zaehlerstand.ID_Tag);
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
            return await Task.FromResult(result!);
        }
        public async Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier)
        {
            var result = electricTarifList.Find(m => m.Id == supplier.Id);
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
                result.Start_Zaehlerstand_280 = supplier.Start_Zaehlerstand_280;
                result.Ende_Zaehlerstand_280 = supplier.Ende_Zaehlerstand_280;
                result.Start_Zaehlerstand_Enfluri = supplier.Start_Zaehlerstand_Enfluri;
                result.Ende_Zaehlerstand_Enfluri = supplier.Ende_Zaehlerstand_Enfluri;
                result.Arbeitspreis = supplier.Arbeitspreis;
                result.Grundpreis = supplier.Grundpreis;
                result.Anzahl_Personen = supplier.Anzahl_Personen;
                result.Bemerkung = supplier.Bemerkung;
            }
            return await Task.FromResult(result!);
        }
        public async Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment)
        {
            var result = electricPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                result.ID_Zahlung = payment.ID_Zahlung;
                result.ID_Anbieter = payment.ID_Anbieter;
                result.Anbieter = payment.Anbieter;
                result.Datum = payment.Datum;
                result.Zahlungsart = payment.Zahlungsart;
                result.Bemerkungen = payment.Bemerkungen;
            }

            return await Task.FromResult(result!);
        }
        public async Task<ElectricCost?> UpdateCostAsync(ElectricCost costs)
        {
            var result = electricCostsList.Find(m => m.Id == costs.Id);
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

            return await Task.FromResult(result!);
        }
        public async Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange)
        {
            var result = electricCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Zaehlerstand_Enfluri_alt = counterChange.Zaehlerstand_Enfluri_alt;
                result.Zaehlerstand_Enfluri_neu = counterChange.Zaehlerstand_Enfluri_neu;
                result.Bemerkung = counterChange.Bemerkung;
            }

            return await Task.FromResult(result!);
        }
        #endregion

        public Task<bool> SaveChangesAsync()
        {
            var result = true;
            return Task.FromResult(result);
        }

    }
}