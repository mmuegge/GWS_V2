using GWS_Api.Models;
using GWS_Api.Models.Gas;

namespace GWS_Api.Repositories.Gas
{
    public class Mock_GasRepository : IGasRepository
    {
        #region Variablendeklaration
        int id_Tarif = 100;
        int id_Counter = 100;
        int id_Payment = 100;
        int id_Cost = 100;
        int id_BoilerData = 100;
        int id_CounterChange = 100;
        int id_EnergieEff = 100;

        private readonly Lock _lock = new();

        private readonly List<GasTarif> _gasTarifList =
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

        private readonly List<GasCounter> _gasCounterList =
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

        private readonly List<GasPayment> _gasPaymentList =
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

        private readonly List<PaymentMethod> _paymentMethodList =
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

        private readonly List<GasCost> _gasCostsList =
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

        private readonly List<GasBoiler> _gasBoilerDataList =
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

        private readonly List<Efficiency> _gasEnergieEffList =
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
        private readonly List<GasCounterChange> _gasCounterChangeList =
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
            id_Tarif = _gasTarifList.Count;
            id_Counter = _gasCounterList.Count;
            id_Payment = _gasPaymentList.Count;
            id_Cost = _gasCostsList.Count;
            id_BoilerData = _gasBoilerDataList.Count;
            id_CounterChange = _gasCounterChangeList.Count;
        }

        #region ADD
        #region AddCounterAsync
        public Task<GasCounter?> AddCounterAsync(GasCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounter?>(cancellationToken);
            }

            var ablesetagDate = counter.Ablesetag?.Date;
            if (ablesetagDate is null || ablesetagDate > DateTime.Today)
            {
                return Task.FromResult<GasCounter?>(null);
            }

            lock (_lock)
            {
                bool exists = _gasCounterList.Exists(m =>
                    m.Ablesetag?.Date == ablesetagDate && m.ID_Anbieter == counter.ID_Anbieter);

                if (exists)
                {
                    return Task.FromResult<GasCounter?>(null);
                }

                var tarif = _gasTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                if (tarif is not null)
                {
                    counter.Anbieter = tarif.Anbieter ?? string.Empty;
                }

                counter.ID_Tag = ++id_Counter;
                counter.Ablesetag = ablesetagDate;

                _gasCounterList.Add(counter);
            }

