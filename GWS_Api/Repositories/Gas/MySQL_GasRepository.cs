using GWS_Api.Models;
using GWS_Api.Models.Gas;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Gas
{
    public class MySQL_GasRepository : IGasRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_GasRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion

        #region ADD
        public async Task<GasTarif?> AddSupplierAsync(GasTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Gas_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<GasCounter?> AddCounterAsync(GasCounter zaehlerstand)
        {
            // prüfen ob Ablesedatum in der Zukunft liegt
            DateTime heute = DateTime.Now;
            if (zaehlerstand.Ablesetag > heute)
            {
                return null;
            }
            // prüfen ob Ablesetag schon vorhanden ist
            int count = await _context.Gas_zaehlerstand.Where(m => m.Ablesetag == zaehlerstand.Ablesetag).CountAsync();
            if (count > 0)
            {
                return null;
            }
            var result = await _context.Gas_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<GasPayment?> AddPaymentAsync(GasPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Gas_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<GasCost?> AddCostAsync(GasCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Gas_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<GasBoiler?> AddBoilerDataAsync(GasBoiler boilerData)
        {
            ArgumentNullException.ThrowIfNull(boilerData);
            var result = await _context.Gas_therme.AddAsync(boilerData);
            return result.Entity;
        }
        public async Task<Efficiency?> AddEnergieEfficiencyAsync(Efficiency eff)
        {
            ArgumentNullException.ThrowIfNull(eff);
            var result = await _context.Energie_effizienz.AddAsync(eff);
            return result.Entity;
        }
        public async Task<GasCounterChange?> AddCounterChangeAsync(GasCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Gas_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(GasTarif supplier)
        {
            var result = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(supplier));
            }
            _context.Remove(supplier);
        }
        public async Task DeleteCounterAsync(GasCounter counter)
        {
            var result = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == counter.Ablesetag);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }
            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(GasPayment payment)
        {
            var result = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.Zahlungen == payment.Zahlungen);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }
            _context.Remove(result);
        }
        public async Task DeleteCostAsync(GasCost cost)
        {
            var result = await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(cost));
            }
            _context.Remove(result);
        }
        public async Task DeleteBoilerDataAsync(GasBoiler boilerData)
        {
            var result = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == boilerData.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(boilerData));
            }
            _context.Remove(result);
        }
        public async Task DeleteEnergieEfficiencyAsync(Efficiency eff)
        {
            var result = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == eff.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(eff));
            }
            _context.Remove(result);
        }
        public async Task DeleteCounterChangeAsync(GasCounterChange counterChange)
        {
            var result = await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id) ?? throw new ArgumentNullException(nameof(counterChange));
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<GasTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<GasTarif?>> GetSuppliersAsync()
        {
            return await _context.Gas_tarif.ToListAsync();
        }
        public async Task<IEnumerable<GasCounter?>> GetCountersAsync()
        {
            var counters = await _context.Gas_zaehlerstand.ToListAsync();
            var tarifList = await _context.Gas_tarif.ToListAsync();
            var supplierName = string.Empty;
            int id = 0;

            foreach (var counter in counters)
            {
                if (counter.ID_Anbieter != id)
                {
                    id = counter.ID_Anbieter;
                    supplierName = tarifList.Where(m => m.Id == counter.ID_Anbieter).Select(x => x.Anbieter).FirstOrDefault();
                }
                counter.Anbieter = supplierName;
            }
            return await Task.FromResult(counters);
        }

        public async Task<IEnumerable<GasCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Gas_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counter in result)
                {
                    counter.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }

        public async Task<GasCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }

        public async Task<GasCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Gas_zahlungen.ToListAsync();
            var tarifList = await _context.Gas_tarif.ToListAsync();
            var supplierName = string.Empty;
            int id = 0;

            foreach (var payment in payments)
            {
                if (payment.ID_Anbieter != id)
                {
                    id = payment.ID_Anbieter;
                    supplierName = tarifList.Where(m => m.Id == payment.ID_Anbieter).Select(x => x.Anbieter).FirstOrDefault();
                }
                payment.Anbieter = supplierName;
            }
            return await Task.FromResult(payments);
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Gas_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<GasPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);
            //if (payment != null)
            //{

            //    List<PaymentMethod> paymentMethods = await _context.Zahlungsarten.ToListAsync();
            //    var method = await _context.Zahlungsarten.FirstOrDefaultAsync(m => m.ID_Zahlungsart == paymentId);
            //    payment.Zahlungsart = method.Zahlungsart;
            //}
            return await Task.FromResult(payment);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            var paymentMethods = await _context.Zahlungsarten.ToListAsync();

            return await Task.FromResult(paymentMethods);
        }
        public async Task<IEnumerable<GasCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Gas_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Gas_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<IEnumerable<GasCost?>> GetCostsAsync()
        {
            var costs = await _context.Gas_kosten.ToListAsync();

            return await Task.FromResult(costs);
        }
        public async Task<GasCost?> GetCostByIdAsync(int costId)
        {
            var cost = await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == costId);

            return await Task.FromResult(cost);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesAsync()
        {
            var counterChanges = await _context.Gas_zaehlerwechsel.ToListAsync();

            return await Task.FromResult(counterChanges);
        }
        public async Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterChange = await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);

            return await Task.FromResult(counterChange);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Gas_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<IEnumerable<GasBoiler?>> GetBoilerDataAsync()
        {
            var boilerData = await _context.Gas_therme.ToListAsync();

            return await Task.FromResult(boilerData);
        }
        public async Task<GasBoiler?> GetBoilerDataByIdAsync(int id)
        {
            var boilerData = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == id);

            return await Task.FromResult(boilerData);
        }
        public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync()
        {
            var eff = await _context.Energie_effizienz.ToListAsync();

            return await Task.FromResult(eff);
        }
        public async Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id)
        {
            var eff = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == id);

            return await Task.FromResult(eff);
        }
        #endregion

        #region UPDATE
        public async Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier)
        {
            var result = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);

            if (result != null)
            {
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
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Brennwert = supplier.Brennwert;
                result.Heizleistung = supplier.Heizleistung;
                result.Zustandszahl = supplier.Zustandszahl;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand)
        {
            var result = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);

            if (result != null)
            {
                result.ID_Anbieter = zaehlerstand.ID_Anbieter;
                result.Anbieter = zaehlerstand.Anbieter;
                result.Ablesetag = zaehlerstand.Ablesetag;
                result.Zaehlerstand = zaehlerstand.Zaehlerstand;
                result.Uhrzeit = zaehlerstand.Uhrzeit;
                result.Temperatur_aussen = zaehlerstand.Temperatur_aussen;
                result.Temperatur_innen = zaehlerstand.Temperatur_innen;
                result.Bemerkungen = zaehlerstand.Bemerkungen;
                return result;
            }
            return null;
        }
        public async Task<GasPayment?> UpdatePaymentAsync(GasPayment payment)
        {
            var result = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);

            if (result != null)
            {
                result.ID_Zahlung = payment.ID_Zahlung;
                result.ID_Anbieter = payment.ID_Anbieter;
                result.Anbieter = payment.Anbieter;
                result.Datum = payment.Datum;
                result.Zahlungsart = payment.Zahlungsart;
                result.Zahlungen = payment.Zahlungen;
                result.Bemerkungen = payment.Bemerkungen;
                return result;
            }
            return null;
        }
        public async Task<GasCost?> UpdateCostAsync(GasCost cost)
        {
            var result = await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);

            if (result != null)
            {
                result.Id = cost.Id;
                result.Id_Anbieter = cost.Id_Anbieter;
                result.Anbieter = cost.Anbieter;
                result.Grundpreis = cost.Grundpreis;
                result.Arbeitspreis = cost.Arbeitspreis;
                result.Zaehlermiete = cost.Zaehlermiete;
                result.Bemerkung = cost.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        public async Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData)
        {
            var result = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == boilerData.Id);

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
        public async Task<Efficiency?> UpdateEnergieEfficiencyAsync(Efficiency eff)
        {
            var result = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == eff.Id);

            if (result != null)
            {
                result.Id = eff.Id;
                result.Energieklasse = eff.Energieklasse;
                result.Energiebedarf = eff.Energiebedarf;
                result.Farbcode = eff.Farbcode;
                result.Bemerkung = eff.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        public async Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange)
        {
            var result = await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);

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

            return await Task.FromResult(result);
        }
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);    // return count of state entries written to DB
        }
        #endregion

        // nur zum Testen
        public async Task<IEnumerable<GasTarif?>> SearchGas(string gasTarif, double? zaehlermiete)
        {
            var query = _context.Gas_tarif;
            if (!string.IsNullOrEmpty(gasTarif))
            {
                //query = query.Where(m => m.Tarif.Contains(tarif) || m.Grundpreis == 0.0);
                //var lulli = query.Where(m => m.Tarif.Contains(gasTarif));
            }
            // if (zaehlermiete != null)
            // {
            //   query = query.Where(m => m.Zaehlermiete == zaehlermiete);
            // }
            return await query.ToListAsync();

        }
    }
}