using GWS_Api.Models;
using GWS_Api.Models.Water;

namespace GWS_Api.Repositories.Water
{
    public class Mock_WaterRepository : IWaterRepository
    {
        #region Variablendeklaration
        int id_Tarif = 0;
        int id_Counter = 0;
        int id_Payment = 0;
        int id_Cost = 0;
        int id_CounterChange = 0;

        private static readonly List<WaterTarif> waterTarifList =
        [
            new ()
            {
                Id=11,
                Anbieter="Wasseranbieter1",
                Tarif="Standard-Wasser1",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_aussen=30,
                Ende_Zaehlerstand_aussen=40,
                Trinkwasserpreis=2.0,
                Verbrauch_Trinkwasser=100,
                Abwasserpreis=2.0,
                Verbrauch_Abwasser=100,
                Grundpreis=5,
                Zaehlermiete=1.2,
                Bemerkung="Wasser-Anbieter1"
            },
            new ()
            {
                Id=12,
                Anbieter="Wasseranbieter2",
                Tarif="Standard-Wasser2",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_aussen=30,
                Ende_Zaehlerstand_aussen=40,
                Trinkwasserpreis=2.0,
                Verbrauch_Trinkwasser=100,
                Abwasserpreis=2.0,
                Verbrauch_Abwasser=100,
                Grundpreis=5,
                Zaehlermiete=1.2,
                Bemerkung="Wasser-Anbieter2"
            },
            new ()
            {
                Id=13,
                Anbieter="Wasseranbieter3",
                Tarif="Standard-Wasser3",
                Zaehlernummer="12345",
                Kuendigungsfrist="6 Monate",
                Zeitraum_Start=DateTime.Now,
                Zeitraum_Ende=DateTime.Now,
                Start_Zaehlerstand=10,
                Ende_Zaehlerstand=20,
                Start_Zaehlerstand_aussen=30,
                Ende_Zaehlerstand_aussen=40,
                Trinkwasserpreis=2.0,
                Verbrauch_Trinkwasser=100,
                Abwasserpreis=2.0,
                Verbrauch_Abwasser=100,
                Grundpreis=5,
                Zaehlermiete=1.2,
                Bemerkung="Wasser-Anbieter3"
            },
        ];

        private static readonly List<WaterCounter> waterCounterList =
        [
            new ()
            {
                ID_Tag=1,
                ID_Anbieter=11,
                Ablesetag =new DateTime(2021,7,21),
                Zaehlerstand=1000,
                Zaehlerstand_aussen=5,
                Temperatur_aussen=20,
                Temperatur_innen=25,
                Bemerkungen=""
            },
            new ()
            {
                ID_Tag=2,
                ID_Anbieter=12,
                Ablesetag =new DateTime(2021,7,22),
                Zaehlerstand=2000,
                Zaehlerstand_aussen=10,
                Temperatur_aussen=21,
                Temperatur_innen=26,
                Bemerkungen=""
            },
            new ()
            {
                ID_Tag=3,
                ID_Anbieter=13,
                Ablesetag =new DateTime(2021,7,23),
                Zaehlerstand=2001,
                Zaehlerstand_aussen=11,
                Temperatur_aussen=22,
                Temperatur_innen=23,
                Bemerkungen="Zählerstand Anbieter=2"
            },
        ];

