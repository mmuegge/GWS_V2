using GWS_Api.Models;
using GWS_Api.Models.Water;

namespace GWS_Api.Repositories.Water
{
    public class Mock_WaterRepository : IWaterRepository
    {
        #region Variablendeklaration
        int id_Tarif = 100;
        int id_Counter = 100;
        int id_Payment = 100;
        int id_Cost = 100;
        int id_CounterChange = 100;

        private readonly Lock _lock = new();

        private readonly List<WaterTarif> _waterTarifList =
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

        private readonly List<WaterCounter> _waterCounterList =
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

        private readonly List<WaterPayment> _waterPaymentList =
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

        private readonly List<PaymentMethod> _waterPaymentMethodList =
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

        private readonly List<WaterCost> _waterCostsList =
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

        private  readonly List<WaterCounterChange> _waterCounterChangeList =
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
            id_Tarif = _waterTarifList.Count;
            id_Counter = _waterCounterList.Count;
            id_Payment = _waterPaymentList.Count;
            id_Cost = _waterCostsList.Count;
            id_CounterChange = _waterCounterChangeList.Count;
        }

        #region ADD
        #region AddCounterAsync
        public Task<WaterCounter?> AddCounterAsync(WaterCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounter?>(cancellationToken);
            }

            var ablesetagDate = counter.Ablesetag?.Date;
            if (ablesetagDate is null || ablesetagDate > DateTime.Today)
            {
                return Task.FromResult<WaterCounter?>(null);
            }


            lock (_lock)
            {
                bool exists = _waterCounterList.Exists(m =>
                    m.Ablesetag?.Date == ablesetagDate &&
                    m.ID_Anbieter == counter.ID_Anbieter);

                if (exists)
                {
                    return Task.FromResult<WaterCounter?>(null);
                }

                var tarif = _waterTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                if (tarif is not null)
                {
                    counter.Anbieter = tarif.Anbieter ?? string.Empty;
                }

                counter.ID_Tag = ++id_Counter;
                counter.Ablesetag = ablesetagDate;

                _waterCounterList.Add(counter);
            }