            return Task.FromResult<GasCounter?>(counter);
        }
        #endregion

        #region AddSupplierAsync
        public Task<GasTarif> AddSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasTarif>(cancellationToken);
            }

            lock (_lock)
            {
                supplier.Id = ++id_Tarif;
                _gasTarifList.Add(supplier);
            }

            return Task.FromResult(supplier);
        }
        #endregion

        #region AddPaymentAsync
        public Task<GasPayment> AddPaymentAsync(GasPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasPayment>(cancellationToken);
            }

            lock (_lock)
            {
                payment.ID_Zahlung = ++id_Payment;
                _gasPaymentList.Add(payment);
            }

            return Task.FromResult(payment);
        }
        #endregion

        #region AddCostAsync
        public Task<GasCost> AddCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCost>(cancellationToken);
            }

            lock (_lock)
            {
                cost.Id = ++id_Cost;
                _gasCostsList.Add(cost);
            }

            return Task.FromResult(cost);
        }
        #endregion

        #region AddBoilerDataAsync
        public Task<GasBoiler> AddBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default)
        { 
            ArgumentNullException.ThrowIfNull(boilerData);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasBoiler>(cancellationToken);
            }

            lock (_lock)
            {
                boilerData.Id = ++id_BoilerData;
                _gasBoilerDataList.Add(boilerData);
            }

            return Task.FromResult(boilerData);
        }
        #endregion

        #region AddCounterChangeAsync
        public Task<GasCounterChange> AddCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounterChange>(cancellationToken);
            }

            lock (_lock)
            {
                counterChange.Id = ++id_CounterChange;
                _gasCounterChangeList.Add(counterChange);
            }

            return Task.FromResult(counterChange);
        }
        #endregion

        #region AddEnergieEfficiencyAsync
        public Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Efficiency>(cancellationToken);
            }

            lock (_lock)
            {
                eff.Id = ++id_EnergieEff;
                _gasEnergieEffList.Add(eff);
            }

            return Task.FromResult(eff);
        }
        #endregion
        #endregion

        #region DELETE
        #region DeleteCounterAsync
        public Task DeleteCounterAsync(GasCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasCounterList.RemoveAll(m => m.ID_Tag == counter.ID_Tag);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteSupplierAsync
        public Task DeleteSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasCostsList.RemoveAll(m => m.Id == supplier.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeletePaymentAsync
        public Task DeletePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasCostsList.RemoveAll(m => m.Id == payment.ID_Zahlung);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCostAsync
        public Task DeleteCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasCostsList.RemoveAll(m => m.Id == cost.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteCounterChangeAsync
        public Task DeleteCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasCounterChangeList.RemoveAll(m => m.Id == counterChange.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteBoilerDataAsync
        public Task DeleteBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(boilerData);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasBoilerDataList.RemoveAll(m => m.Id == boilerData.Id);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteEnergieEfficiencyAsync
        public Task DeleteEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (_lock)
            {
                _gasEnergieEffList.RemoveAll(m => m.Id == eff.Id);
            }

            return Task.CompletedTask;
        }
        #endregion
        #endregion

        #region GET
        #region GetCounterByDateAsync
        public Task<GasCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounter?>(cancellationToken);
            }

            lock (_lock)
            {

                var counter = _gasCounterList.Find(m => m.Ablesetag?.Date == date.Date);
                if (counter is not null)
                {
                    var tarif = _gasTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
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
        public Task<IEnumerable<GasCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasCounterList.FindAll(m => m.ID_Anbieter == supplierId);
                if (result.Count > 0)
                {
                    var supplier = _gasTarifList.FindLast(m => m.Id == supplierId);
                    if (supplier is not null)
                    {
                        foreach (var counter in result)
                        {
                            counter.Anbieter = supplier.Anbieter;
                        }
                    }
                }

                return Task.FromResult<IEnumerable<GasCounter>>(result);
            }
        }
        #endregion

        #region GetCounterByIdAsync
        public Task<GasCounter?> GetCounterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounter?>(cancellationToken);
            }

            lock (_lock)
            {
                var counter = _gasCounterList.Find(m => m.ID_Tag == id);
                if (counter is not null)
                {
                    var tarif = _gasTarifList.FindLast(m => m.Id == counter.ID_Anbieter);
                    if (tarif is not null)
                    {
                        counter.Anbieter = tarif.Anbieter;
                    }
                }

                return Task.FromResult(counter);
            }
        }
        #endregion

        #region GetCountersAsync
        public Task<IEnumerable<GasCounter>> GetCountersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCounter>>(cancellationToken);
            }

            lock (_lock)
            {
                var anbieterById = _gasTarifList
                    .GroupBy(t => t.Id)
                    .ToDictionary(g => g.Key, g => g.Last().Anbieter);

                foreach (var counter in _gasCounterList)
                {
                    if (anbieterById.TryGetValue(counter.ID_Anbieter, out var anbieter))
                    {
                        counter.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<GasCounter>>([.. _gasCounterList]);
            }
        }
        #endregion

        #region GetSupplierByIdAsync
        public Task<GasTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasTarif?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasTarifList.Find(m => m.Id == supplierId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetSuppliersAsync
        public Task<IEnumerable<GasTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasTarif>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<GasTarif>>([.. _gasTarifList]);
            }
        }
        #endregion

        #region GetPaymentsAsync
        public Task<IEnumerable<GasPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasPayment>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<GasPayment>>([.. _gasPaymentList]);
            }
        }
        #endregion

        #region GetPaymentsBySupplierAsync
        public Task<IEnumerable<GasPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasPayment>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasPaymentList
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

                return Task.FromResult<IEnumerable<GasPayment>>(result);
            }
        }
        #endregion

        #region GetPaymentByIdAsync
        public Task<GasPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasPayment?>(cancellationToken);
            }

            lock (_lock)
            {
                var payment = _gasPaymentList.Find(m => m.ID_Zahlung == paymentId);
                return Task.FromResult<GasPayment?>(payment);
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
                return Task.FromResult<IEnumerable<PaymentMethod>>([.. _paymentMethodList]);
            }
        }
        #endregion

        #region GetCostsBySupplierIdAsync
        public Task<IEnumerable<GasCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCost>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasCostsList
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

                return Task.FromResult<IEnumerable<GasCost>>(result);
            }
        }
        #endregion

        #region GetCostsAsync
        public Task<IEnumerable<GasCost>> GetCostsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCost>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<GasCost>>([.. _gasCostsList]);
            }
        }
        #endregion

        #region GetCostByIdAsync
        public Task<GasCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default)
        {
            //var cost = _gasCostsList.Find(m => m.Id == costId);
            //return await Task.FromResult(cost);
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCost?>(cancellationToken);
            }

            var result = _gasCostsList.FirstOrDefault(m => m.Id == costId);
            return Task.FromResult(result);
        }
        #endregion

        #region GetCounterChangesAsync
        public Task<IEnumerable<GasCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<GasCounterChange>>([.. _gasCounterChangeList]);
            }
        }
        #endregion

        #region GetCounterChangeByIdAsync
        public Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounterChange?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasCounterChangeList.FirstOrDefault(m => m.Id == counterChangeId);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetCounterChangesBySupplierAsync
        public Task<IEnumerable<GasCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasCounterChange>>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasCounterChangeList
                    .Where(m => m?.Id_Anbieter == supplierId)
                    .ToList();

                // Aktuellen Anbieternamen frisch aus der Tarifliste laden
                var tarif = _gasTarifList.FindLast(m => m.Id == supplierId);

                if (tarif is not null)
                {
                    var anbieter = tarif.Anbieter ?? string.Empty;
                    foreach (var counterChange in result)
                    {
                        counterChange.Anbieter = anbieter;
                    }
                }

                return Task.FromResult<IEnumerable<GasCounterChange>>(result);
            }
        }
        #endregion

        #region GetBoilerDataAsync
        public Task<IEnumerable<GasBoiler>> GetBoilerDataAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<GasBoiler>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<GasBoiler>>([.. _gasBoilerDataList]);
            }
        }
        #endregion

        #region GetBoilerDataByIdAsync
        public Task<GasBoiler?> GetBoilerDataByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasBoiler?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasBoilerDataList.FirstOrDefault(m => m.Id == id);
                return Task.FromResult(result);
            }
        }
        #endregion

        #region GetEnergieEfficiencyAsync
        public Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<Efficiency>>(cancellationToken);
            }

            lock (_lock)
            {
                return Task.FromResult<IEnumerable<Efficiency>>([.. _gasEnergieEffList]);
            }
        }
        #endregion

        #region GetEnergieEfficiencyByIdAsync
        public Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Efficiency?>(cancellationToken);
            }

            lock (_lock)
            {
                var result = _gasEnergieEffList.FirstOrDefault(m => m.Id == id);
                return Task.FromResult(result);
            }
        }
        #endregion
        #endregion

        #region UPDATE (Put)
        #region UpdateCounterAsync
        public Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounter?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasCounterList.FindIndex(m => m.ID_Tag == zaehlerstand.ID_Tag);

                if (index == -1)
                {
                    return Task.FromResult<GasCounter?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasCounterList[index] = zaehlerstand;

                return Task.FromResult<GasCounter?>(zaehlerstand);
            }
        }
        #endregion

        #region UpdateSupplierAsync
        public Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasTarif?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasTarifList.FindIndex(m => m.Id == supplier.Id);

                if (index == -1)
                {
                    return Task.FromResult<GasTarif?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasTarifList[index] = supplier;

                return Task.FromResult<GasTarif?>(supplier);
            }
        }
        #endregion

        #region UpdatePaymentAsync
        public Task<GasPayment?> UpdatePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default) // not used
        {
            ArgumentNullException.ThrowIfNull(payment);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasPayment?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasPaymentList.FindIndex(m => m.ID_Zahlung == payment.ID_Zahlung);

                if (index == -1)
                {
                    return Task.FromResult<GasPayment?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasPaymentList[index] = payment;

                return Task.FromResult<GasPayment?>(payment);
            }
        }
        #endregion

        #region UpdateCostAsync
        public Task<GasCost?> UpdateCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCost?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasCostsList.FindIndex(m => m.Id == cost.Id);

                if (index == -1)
                {
                    return Task.FromResult<GasCost?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasCostsList[index] = cost;

                return Task.FromResult<GasCost?>(cost);
            }
        }
        #endregion


        #region
        public Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default) // not used
        {
            ArgumentNullException.ThrowIfNull(boilerData);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasBoiler?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasBoilerDataList.FindIndex(m => m.Id == boilerData.Id);

                if (index == -1)
                {
                    return Task.FromResult<GasBoiler?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasBoilerDataList[index] = boilerData;

                return Task.FromResult<GasBoiler?>(boilerData);
            }
        }
        #endregion

        #region UpdateCounterChangeAsync
        public Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<GasCounterChange?>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasCounterChangeList.FindIndex(m => m.Id == counterChange.Id);

                if (index == -1)
                {
                    return Task.FromResult<GasCounterChange?>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasCounterChangeList[index] = counterChange;

                return Task.FromResult<GasCounterChange?>(counterChange);
            }
        }
        #endregion

        #region
        public Task<Efficiency> UpdateEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Efficiency>(cancellationToken);
            }

            // Optional: Lock verwenden, falls mehrere Threads gleichzeitig auf die Liste zugreifen können
            lock (_lock)
            {
                var index = _gasEnergieEffList.FindIndex(m => m.Id == eff.Id);

                if (index == -1)
                {
                    return Task.FromResult<Efficiency>(null);
                }

                // Ersetzt das komplette Objekt auf einen Schlag
                _gasEnergieEffList[index] = eff;

                return Task.FromResult<Efficiency>(eff);
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