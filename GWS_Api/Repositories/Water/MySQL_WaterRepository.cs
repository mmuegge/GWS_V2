using GWS_Api.Models;
using GWS_Api.Models.Water;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Water
{
    public class MySQL_WaterRepository : IWaterRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_WaterRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion

        #region ADD
        public async Task<WaterTarif> AddSupplierAsync(WaterTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Wasser_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<WaterCounter?> AddCounterAsync(WaterCounter zaehlerstand)
        {
            // prüfen ob Ablesedatum in der Zukunft liegt
            DateTime heute = DateTime.Now;
            if (zaehlerstand.Ablesetag > heute)
            {
                return null;
            }
            // prüfen ob Ablesetag schon vorhanden ist
            int count = await _context.Wasser_zaehlerstand.Where(m => m.Ablesetag == zaehlerstand.Ablesetag).CountAsync();
            if (count > 0)
            {
                return null;
            }

            var result = await _context.Wasser_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<WaterPayment> AddPaymentAsync(WaterPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Wasser_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<WaterCost> AddCostAsync(WaterCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Wasser_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Wasser_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(WaterTarif supplier)
        {
            var result = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(supplier));
            }
            _context.Remove(result);
        }
        public async Task DeleteCounterAsync(WaterCounter counter)
        {
            var result = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == counter.Ablesetag);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }
            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(WaterPayment payment)
        {
            var result = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.Zahlungen == payment.Zahlungen);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }
            _context.Remove(result);
        }
        public async Task DeleteCostAsync(WaterCost cost)
        {
            var result = await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(cost));
            }
            _context.Remove(result);
        }
        public async Task DeleteCounterChangeAsync(WaterCounterChange counterChange)
        {
            var result = await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id) ?? throw new ArgumentNullException(nameof(counterChange));
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<WaterTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<WaterTarif>> GetSuppliersAsync()
        {
            return await _context.Wasser_tarif.ToListAsync();
        }
        public async Task<IEnumerable<WaterCounter?>> GetCountersAsync()
        {
            var counters = await _context.Wasser_zaehlerstand.ToListAsync();
            var tarifList = await _context.Wasser_tarif.ToListAsync();
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
        public async Task<IEnumerable<WaterCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Wasser_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counter in result)
                {
                    counter.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }

        public async Task<WaterCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }

        public async Task<WaterCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter;
            }
            return await Task.FromResult(counter);
        }
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Wasser_zahlungen.ToListAsync();
            var tarifList = await _context.Wasser_tarif.ToListAsync();
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
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Wasser_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<WaterPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);

            return await Task.FromResult(payment);
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            List<PaymentMethod> paymentMethods = await _context.Zahlungsarten.ToListAsync();

            return await Task.FromResult(paymentMethods);
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsAsync()
        {
            List<WaterCost> costs = await _context.Wasser_kosten.ToListAsync();

            return await Task.FromResult(costs);
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesAsync()
        {
            List<WaterCounterChange> counterChanges = await _context.Wasser_zaehlerwechsel.ToListAsync();

            return await Task.FromResult(counterChanges);
        }
        public async Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            var counterChange = await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);

            return await Task.FromResult(counterChange);
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Wasser_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Wasser_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<WaterCost?> GetCostByIdAsync(int costId)
        {
            var cost = await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == costId);

            return await Task.FromResult(cost);
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            if (result != null)
            {
                var supplierName = await _context.Wasser_tarif.Where(m => m.Id == supplierId).Select(x => x.Anbieter).FirstOrDefaultAsync();
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName;
                }
            }
            return await Task.FromResult(result!);
        }
        public async Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange)
        {
            var result = await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);

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

            return await Task.FromResult(result);
        }
        #endregion

        #region UPDATE
        public async Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier)
        {
            var result = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);

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
                result.Start_Zaehlerstand_aussen = supplier.Start_Zaehlerstand_aussen;
                result.Ende_Zaehlerstand_aussen = supplier.Ende_Zaehlerstand_aussen;
                result.Trinkwasserpreis = supplier.Trinkwasserpreis;
                result.Verbrauch_Trinkwasser = supplier.Verbrauch_Trinkwasser;
                result.Abwasserpreis = supplier.Abwasserpreis;
                result.Verbrauch_Abwasser = supplier.Verbrauch_Abwasser;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand)
        {
            var result = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);

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
                result.Zaehlerstand_aussen = zaehlerstand.Zaehlerstand_aussen;
                return result;
            }
            return null;
        }
        public async Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment)
        {
            var result = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);

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
        public async Task<WaterCost?> UpdateCostAsync(WaterCost cost)
        {
            var result = await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);

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
                return result;
            }

            return null;
        }
        #endregion

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);    // return count of state entries written to DB
        }

        // nur zum Testen
        public async Task<IEnumerable<WaterTarif>> Search(string tarif, double? zaehlermiete)
        {
            IQueryable<WaterTarif> query = _context.Wasser_tarif;
            if (!string.IsNullOrEmpty(tarif))
            {
                //query = query.Where(m => m.Tarif.Contains(tarif) || m.Grundpreis == 0.0);
                query = query.Where(m => m.Tarif!.Contains(tarif));
            }
            if (zaehlermiete != null)
            {
                query = query.Where(m => m.Zaehlermiete == zaehlermiete);
            }
            return await query.ToListAsync();

        }

    }

}