        private static readonly List<WaterPayment> waterPaymentList =
        [
            new ()
            {
                ID_Zahlung=1,
                ID_Anbieter=11,
                Anbieter="WasserAnbieter1",
                Datum=new DateTime(2021,5,1),
                Zahlungsart="Bonus",
                Zahlungen=91.0,
                Bemerkungen="Zahlung 1"
            },
            new ()
            {
                ID_Zahlung=2,
                ID_Anbieter=12,
                Anbieter="WasserAnbieter2",
                Datum=new DateTime(2021,2,1),
                Zahlungsart="Abschlag",
                Zahlungen=55.0,
                Bemerkungen="Zahlung 1"
            },
            new ()
            {
                ID_Zahlung=3,
                ID_Anbieter=13,
                Anbieter="WasserAnbieter3",
                Datum=new DateTime(2020,12,1),
                Zahlungsart="Abschlag",
                Zahlungen=77.0,
                Bemerkungen="Zahlung 1"
            },
            new ()
            {
                ID_Zahlung=4,
                ID_Anbieter=13,
                Anbieter="WasserAnbieter3",
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

        private static readonly List<WaterCost> waterCostsList =
        [
           new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="WasserAnbieter1",
                Gueltig_Ab=new DateTime(2000,1,1),
                Grundpreis=19.99d,
                Trinkwasserpreis=2.50d,
                Abwasserpreis=3.30d,
                Zaehlermiete=10.0d,
                Bemerkung="test1"
            },
            new ()
            {
                Id=2,
                Id_Anbieter=2,
                Anbieter="WasserAnbieter2",
                Gueltig_Ab=new DateTime(2020,7,27),
                Grundpreis=15.99d,
                Trinkwasserpreis=2.50d,
                Abwasserpreis=3.30d,
                Zaehlermiete=12.0d,
                Bemerkung="test2"
            },
             new ()
             {
                Id=3,
                Id_Anbieter=3,
                Anbieter="WasserAnbieter3",
                Gueltig_Ab=new DateTime(2022,8,12),
                Grundpreis=16.99d,
                Trinkwasserpreis=2.50d,
                Abwasserpreis=3.30d,
                Zaehlermiete=9.0d,
                Bemerkung="test3"
             }
        ];

        private static readonly List<WaterCounterChange> waterCounterChangeList =
      [
          new ()
            {
                Id=1,
                Id_Anbieter=1,
                Anbieter="WasserAnbieter1",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_aussen_alt=200.0d,
                Zaehlerstand_aussen_neu=300.0d,
                Bemerkung="test1"
            },
            new ()
            {
                Id=2,
                Id_Anbieter=2,
                Anbieter="WasserAnbieter2",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_aussen_alt=2000.0d,
                Zaehlerstand_aussen_neu=0.0d,
                Bemerkung="test2"
            },
             new ()
             {
                Id=3,
                Id_Anbieter=3,
                Anbieter="WasserAnbieter3",
                Wechsel_Datum=new DateTime(2000,1,1),
                Zaehlerstand_alt=100.0d,
                Zaehlerstand_neu=250.0d,
                Zaehlerstand_aussen_alt=2000.0d,
                Zaehlerstand_aussen_neu=0.0d,
                Bemerkung="test3"
             }
      ];
        #endregion
        public Mock_WaterRepository()
        {
            id_Tarif = waterTarifList.Count;
            id_Counter = waterCounterList.Count;
            id_Payment = waterPaymentList.Count;
            id_Cost = waterCostsList.Count;
        }

        #region ADD
        public async Task<WaterCounter?> AddCounterAsync(WaterCounter counter)
        {
            counter.ID_Tag = ++id_Counter;
            waterCounterList.Add(counter);
            // Name des Anbieters[id]
            var tarif = waterTarifList.Find(m => m.Id == counter.ID_Anbieter);
            if (tarif != null)
            {
                counter.Anbieter = tarif.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<WaterTarif> AddSupplierAsync(WaterTarif supplier)
        {
            supplier.Id = ++id_Tarif;
            waterTarifList.Add(supplier);
            return await Task.FromResult(supplier);
        }
        public async Task<WaterPayment> AddPaymentAsync(WaterPayment payment)
        {
            payment.ID_Zahlung = ++id_Payment;
            waterPaymentList.Add(payment);
            return await Task.FromResult(payment);
        }
        public async Task<WaterCost> AddCostAsync(WaterCost cost)
        {
            cost.Id = ++id_Cost;
            waterCostsList.Add(cost);
            return await Task.FromResult(cost);
        }
        public async Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange)
        {
            counterChange.Id = ++id_CounterChange;
            waterCounterChangeList.Add(counterChange);
            return await Task.FromResult(counterChange);
        }
        #endregion

        #region DELETE
        public Task DeleteCounterAsync(WaterCounter counter)
        {
            var result = waterCounterList.Find(m => m.ID_Tag == counter.ID_Tag);
            if (result != null)
            {
                waterCounterList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteSupplierAsync(WaterTarif supplier)
        {
            var result = waterTarifList.Find(m => m.Id == supplier.Id);
            if (result != null)
            {
                waterTarifList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeletePaymentAsync(WaterPayment payment)
        {
            var result = waterPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                waterPaymentList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCostAsync(WaterCost cost)
        {
            var result = waterCostsList.Find(m => m.Id == cost.Id);
            if (result != null)
            {
                waterCostsList.Remove(result);
            }
            return Task.FromResult(result);
        }
        public Task DeleteCounterChangeAsync(WaterCounterChange counterChange)
        {
            var result = waterCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                waterCounterChangeList.Remove(result);
            }
            return Task.FromResult(result);
        }
        #endregion

        #region GET
        public async Task<WaterCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = waterCounterList.Find(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = waterTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter!);
        }

        public async Task<IEnumerable<WaterCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = waterCounterList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var supplier = waterTarifList.FindLast(m => m.Id == supplierId);
                foreach (var counter in result)
                {
                    counter.Anbieter = supplier!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }

        public async Task<WaterCounter?> GetCounterByIdAsync(int id)
        {
            var counter = waterCounterList.Find(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = waterTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter!);
        }

        public async Task<IEnumerable<WaterCounter?>> GetCountersAsync()
        {
            var counters = waterCounterList;
            var tarifList = waterTarifList;
            WaterTarif tarif = new();
            foreach (var counter in counters)
            {
                tarif = tarifList.FindLast(m => m.Id == counter.ID_Anbieter)!;
                counter.Anbieter = tarif.Anbieter;
            }

            return await Task.FromResult(counters);
        }
        public async Task<WaterTarif?> GetSupplierByIdAsync(int supplierId) => await Task.FromResult(waterTarifList.Find(m => m.Id == supplierId)!);
        public async Task<IEnumerable<WaterTarif>> GetSuppliersAsync()
        {
            return await Task.FromResult(waterTarifList);
        }
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsAsync()
        {
            return await Task.FromResult(waterPaymentList);
        }
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = waterPaymentList.FindAll(m => m.ID_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var payments = waterPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var payment in result)
                {
                    payment.Anbieter = payments!.Anbieter;
                }
            }
            return await Task.FromResult(result);
        }
        public async Task<WaterPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = waterPaymentList.Find(m => m.ID_Zahlung == paymentId);

            return await Task.FromResult(payment!);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await Task.FromResult(paymentMethodList);
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = waterCostsList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var costs = waterPaymentList.FindLast(m => m.ID_Anbieter == supplierId);
                foreach (var cost in result)
                {
                    cost.Anbieter = costs!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsAsync()
        {
            return await Task.FromResult(waterCostsList);
        }
        public async Task<WaterCost?> GetCostByIdAsync(int costId)
        {
            var cost = waterCostsList.Find(m => m.Id == costId);

            return await Task.FromResult(cost!);
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesAsync()
        {
            return await Task.FromResult(waterCounterChangeList);
        }
        public async Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterChange = waterCounterChangeList.Find(m => m.Id == counterChangeId);

            return await Task.FromResult(counterChange!);
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = waterCounterChangeList.FindAll(m => m.Id_Anbieter == supplierId);
            if (result.Count > 0)
            {
                var counterChanges = waterCounterChangeList.FindLast(m => m.Id_Anbieter == supplierId);
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = counterChanges!.Anbieter;
                }
            }

            return await Task.FromResult(result);
        }
        #endregion

        #region UPDATE
        public async Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand)
        {
            var result = waterCounterList.Find(m => m.ID_Tag == zaehlerstand.ID_Tag);
            if (result != null)
            {
                result.ID_Tag = zaehlerstand.ID_Tag;
                result.ID_Anbieter = zaehlerstand.ID_Anbieter;
                result.Ablesetag = zaehlerstand.Ablesetag;
                result.Zaehlerstand = zaehlerstand.Zaehlerstand;
                result.Zaehlerstand_aussen = zaehlerstand.Zaehlerstand_aussen;
                result.Temperatur_aussen = zaehlerstand.Temperatur_aussen;
                result.Temperatur_innen = zaehlerstand.Temperatur_innen;
                result.Bemerkungen = zaehlerstand.Bemerkungen;
            }
            return await Task.FromResult(result!);
        }
        public async Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier)
        {
            var result = waterTarifList.Find(m => m.Id == supplier.Id);
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
                result.Start_Zaehlerstand_aussen = supplier.Start_Zaehlerstand_aussen;
                result.Ende_Zaehlerstand_aussen = supplier.Ende_Zaehlerstand_aussen;
                result.Trinkwasserpreis = supplier.Trinkwasserpreis;
                result.Verbrauch_Abwasser = supplier.Verbrauch_Abwasser;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Grundpreis;
                result.Bemerkung = supplier.Bemerkung;
            }
            return await Task.FromResult(result!);
        }
        public async Task<WaterCost?> UpdateCostAsync(WaterCost cost)
        {
            var result = waterCostsList.Find(m => m.Id == cost.Id);
            if (result != null)
            {
                result.Id = cost.Id;
                result.Id_Anbieter = cost.Id_Anbieter;
                result.Anbieter = cost.Anbieter;
                result.Grundpreis = cost.Grundpreis;
                result.Trinkwasserpreis = cost.Trinkwasserpreis;
                result.Abwasserpreis = cost.Abwasserpreis;
                result.Zaehlermiete = cost.Zaehlermiete;
                result.Bemerkung = cost.Bemerkung;
            }

            return await Task.FromResult(result!);
        }
        public async Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment)
        {
            var result = waterPaymentList.Find(m => m.ID_Zahlung == payment.ID_Zahlung);
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
        public async Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange)
        {
            var result = waterCounterChangeList.Find(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Zaehlerstand_aussen_alt = counterChange.Zaehlerstand_aussen_alt;
                result.Zaehlerstand_aussen_neu = counterChange.Zaehlerstand_aussen_neu;
                result.Bemerkung = counterChange.Bemerkung;
            }

            return await Task.FromResult(result!);
        }
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            var result = true;
            return await Task.FromResult(result);
        }
        #endregion
    }
}
