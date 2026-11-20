using GWS_Api.Models;
using GWS_Api.Models.Electric;

namespace GWS_Api.Repositories.Electric
{
    public class Mock_ElectricRepository : IElectricRepository
    {
        #region Variablendeklaration
        private int id_Tarif = 100;
        private int id_Counter = 100;
        private int id_Payment = 100;
        private int id_Costs = 100;
        private int id_CounterChange = 100;

        private readonly Lock _lock = new();

        private readonly List<ElectricTarif> _electricTarifList =
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

        private readonly List<ElectricCounter> _electricCounterList =
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

        private readonly List<ElectricPayment> _electricPaymentList =
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
        private readonly List<PaymentMethod> _electricPaymentMethodList =
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
        private readonly List<ElectricCost> _electricCostsList =
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
        private readonly List<ElectricCounterChange> _electricCounterChangeList =
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
            id_Tarif = _electricTarifList.Count;
            id_Counter = _electricCounterList.Count;
            id_Payment = _electricPaymentList.Count;
            id_Costs = _electricCostsList.Count;
            id_CounterChange = _electricCounterChangeList.Count;
        }

        #region ADD
        #region AddCounterAsync
        public Task<ElectricCounter?> AddCounterAsync(ElectricCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounter?>(cancellationToken);
            }

            var ablesetagDate = counter.Ablesetag?.Date;
            if (ablesetagDate is null || ablesetagDate > DateTime.Today)
            {
                return Task.FromResult<ElectricCounter?>(null);
            }


            lock (_lock)
            {
                bool exists = _electricCounterList.Exists(m =>
                    m.Ablesetag?.Date == ablesetagDate &&
                    m.ID_Anbieter == counter.ID_Anbieter);

                if (exists)
                {
                    return Task.FromResult<ElectricCounter?>(null);
                }

                var tarif = _electricTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                if (tarif is not null)
                {
                    counter.Anbieter = tarif.Anbieter ?? string.Empty;
                }

                counter.ID_Tag = ++id_Counter;
                counter.Ablesetag = ablesetagDate;

                _electricCounterList.Add(counter);
            }