            return Task.FromResult<WaterCounter?>(counter);
        }
        #endregion

        #region AddSupplierAsync
        public Task<WaterTarif> AddSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterTarif>(cancellationToken);
            }

            lock (_lock)
            {
                supplier.Id = ++id_Tarif;
                _waterTarifList.Add(supplier);
            }

            return Task.FromResult(supplier);
        }
        #endregion

        #region AddPaymentAsync
        public Task<WaterPayment> AddPaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterPayment>(cancellationToken);
            }

            lock (_lock)
            {
                payment.ID_Zahlung = ++id_Payment;
                _waterPaymentList.Add(payment);
            }

            return Task.FromResult(payment);
        }
        #endregion

        #region AddCostAsync
        public Task<WaterCost> AddCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCost>(cancellationToken);
            }

            lock (_lock)
            {
                cost.Id = ++id_Cost;
                _waterCostsList.Add(cost);
            }

            return Task.FromResult(cost);
        }
        #endregion

        #region AddCounterChangeAsync
        public Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounterChange>(cancellationToken);
            }

            lock (_lock)
            {
                counterChange.Id = ++id_CounterChange;
                _waterCounterChangeList.Add(counterChange);
            }

            return Task.FromResult(counterChange);

        }
        #endregion
        #endregion

        #region DELETE
        #region DeleteCounterAsync
        public Task DeleteCounterAsync(WaterCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _waterCounterList.RemoveAll(m => m.ID_Tag == counter.ID_Tag);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteSupplierAsync
        public Task DeleteSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _waterCostsList.RemoveAll(m => m.Id == supplier.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeletePaymentAsync
        public Task DeletePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _waterCostsList.RemoveAll(m => m.Id == payment.ID_Zahlung);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCostAsync
        public Task DeleteCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _waterCostsList.RemoveAll(m => m.Id == cost.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCounterChangeAsync
        public Task DeleteCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _waterCounterChangeList.RemoveAll(m => m.Id == counterChange.Id);
            }

            return Task.CompletedTask;
        }
        #endregion
        #endregion

        #region GET
        #region GetCounterByDateAsync
        public Task<WaterCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounter?>(cancellationToken);
            }

            lock (_lock)
            {
                var counter = _waterCounterList.Find(m => m.Ablesetag?.Date == date.Date);
                if (counter is not null)
                {
                    var tarif = _waterTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                    if (tarif is not null)
                    {
                        counter.Anbieter = tarif.Anbieter ?? string.Empty;
                    }
                }

                return Task.FromResult(counter);
            }
        }
        #endregion

        #region GetCountersBySupplierAsync
        public Task<IEnumerable<WaterCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterCounterList.FindAll(m => m.ID_Anbieter == supplierId);
                if (result.Count > 0)
                {
                    var supplier = _waterTarifList.FindLast(m => m.Id == supplierId);
                    if (supplier is not null)
                    {
                        foreach (var counter in result)
                        {
                            counter.Anbieter = supplier.Anbieter ?? string.Empty;
                        }
                    }
                }

                return Task.FromResult<IEnumerable<WaterCounter>>(result);
            }
        }
        #endregion

        #region GetCounterByIdAsync
        public Task<WaterCounter?> GetCounterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounter?>(cancellationToken);
            }

            lock (_lock)
            {
                var counter = _waterCounterList.Find(m => m.ID_Tag == id);
                if (counter is not null)
                {
                    var tarif = _waterTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                    if (tarif is not null)
                    {
                        counter.Anbieter = tarif.Anbieter ?? string.Empty;
                    }
                }

                return Task.FromResult(counter);
            }
        }
        #endregion

        #region GetCountersAsync
        public Task<IEnumerable<WaterCounter>> GetCountersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var anbieterById = _waterTarifList
                .GroupBy(t => t.Id)
                .ToDictionary(g => g.Key, g => g.Last().Anbieter);

                foreach (var counter in _waterCounterList)
                {
                    if (anbieterById.TryGetValue(counter.ID_Anbieter, out var anbieter))
                    {
                        counter.Anbieter = anbieter ?? string.Empty;
                    }
                }

                return Task.FromResult<IEnumerable<WaterCounter>>([.. _waterCounterList]);
            }
        }
        #endregion

        #region GetSupplierByIdAsync
        public Task<WaterTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterTarif?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterTarifList.Find(m => m.Id == supplierId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetSuppliersAsync
        public Task<IEnumerable<WaterTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterTarif>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<WaterTarif>>([.. _waterTarifList]);
            }
        }
        #endregion

        #region GetPaymentsAsync
        public Task<IEnumerable<WaterPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterPayment>>(cancellationToken);
            }

            lock (_lock)
            {

                return Task.FromResult<IEnumerable<WaterPayment>>([.. _waterPaymentList]);
            }
        }
        #endregion

        #region GetPaymentsBySupplierAsync
        public Task<IEnumerable<WaterPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterPayment>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterPaymentList
                .Where(m => m.ID_Anbieter == supplierId)
                .ToList();

                var anbieter = result.LastOrDefault(m => m.Anbieter is not null)?.Anbieter;

                if (anbieter is not null)
                {
                    foreach (var payment in result)
                    {
                        payment.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<WaterPayment>>(result);
            }
        }
        #endregion

        #region GetPaymentByIdAsync
        public Task<WaterPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterPayment?>(cancellationToken);
            }

            lock (_lock)
            {
                var payment = _waterPaymentList.Find(m => m.ID_Zahlung == paymentId);
                return Task.FromResult<WaterPayment?>(payment);
            }
        }
        #endregion

        #region GetPaymentMethodsAsync
        public Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<PaymentMethod>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<PaymentMethod>>([.. _waterPaymentMethodList]);
            }
        }
        #endregion

        #region GetCostsBySupplierIdAsync
        public Task<IEnumerable<WaterCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCost>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterCostsList
                .Where(m => m.Id_Anbieter == supplierId)
                .ToList();

                var anbieter = result.LastOrDefault(m => m.Anbieter is not null)?.Anbieter;

                if (anbieter is not null)
                {
                    foreach (var cost in result)
                    {
                        cost.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<WaterCost>>(result);
            }
        }
        #endregion

        #region GetCostsAsync
        public Task<IEnumerable<WaterCost>> GetCostsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCost>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<WaterCost>>([.. _waterCostsList]);
            }
        }
        #endregion

        #region GetCostByIdAsync
        public Task<WaterCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCost?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterCostsList.FirstOrDefault(m => m.Id == costId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetCounterChangesAsync
        public Task<IEnumerable<WaterCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<WaterCounterChange>>([.. _waterCounterChangeList]);
            }
        }
        #endregion

        #region GetCounterChangeByIdAsync
        public Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounterChange?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterCounterChangeList.FirstOrDefault(m => m.Id == counterChangeId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetCounterChangesBySupplierAsync
        public Task<IEnumerable<WaterCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<WaterCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _waterCounterChangeList
                    .Where(m => m?.Id_Anbieter == supplierId)
                    .ToList();

                // Aktuellen Anbieternamen frisch aus der Tarifliste laden
                var tarif = _waterTarifList.FindLast(m => m.Id == supplierId);

                if (tarif is not null)
                {
                    var anbieter = tarif.Anbieter ?? string.Empty;
                    foreach (var counterChange in result)
                    {
                        counterChange.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<WaterCounterChange>>(result);
            }
        }
        #endregion
        #endregion

        #region UPDATE (Put)
        #region UpdateCounterAsync
        public Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounter?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _waterCounterList.FindIndex(m => m.ID_Tag == zaehlerstand.ID_Tag);

                if (index == -1)
                {
                    return Task.FromResult<WaterCounter?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _waterCounterList[index] = zaehlerstand;

                return Task.FromResult<WaterCounter?>(zaehlerstand);
            }
        }
        #endregion

        #region UpdateSupplierAsync
        public Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterTarif?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _waterTarifList.FindIndex(m => m.Id == supplier.Id);

                if (index == -1)
                {
                    return Task.FromResult<WaterTarif?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _waterTarifList[index] = supplier;

                return Task.FromResult<WaterTarif?>(supplier);
            }
        }
        #endregion

        #region UpdatePaymentAsync
        public Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterPayment?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _waterPaymentList.FindIndex(m => m.ID_Zahlung == payment.ID_Zahlung);

                if (index == -1)
                {
                    return Task.FromResult<WaterPayment?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _waterPaymentList[index] = payment;

                return Task.FromResult<WaterPayment?>(payment);
            }
        }
        #endregion

        #region UpdateCostAsync
        public Task<WaterCost?> UpdateCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCost?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _waterCostsList.FindIndex(m => m.Id == cost.Id);

                if (index == -1)
                {
                    return Task.FromResult<WaterCost?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _waterCostsList[index] = cost;

                return Task.FromResult<WaterCost?>(cost);
            }
        }
        #endregion

        #region UpdateCounterChangeAsync
        public Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WaterCounterChange?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _waterCounterChangeList.FindIndex(m => m.Id == counterChange.Id);

                if (index == -1)
                {
                    return Task.FromResult<WaterCounterChange?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _waterCounterChangeList[index] = counterChange;

                return Task.FromResult<WaterCounterChange?>(counterChange);
            }
        }
        #endregion
        #endregion

        #region SaveChangesAsync
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            return Task.FromResult(true);
        }
        #endregion
    }
}
