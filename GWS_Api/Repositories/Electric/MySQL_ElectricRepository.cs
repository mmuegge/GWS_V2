using GWS_Api.Models;
using GWS_Api.Models.Electric;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Electric
{
    public class MySQL_ElectricRepository(GWS_DbContext context) : IElectricRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context = context;
        #endregion

        #region ADD
        public async Task<ElectricTarif?> AddSupplierAsync(ElectricTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Strom_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<ElectricCounter?> AddCounterAsync(ElectricCounter zaehlerstand)
        {
            // prüfen ob Ablesedatum in der Zukunft liegt
            DateTime heute = DateTime.Now;
            if (zaehlerstand.Ablesetag > heute)
            {
                return null;
            }
            // prüfen ob Ablesetag schon vorhanden ist
            int count = await _context.Strom_zaehlerstand.Where(m => m.Ablesetag == zaehlerstand.Ablesetag).CountAsync();
            if (count > 0)
            {
                return null;
            }

            var result = await _context.Strom_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<ElectricPayment?> AddPaymentAsync(ElectricPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Strom_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<ElectricCost?> AddCostAsync(ElectricCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Strom_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<ElectricCounterChange?> AddCounterChangeAsync(ElectricCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Strom_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(ElectricTarif supplier)
        {
            var result = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id) ?? throw new ArgumentNullException(nameof(supplier));
            _context.Remove(supplier);
        }
        public async Task DeleteCounterAsync(ElectricCounter counter)
        {
            var result = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == counter.Ablesetag) ?? throw new ArgumentNullException(nameof(counter));
            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(ElectricPayment payment)
        {
            var result = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.Zahlungen == payment.Zahlungen) ?? throw new ArgumentNullException(nameof(payment));
            _context.Remove(result);
        }
        public async Task DeleteCostAsync(ElectricCost cost)
        {
            var result = await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id) ?? throw new ArgumentNullException(nameof(cost));
            _context.Remove(result);
        }
        public async Task DeleteCounterChangeAsync(ElectricCounterChange counterChange)
        {
            var result = await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id) ?? throw new ArgumentNullException(nameof(counterChange));
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<ElectricTarif?>> GetSuppliersAsync()
        {
            return await _context.Strom_tarif.ToListAsync();
        }
        public async Task<IEnumerable<ElectricCounter?>> GetCountersAsync()
        {
            var counters = await _context.Strom_zaehlerstand.ToListAsync();
            var tarifList = await _context.Strom_tarif.ToListAsync();
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
        public async Task<IEnumerable<ElectricCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Strom_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counter in result)
                {
                    counter.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<ElectricCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }

        public async Task<ElectricCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Strom_zahlungen.ToListAsync();
            var tarifList = await _context.Strom_tarif.ToListAsync();
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
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Strom_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);

            return await Task.FromResult(payment);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            var paymentMethods = await _context.Zahlungsarten.ToListAsync();

            return await Task.FromResult(paymentMethods);
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsAsync()
        {
            var costs = await _context.Strom_kosten.ToListAsync();

            return await Task.FromResult(costs);
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Strom_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Strom_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<ElectricCost?> GetCostByIdAsync(int costId)
        {
            var cost = await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == costId);

            return await Task.FromResult(cost);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesAsync()
        {
            var counterChanges = await _context.Strom_zaehlerwechsel.ToListAsync();

            return await Task.FromResult(counterChanges);
        }
        public async Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterchange = await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);

            return await Task.FromResult(counterchange);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Strom_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        #endregion

        #region UPDATE
        public async Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier)
        {
            var result = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);

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
                result.Start_Zaehlerstand_280 = supplier.Start_Zaehlerstand_280;
                result.Ende_Zaehlerstand_280 = supplier.Ende_Zaehlerstand_280;
                result.Start_Zaehlerstand_Enfluri = supplier.Start_Zaehlerstand_Enfluri;
                result.Ende_Zaehlerstand_Enfluri = supplier.Ende_Zaehlerstand_Enfluri;
                result.Arbeitspreis = supplier.Arbeitspreis;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Anzahl_Personen = supplier.Anzahl_Personen;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand)
        {
            var result = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);

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
        public async Task<ElectricCost?> UpdateCostAsync(ElectricCost cost)
        {
            var result = await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);

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
        public async Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment)
        {
            var result = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);

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
        public async Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange)
        {
            var result = await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);

            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Zaehlerstand_280_alt = counterChange.Zaehlerstand_280_alt;
                result.Zaehlerstand_280_neu = counterChange.Zaehlerstand_280_neu;
                result.Zaehlerstand_Enfluri_alt = counterChange.Zaehlerstand_Enfluri_alt;
                result.Zaehlerstand_Enfluri_neu = counterChange.Zaehlerstand_Enfluri_neu;
                result.Bemerkung = counterChange.Bemerkung;
            }

            return await Task.FromResult(result!);
        }
        #endregion

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);    // return count of state entries written to DB
        }



        // nur zum Testen
        //public async Task<IEnumerable<ElectricTarif>> SearchElectric(string tarif, double? zaehlermiete)
        //{
        //    IQueryable<ElectricTarif> query = _context.Strom_tarif;
        //    if (!string.IsNullOrEmpty(tarif))
        //    {
        //        //query = query.Where(m => m.Tarif.Contains(tarif) || m.Grundpreis == 0.0);
        //        query = query.Where(m => m.Tarif.Contains(tarif));
        //    }
        //    // if (zaehlermiete != null)
        //    // {
        //    //   query = query.Where(m => m.Zaehlermiete == zaehlermiete);
        //    // }
        //    return await query.ToListAsync();

        //}
    }

}