            return Task.FromResult<ElectricCounter?>(counter);
        }
        #endregion

        #region AddSupplierAsync
        public Task<ElectricTarif> AddSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricTarif>(cancellationToken);
            }

            lock (_lock)
            {
                supplier.Id = ++id_Tarif;
                _electricTarifList.Add(supplier);
            }

            return Task.FromResult(supplier);
        }
        #endregion

        #region AddPaymentAsync
        public Task<ElectricPayment> AddPaymentAsync(ElectricPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricPayment>(cancellationToken);
            }

            lock (_lock)
            {
                payment.ID_Zahlung = ++id_Payment;
                _electricPaymentList.Add(payment);
            }

            return Task.FromResult(payment);
        }
        #endregion

        #region AddCostAsync
        public Task<ElectricCost> AddCostAsync(ElectricCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCost>(cancellationToken);
            }

            lock (_lock)
            {
                cost.Id = ++id_Costs;
                _electricCostsList.Add(cost);
            }

            return Task.FromResult(cost);
        }
        #endregion

        #region AddCounterChangeAsync
        public Task<ElectricCounterChange> AddCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounterChange>(cancellationToken);
            }

            lock (_lock)
            {
                counterChange.Id = ++id_CounterChange;
                _electricCounterChangeList.Add(counterChange);
            }

            return Task.FromResult(counterChange);
        }
        #endregion
        #endregion

        #region DELETE
        #region DeleteCounterAsync
        public Task DeleteCounterAsync(ElectricCounter counter, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _electricCounterList.RemoveAll(m => m.ID_Tag == counter.ID_Tag);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteSupplierAsync
        public Task DeleteSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _electricCostsList.RemoveAll(m => m.Id == supplier.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeletePaymentAsync
        public Task DeletePaymentAsync(ElectricPayment payment, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _electricCostsList.RemoveAll(m => m.Id == payment.ID_Zahlung);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCostAsync
        public Task DeleteCostAsync(ElectricCost cost, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _electricCostsList.RemoveAll(m => m.Id == cost.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCounterChangeAsync
        public Task DeleteCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _electricCounterChangeList.RemoveAll(m => m.Id == counterChange.Id);
            }

            return Task.CompletedTask;
        }
        #endregion
        #endregion

        #region GET
        #region GetCounterByDateAsync
        public Task<ElectricCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounter?>(cancellationToken);
            }

            lock (_lock)
            {
                var counter = _electricCounterList.Find(m => m.Ablesetag?.Date == date.Date);
                if (counter is not null)
                {
                    var tarif = _electricTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
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
        public Task<IEnumerable<ElectricCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricCounterList.FindAll(m => m.ID_Anbieter == supplierId);
                if (result.Count > 0)
                {
                    var supplier = _electricTarifList.FindLast(m => m.Id == supplierId);
                    if (supplier is not null)
                    {
                        foreach (var counter in result)
                        {
                            counter.Anbieter = supplier.Anbieter ?? string.Empty;
                        }
                    }
                }

                return Task.FromResult<IEnumerable<ElectricCounter>>(result);
            }
        }
        #endregion

        #region GetCounterByIdAsync
        public Task<ElectricCounter?> GetCounterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounter?>(cancellationToken);
            }

            lock (_lock)
            {
                var counter = _electricCounterList.Find(m => m.ID_Tag == id);
                if (counter is not null)
                {
                    var tarif = _electricTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
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
        public Task<IEnumerable<ElectricCounter>> GetCountersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var anbieterById = _electricTarifList
                .GroupBy(t => t.Id)
                .ToDictionary(g => g.Key, g => g.Last().Anbieter);

                foreach (var counter in _electricCounterList)
                {
                    if (anbieterById.TryGetValue(counter.ID_Anbieter, out var anbieter))
                    {
                        counter.Anbieter = anbieter ?? string.Empty;
                    }
                }

                return Task.FromResult<IEnumerable<ElectricCounter>>([.. _electricCounterList]);
            }
        }
        #endregion

        #region GetSupplierByIdAsync
        public Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricTarif?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricTarifList.Find(m => m.Id == supplierId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetSuppliersAsync
        public Task<IEnumerable<ElectricTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricTarif>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<ElectricTarif>>([.. _electricTarifList]);
            }
        }
        #endregion

        #region GetPaymentsAsync
        public Task<IEnumerable<ElectricPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricPayment>>(cancellationToken);
            }

            lock (_lock)
            {

                return Task.FromResult<IEnumerable<ElectricPayment>>([.. _electricPaymentList]);
            }
        }
        #endregion

        #region GetPaymentsBySupplierAsync
        public Task<IEnumerable<ElectricPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricPayment>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricPaymentList
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

                return Task.FromResult<IEnumerable<ElectricPayment>>(result);
            }
        }
        #endregion

        #region GetPaymentByIdAsync
        public Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricPayment?>(cancellationToken);
            }

            lock (_lock)
            {
                var payment = _electricPaymentList.Find(m => m.ID_Zahlung == paymentId);
                return Task.FromResult<ElectricPayment?>(payment);
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
                return Task.FromResult<IEnumerable<PaymentMethod>>([.. _electricPaymentMethodList]);
            }
        }
        #endregion

        #region GetCostsBySupplierIdAsync
        public Task<IEnumerable<ElectricCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCost>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricCostsList
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

                return Task.FromResult<IEnumerable<ElectricCost>>(result);
            }
        }
        #endregion

        #region GetCostsAsync
        public Task<IEnumerable<ElectricCost>> GetCostsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCost>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<ElectricCost>>([.. _electricCostsList]);
            }
        }
        #endregion

        #region GetCostByIdAsync
        public Task<ElectricCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCost?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricCostsList.FirstOrDefault(m => m.Id == costId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetCounterChangesAsync
        public Task<IEnumerable<ElectricCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<ElectricCounterChange>>([.. _electricCounterChangeList]);
            }
        }
        #endregion

        #region GetCounterChangeByIdAsync
        public Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounterChange?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricCounterChangeList.FirstOrDefault(m => m.Id == counterChangeId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetCounterChangesBySupplierAsync
        public Task<IEnumerable<ElectricCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<ElectricCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _electricCounterChangeList
                    .Where(m => m?.Id_Anbieter == supplierId)
                    .ToList();

                // Aktuellen Anbieternamen frisch aus der Tarifliste laden
                var tarif = _electricTarifList.FindLast(m => m.Id == supplierId);

                if (tarif is not null)
                {
                    var anbieter = tarif.Anbieter ?? string.Empty;
                    foreach (var counterChange in result)
                    {
                        counterChange.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<ElectricCounterChange>>(result);
            }
        }
        #endregion
        #endregion

        #region UPDATE (Put)
        #region UpdateCounterAsync
        /// <summary>
        /// UpdateCounterAsync
        /// </summary>
        /// <param name="zaehlerstand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounter?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _electricCounterList.FindIndex(m => m.ID_Tag == zaehlerstand.ID_Tag);

                if (index == -1)
                {
                    return Task.FromResult<ElectricCounter?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _electricCounterList[index] = zaehlerstand;

                return Task.FromResult<ElectricCounter?>(zaehlerstand);
            }
        }
        #endregion

        #region UpdateSupplierAsync
        public Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricTarif?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _electricTarifList.FindIndex(m => m.Id == supplier.Id);

                if (index == -1)
                {
                    return Task.FromResult<ElectricTarif?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _electricTarifList[index] = supplier;

                return Task.FromResult<ElectricTarif?>(supplier);
            }
        }
        #endregion

        #region UpdatePaymentAsync
        public Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricPayment?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _electricPaymentList.FindIndex(m => m.ID_Zahlung == payment.ID_Zahlung);

                if (index == -1)
                {
                    return Task.FromResult<ElectricPayment?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _electricPaymentList[index] = payment;

                return Task.FromResult<ElectricPayment?>(payment);
            }
        }
        #endregion

        #region UpdateCostAsync
        public Task<ElectricCost?> UpdateCostAsync(ElectricCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCost?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _electricCostsList.FindIndex(m => m.Id == cost.Id);

                if (index == -1)
                {
                    return Task.FromResult<ElectricCost?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _electricCostsList[index] = cost;

                return Task.FromResult<ElectricCost?>(cost);
            }
        }
        #endregion

        #region UpdateCounterChangeAsync
        public Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<ElectricCounterChange?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _electricCounterChangeList.FindIndex(m => m.Id == counterChange.Id);

                if (index == -1)
                {
                    return Task.FromResult<ElectricCounterChange?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _electricCounterChangeList[index] = counterChange;

                return Task.FromResult<ElectricCounterChange?>(counterChange);
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